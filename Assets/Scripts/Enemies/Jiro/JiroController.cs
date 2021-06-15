using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiroController : Enemy
{
    private enum enum_actions { Jump = 1, Arm = 2, EnergyBall = 3, Earthquake = 4, Null = 0 }
    private enum enum_animations { Idle = 0, Walk = 1, Attack = 2, Jump_1 = 3, Jump_2 = 4, EnergyBall_1 = 5, EnergyBall_2 = 6, Earthquake = 7, Dead = 8 }
    private enum enum_cor { Attack, Earthquake, PrepareEnergyball, JumpToOrigin, Jump, EndEnergyball, EndJump, CameraShake, Default }

    [SerializeField] GameObject projectile, rock, explosion, cam;
    [SerializeField] Animator animator;
    [SerializeField] int minRockNumber = 9, maxRockNumber = 12;

    private Dictionary<enum_actions, int> actions = new Dictionary<enum_actions, int>();
    private BoxCollider armCollider;
    private enum_actions currentAction;
    private Rigidbody rb;
    private Vector3 yForce, originalPos, yForceReturning = 8.0f * Vector3.up;
    private bool mustLookAtPlayer = false, flag_run_towards_player = true, canExplodeFloor = false, willUseEnergyAttack = false, flag_shake_camera = false, isActive = false;
    private float yFloatForce = 15.0f, xFloatForce = 0f, constantXForce = 3.04f, shakingForce = 0.5f, rockRad = 3f;
    private int mustChangeAction = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = 100;
        originalPos = transform.position;

        // El searchRadius debe ser lo suficientemente grande como para abarcar todo el �rea del combate
        searchRadius = 300;
        attackRadius = 6;

        // Collider del brazo. Solo estar� activo cuando est� haciendo el ataque f�sico
        armCollider = GetComponentInChildren<BoxCollider>();
        armCollider.enabled = false;

        changeAnimation(enum_animations.Idle);
        
        // Peso de cada acci�n (cuanto m�s peso, m�s probable que lo haga)
        actions.Add(enum_actions.Arm, 4);
        actions.Add(enum_actions.Jump, 4);
        actions.Add(enum_actions.Earthquake, 3);
        actions.Add(enum_actions.EnergyBall, 2);

        // Esto es para el ataque del salto. La altura est� calculada "a mano"
        yForce = yFloatForce * Vector3.up;
    }
    private new void Update()
    {
        
        // Si no est� muerto y est� activo (de momento se activa al pulsar una tecla)...
        if (!isDead && isActive)
        {
            // Mientra sea true, Jiro mirar� a la posici�n del jugador PERO la 'y' ser� la actual (mirar� de frente)
            if (mustLookAtPlayer)
            {
                lookAtPlayer();
            }
            // Movimiento de la c�mara para simular un impacto fuerte (efecto terremoto)
            if (flag_shake_camera)
            {
                cam.transform.position += new Vector3(0, shakingForce, 0);
                shakingForce = -shakingForce;
            }
            // Si no hay ninguna acci�n llev�ndose a cabo, se ha de asignar una nueva
            if (mustChangeAction == 0)
            {
                mustChangeAction++;
                currentAction = ExtRandom.ChooseWeighted(actions);
                // Se dejan solo las restricciones angulares. Esto se hace porque en alguna acci�n es necesario restringir
                // la posici�n de Jiro
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                // El NavMeshAgent solo debe estar activado para la acci�n "Attack"
                agent.enabled = false;

                // Si sale la acci�n de perseguir al jugador, se define ya el tiempo para acabar dicha acci�n pues esta
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
            else if (!currentAction.Equals(enum_actions.Null)) // Solo ocurre una vez cuando se decide la nueva acci�n
            {
                // Desactivo el collider del brazo para que no colisione con otros elementos (bola m�gica, explosi�n, etc.)
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
        ////////////////////////////////////////////////////////////////////////////
        // DEBUG. Seguramente no use isActive. Habr� que despertarlo de otra forma
        ////////////////////////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.J))
        {
            isActive = true;
        }
    }

    // Para cambiar la animaci�n se elije la id y se activas el trigger
    private void changeAnimation(enum_animations anim)
    {
        animator.SetInteger("id", (int)anim);
        animator.SetTrigger("canBeginAnimation");
    }

    protected override void searchPlayer()
    {
        // Si el cooldown es true, significa que estoy en medio del ataque del brazo. Hasta que no acabe, no pasar� nada.
        if (!cooldown)
        {
            // Distancia Jiro - Jugador
            Vector3 pos1 = transform.position;
            Vector3 pos2 = player.transform.position;
            int distance = (int)Vector3.Distance(pos1, pos2);

            // Si no est� en rango de ataque...
            if (distance > attackRadius)
            {
                // Con el flag hago que para la acci�n de perseguir al jugador se ejecute una vez y lo �nico
                // que ocurre en cada frame es actualizar la posici�n del mismo
                if (flag_run_towards_player)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    // En el momento en el que se pone en modo b�squeda, desactivo el collider del brazo
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
        // N�mero aleatorio de piedras dentro de cierto rango
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
            // La roca original mira a la nueva posici�n para darle un �ngulo correcto al salir
            rock.transform.LookAt(nPos);
            // Se crea un clon en la posici�n nueva
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
        // mustChangeAction aumenta en 1 al comenzar la animaci�n y disminuye en 1 al finalizarla. Esto es para
        // evitar que cambie de acci�n en medio de un ataaque. La funci�n para restarle 1 se llama desde un evento
        // dentro de la propia animaci�n (script: AnimationEvents)
        mustChangeAction++;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        armCollider.enabled = true;
        agent.isStopped = true;

        lookAtPlayer();
        changeAnimation(enum_animations.Attack);
        cooldown = true;
        isAttacking = true;

        StartCoroutine(cor_actions(3f, 0));
    }
    // El c�lculo del salto est� hecho "a pi��n". No se han tenido en cuenta fuerzas f�sicas como la masa
    // o la gravedad. Abajo del todo est� explicada la "f�rmula" para hallar y2 y xf2.
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
        // Se calcula la fuerza necesaria para llegar a la posici�n original
        float distance = Vector3.Distance(posJiro, posOrig);
        
        xFloatForce = distance / 1.626f;
        Vector3 xForce = xFloatForce * transform.forward;

        // Se a�aden las fuerzas de impulso vertical y horizontal
        rb.AddForce(yForceReturning, ForceMode.Impulse);
        rb.AddForce(xForce, ForceMode.Impulse);

        // Indica que lanzar� el ataque de energ�a
        willUseEnergyAttack = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bossFloor")
        {
            rb.velocity = Vector3.zero;
            // Si est� realizando la acci�n para la energy ball
            if (willUseEnergyAttack)
            {
                willUseEnergyAttack = false;
                changeAnimation(enum_animations.Jump_2);
                StartCoroutine(cor_actions(0, enum_cor.PrepareEnergyball));
                StartCoroutine(cor_actions(4.2f, enum_cor.EndEnergyball));
            }
            // Si est� realizando el ataque del salto
            else if (canExplodeFloor)
            {
                animator.SetInteger("id", (int)enum_animations.Jump_2);
                animator.SetTrigger("canBeginAnimation");
                canExplodeFloor = false;
                StartCoroutine(cor_actions(0.2f, enum_cor.EndJump));
            }
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
                // Mirar� al jugador hasta que lance la bola
                mustLookAtPlayer = true;
                changeAnimation(enum_animations.EnergyBall_1);
                rb.constraints = RigidbodyConstraints.FreezeAll;
                yield return new WaitForSeconds(1.2f);
                energyBall();
                break;
            case enum_cor.JumpToOrigin:
                //Calcula la distancio Jiro - Posici�n original
                Vector3 posMe = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 posOrg = new Vector3(originalPos.x, 0, originalPos.z);
                transform.LookAt(new Vector3(originalPos.x, transform.position.y, originalPos.z));
                changeAnimation(enum_animations.Jump_1);
                yield return new WaitForSeconds(0.8f);
                jumpToOriginalPosition(posMe, posOrg);
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
                StartCoroutine(cor_actions(0, enum_cor.CameraShake));
                break;
            case enum_cor.CameraShake:
                flag_shake_camera = true;
                yield return new WaitForSeconds(0.6f);
                flag_shake_camera = false;
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
            StopAllCoroutines();
            Destroy(gameObject, 3);
            Destroy(agent);
            isDead = true;
            rb.isKinematic = true;
            changeAnimation(enum_animations.Dead);
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
}

///
/// C�LCULO Yf - Xf
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