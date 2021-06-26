using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JiroController : Enemy
{
    private int count_cin = 0;
    private enum enum_actions { Jump = 1, Arm = 2, EnergyBall = 3, Earthquake = 4, Null = 0 }
    private enum enum_animations { Idle = 0, Walk = 1, Attack = 2, Jump_1 = 3, Jump_2 = 4, EnergyBall_1 = 5, EnergyBall_2 = 6, Earthquake = 7, Dead = 8 }
    private enum enum_cor { Attack, Earthquake, PrepareEnergyball, JumpToOrigin, Jump, EndEnergyball, EndJump, CameraShake, AttackSound, JumpSound, Default }
    private enum enum_sounds { PrepareEnergyball = 0, Jump = 1, Dead = 2, Attack =3, EarthQuake =4}
    [SerializeField] GameObject projectile, rock, explosion, cam;
    [SerializeField] Animator animator;
    [SerializeField] int minRockNumber = 9, maxRockNumber = 12;

    private Dictionary<enum_actions, int> actions = new Dictionary<enum_actions, int>();
    private BoxCollider armCollider;
    private enum_actions currentAction;
    private Rigidbody rb;
    private Vector3 yForce, originalPos, yForceReturning = 8.0f * Vector3.up;
    private bool mustLookAtPlayer = false, flag_run_towards_player = true, canExplodeFloor = false, willUseEnergyAttack = false, flag_shake_camera = false;
    private float yFloatForce = 15.0f, xFloatForce = 0f, constantXForce = 3.04f, shakingForce = 0.5f, rockRad = 3f;
    private int mustChangeAction = 0;
    public AudioSource audiosource;
    [SerializeField] AudioClip[] sonidos;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("playerCam");
        rb = GetComponent<Rigidbody>();
        health = GameConstants.Jiro_HP;
        originalPos = transform.position;

        // El searchRadius debe ser lo suficientemente grande como para abarcar todo el área del combate
        searchRadius = 300;
        attackRadius = 6;

        // Collider del brazo. Solo estará activo cuando esté haciendo el ataque físico
        armCollider = GetComponentInChildren<BoxCollider>();
        armCollider.enabled = false;

        changeAnimation(enum_animations.Idle);
        
        // Peso de cada acción (cuanto más peso, más probable que lo haga)
        actions.Add(enum_actions.Arm, 3);
        actions.Add(enum_actions.Jump, 5);
        actions.Add(enum_actions.Earthquake, 5);
        actions.Add(enum_actions.EnergyBall, 2);

        // Esto es para el ataque del salto. La altura está calculada "a mano"
        yForce = yFloatForce * Vector3.up;

        //Jiro permanece inactivo al inicio (cinematica)
        isDead = true;
        StartCoroutine(cor_EndCinematic(20f));
    }

    protected override void Awake()
    {
        base.Awake();
        healthBarClone = healthBarGO;
        healthBar = healthBarClone.GetComponentsInChildren<Image>()[1];
        healthBarClone.SetActive(false);
    }



    protected override void FixedUpdate() {
       // healthBarClone.transform.position = healthBar.transform.position;
        //healthBarClone.transform.rotation = healthBar.transform.rotation;
    }
    private new void Update()
    {
        if (Input.GetKeyDown(GameConstants.key_interact) && count_cin == 0 && SceneManager.GetActiveScene().name.Equals("FinalBoss"))
        {
            StopCoroutine(cor_EndCinematic(17.5f));
            player.GetComponent<Player>().isDead = false;
            isDead = false;
            count_cin++;
            healthBarClone.SetActive(true);
        }
        // Si no está muerto y está activo (de momento se activa al pulsar una tecla)...
        if (!isDead)
        {
            // Mientra sea true, Jiro mirará a la posición del jugador PERO la 'y' será la actual (mirará de frente)
            if (mustLookAtPlayer)
            {
                lookAtPlayer();
            }
            // Movimiento de la cámara para simular un impacto fuerte (efecto terremoto)
            if (flag_shake_camera)
            {
                cam.transform.position += new Vector3(0, shakingForce, 0);
                shakingForce = -shakingForce;
            }
            // Si no hay ninguna acción llevándose a cabo, se ha de asignar una nueva
            if (mustChangeAction == 0)
            {
                mustChangeAction++;
                currentAction = ExtRandom.ChooseWeighted(actions);
                //currentAction = enum_actions.Earthquake;
                // Se dejan solo las restricciones angulares. Esto se hace porque en alguna acción es necesario restringir
                // la posición de Jiro
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                // El NavMeshAgent solo debe estar activado para la acción "Attack"
                agent.enabled = false;

                // Si sale la acción de perseguir al jugador, se define ya el tiempo para acabar dicha acción pues esta
                // se ejecuta continuamente en el Update a diferencia de los otros, que solo se activan 1 vez
                if (currentAction.Equals(enum_actions.Arm))
                {
                    agent.enabled = true;
                    StartCoroutine(cor_actions(14f));
                }
            }
            if (currentAction.Equals(enum_actions.Arm))
            {
                searchPlayer();
                return;
            }
            else if (!currentAction.Equals(enum_actions.Null)) // Solo ocurre una vez cuando se decide la nueva acción
            {
                // Desactivo el collider del brazo para que no colisione con otros elementos (bola mágica, explosión, etc.)
                armCollider.enabled = false;
                switch (currentAction)
                {
                    case enum_actions.Earthquake:
                        StartCoroutine(cor_actions(0, enum_cor.Earthquake));
                        break;
                    case enum_actions.EnergyBall:
                        StartCoroutine(cor_actions(0, enum_cor.JumpToOrigin));
                        break;
                    case enum_actions.Jump:
                        StartCoroutine(cor_actions(0, enum_cor.Jump));
                        break;
                    default:
                        return;
                }
                currentAction = enum_actions.Null;
            }
        }      
    }
    private void playSound(enum_sounds sonido)
    {
        audiosource.PlayOneShot(sonidos[(int)sonido]);
    }

    // Para cambiar la animación se elije la id y se activas el trigger
    private void changeAnimation(enum_animations anim)
    {
        animator.SetInteger("id", (int)anim);
        animator.SetTrigger("canBeginAnimation");
    }

    protected override void searchPlayer()
    {
        // Si el cooldown es true, significa que estoy en medio del ataque del brazo. Hasta que no acabe, no pasará nada.
        if (!cooldown)
        {
            // Distancia Jiro - Jugador
            Vector3 pos1 = transform.position;
            Vector3 pos2 = player.transform.position;
            int distance = (int)Vector3.Distance(pos1, pos2);

            // Si no está en rango de ataque...
            if (distance > attackRadius)
            {
                // Con el flag hago que para la acción de perseguir al jugador se ejecute una vez y lo único
                // que ocurre en cada frame es actualizar la posición del mismo
                if (flag_run_towards_player)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    // En el momento en el que se pone en modo búsqueda, desactivo el collider del brazo
                    armCollider.enabled = false;
                    changeAnimation(enum_animations.Walk);
                    agent.isStopped = false;
                    flag_run_towards_player = false;
                }
                agent.SetDestination(player.transform.position);
            } else {
                flag_run_towards_player = true;
                attack();
            }
        }
    }
    private void lookAtPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
    private void energyBall()
    {
        GameObject clone = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation, transform);
        clone.SetActive(true);
    }

    private void earthquake()
    {
        playSound(enum_sounds.EarthQuake);
        // Número aleatorio de piedras dentro de cierto rango
        int i = Random.Range(minRockNumber, maxRockNumber);
        while (i > 0)
        {
            float jiroRad = 1f;
            Vector2 circle = Random.insideUnitCircle * rockRad;
            float r1 = circle.x;
            float r2 = circle.y;
            // Para evitar que salga justo debajo de jiro, al resultado se le suma un radio establecido
            if (r1 < 0) r1 -= jiroRad; else r1 += jiroRad;
            if (r2 < 0) r2 -= jiroRad; else r2 += jiroRad;
            Vector3 nPos = new Vector3(rock.transform.position.x + circle.x, rock.transform.position.y, rock.transform.position.z + circle.y);
            // La roca original mira a la nueva posición para darle un ángulo correcto al salir
            rock.transform.LookAt(nPos);
            // Se crea un clon en la posición nueva
            GameObject clone = Instantiate(rock, nPos, rock.transform.rotation, transform);
            clone.SetActive(true);
            i--;
        }
        // El tiempo de lanzamiento de cada roca se maneja en su propio script
        StartCoroutine(cor_actions(4f));
        StartCoroutine(cor_actions(0, enum_cor.CameraShake));
    }

    protected override void attack()
    {
        // mustChangeAction aumenta en 1 al comenzar la animación y disminuye en 1 al finalizarla. Esto es para
        // evitar que cambie de acción en medio de un ataaque. La función para restarle 1 se llama desde un evento
        // dentro de la propia animación (script: AnimationEvents)
        mustChangeAction++;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        armCollider.enabled = true;
        agent.isStopped = true;

        lookAtPlayer();
        changeAnimation(enum_animations.Attack);
        cooldown = true;
        isAttacking = true;
        StartCoroutine(cor_actions(1f, enum_cor.AttackSound));
        StartCoroutine(cor_actions(3f, 0));
    }
    // El cálculo del salto está hecho "a piñón". No se han tenido en cuenta fuerzas físicas como la masa
    // o la gravedad. Abajo del todo está explicada la "fórmula" para hallar y2 y xf2.
    private void jump(Vector3 posJiro, Vector3 posPlayer)
    {
        canExplodeFloor = true;
        float distance = Vector3.Distance(posJiro, posPlayer);

        xFloatForce = distance / constantXForce;

        Vector3 xForce = xFloatForce * transform.forward;
        rb.AddForce(yForce, ForceMode.Impulse);
        rb.AddForce(xForce, ForceMode.Impulse);
    }

    private void jumpToOriginalPosition(Vector3 posJiro, Vector3 posOrig)
    {
        // Se calcula la fuerza necesaria para llegar a la posición original
        float distance = Vector3.Distance(posJiro, posOrig);
        
        xFloatForce = distance / 1.626f;
        Vector3 xForce = xFloatForce * transform.forward;

        // Se añaden las fuerzas de impulso vertical y horizontal
        rb.AddForce(yForceReturning, ForceMode.Impulse);
        rb.AddForce(xForce, ForceMode.Impulse);

        // Indica que lanzará el ataque de energía
        willUseEnergyAttack = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bossFloor"))
        {
            rb.velocity = Vector3.zero;
            // Si está realizando la acción para la energy ball
            if (willUseEnergyAttack)
            {
                willUseEnergyAttack = false;
                changeAnimation(enum_animations.Jump_2);
                StartCoroutine(cor_actions(0, enum_cor.PrepareEnergyball));
                StartCoroutine(cor_actions(4.2f, enum_cor.EndEnergyball));
            }
            // Si está realizando el ataque del salto
            else if (canExplodeFloor)
            {
                animator.SetInteger("id", (int)enum_animations.Jump_2);
                animator.SetTrigger("canBeginAnimation");
                canExplodeFloor = false;
                StartCoroutine(cor_actions(0.2f, enum_cor.EndJump));
            }
        }
        if (collision.gameObject.CompareTag("Player") && canExplodeFloor)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
            rb.AddForce(transform.up * 3f, ForceMode.Impulse);
            Debug.Log("se esta aplicando");
        }
    }
    IEnumerator cor_actions(float time, enum_cor action = enum_cor.Default)
    {
        yield return new WaitForSeconds(time);
        switch (action)
        {
            case enum_cor.Attack:
                cooldown = false;
                break;
            case enum_cor.Earthquake:
                rb.constraints = RigidbodyConstraints.FreezeAll;
                lookAtPlayer();
                changeAnimation(enum_animations.Earthquake);
                yield return new WaitForSeconds(1.2f);
                earthquake();
                break;
            case enum_cor.PrepareEnergyball:
                // Mirará al jugador hasta que lance la bola
                mustLookAtPlayer = true;
                changeAnimation(enum_animations.EnergyBall_1);
                rb.constraints = RigidbodyConstraints.FreezeAll;
                yield return new WaitForSeconds(1.2f);
                energyBall();
                playSound(enum_sounds.PrepareEnergyball);
                break;
            case enum_cor.JumpToOrigin:
                //Calcula la distancio Jiro - Posición original
                Vector3 posMe = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 posOrg = new Vector3(originalPos.x, 0, originalPos.z);
                transform.LookAt(new Vector3(originalPos.x, transform.position.y, originalPos.z));
                changeAnimation(enum_animations.Jump_1);
                yield return new WaitForSeconds(0.8f);
                jumpToOriginalPosition(posMe, posOrg);
                StartCoroutine(cor_actions(1f, enum_cor.JumpSound));
                break;
            case enum_cor.Jump:
                //Calcula la distancio Jiro - Jugador
                Vector3 posJiro = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 posPlayer = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                lookAtPlayer();
                changeAnimation(enum_animations.Jump_1);
                yield return new WaitForSeconds(0.8f);
                jump(posJiro, posPlayer);
                break;
            case enum_cor.EndEnergyball:
                mustLookAtPlayer = false;
                changeAnimation(enum_animations.EnergyBall_2);
                StartCoroutine(cor_actions(3f));
                break;
            case enum_cor.EndJump:
                GameObject clone = Instantiate(explosion, explosion.transform.position, explosion.transform.rotation, transform);
                clone.SetActive(true);
                StartCoroutine(cor_actions(1.2f));
                playSound(enum_sounds.Jump);
                StartCoroutine(cor_actions(0, enum_cor.CameraShake));
                break;
            case enum_cor.CameraShake:
                flag_shake_camera = true;
                yield return new WaitForSeconds(0.6f);
                flag_shake_camera = false;
                break;
            case enum_cor.AttackSound:
                playSound(enum_sounds.Attack);
                break;
            case enum_cor.JumpSound:
                playSound(enum_sounds.Jump);
                break;
            default:
                mustChangeAction--;
                break;
        }
    }

    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            playSound(enum_sounds.Dead);
            StopAllCoroutines();
            //Destroy(gameObject, 3);
            Destroy(agent);
            isDead = true;
            rb.isKinematic = true;
            changeAnimation(enum_animations.Dead);
            
            StartCoroutine(cor_StartFinalCinem(3f));

        }
    }
    public void substractActionCounter()
    {
        mustChangeAction--;
    }
    public bool isJiroDead()
    {
        return isDead;
    }

    IEnumerator cor_EndCinematic(float time)
    {
        yield return new WaitForSeconds(time);
        isDead = false;
        healthBarClone.SetActive(true);
    }

    IEnumerator cor_StartFinalCinem(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(Player.getInstance().gameObject);
        Destroy(HUDController.getInstance().gameObject);
        SceneManager.LoadScene("AfterBossFight");
    }
}

///
/// CÁLCULO Yf - Xf
/// 
/// suponemos que distancia d = 20
/// y1 = 15; x1 = d/xf1 = 20 / 3.04 = 6.58;
/// x1 * y1 / g = P; P = 10.07; (P es const.)
/// 
/// 
/// Elijo una y2 
/// 
/// xf2 = d / (T * g / y2)
/// R: xf2 = 20 / (10.04 * 9.8 / y2)
/// 