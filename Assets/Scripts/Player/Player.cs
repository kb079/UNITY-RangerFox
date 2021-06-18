using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int health;
    private float stamina, mana;

    protected Rigidbody rb;
    private Animator anim;

    public Camera playerCamera;
    public GameObject bola;
    public GameObject hand;
    public GameObject barrier;
    public GameObject bossBarrier;
    public GameObject crossHair;

    private const float defaultSpeed = 7.5f;
    private float movSpeed;
    public bool isAttacking;
    public bool isBarrierActive = false;
    private string hudText;
    private float poisonTime = 1.2f;
    protected GameObject inventory;
    protected bool isInventoryEnabled = true;
    public GameObject nuevaPosicion;
    public bool isPaused;

    private enum enum_sounds { Barrier = 0, Dash = 1, Attack = 2, Magic = 3 }
    private bool cooldownA1, cooldownA2, cooldownDash, runningAnim, canUseBarrier, isDead;

    public AudioSource audiosource;
    public AudioClip[] sonidos;
    public AudioSource loopAudiosource;
    private void Awake()
    {

        health = 100;
        stamina = 100;
        mana = 100;
    }
    private void Start()
    {
        //El cursor no se sale de la pantalla
        Cursor.lockState = CursorLockMode.Confined;

        inventory = GameObject.FindGameObjectWithTag("Inventory");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isDead = false;
        isAttacking = false;
        canUseBarrier = true;
        isPaused = false;
        hudText = "";
    }

    public void StartWalkingSound(float time)
    {
        if (!loopAudiosource.isPlaying)
        {
            loopAudiosource.PlayDelayed(time);
        }
    }

    public void StopWalkingSound()
    {
        if (loopAudiosource.isPlaying)
        {
            loopAudiosource.Stop();
        }
    }

    void Update()
    {
        if (!isDead && !isPaused)
        {
            playSound();
            playerMoves();
            activateActions();

            if (!cooldownA1 && Input.GetMouseButtonDown(1) && useMana(8))
            {
                cooldownA1 = true;
                float time = 0.84f;
                if (!crossHair.activeInHierarchy)
                {
                    anim.SetTrigger("magic");
                }
                else
                {
                    anim.SetBool("magicCH", true);
                    time = 0.55f;
                }

                StartCoroutine(attack1(time));
            }
            if (!cooldownA2 && Input.GetMouseButtonDown(0) && useStamina(5)) attack2();
        }

        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
            playerCamera.GetComponent<CameraManager>().isPaused = true;
        }
    }

    public void resumeGame()
    {
        playerCamera.GetComponent<CameraManager>().isPaused = false;
        isPaused = false;
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1;
    }
    protected void playerMoves()
    {
        movSpeed = defaultSpeed;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * GameConstants.camMovementSpeed;

        Vector3 move = transform.right * x * movSpeed + transform.forward * y * movSpeed;
        //ROTATE PLAYER WITH CAMERA
        Vector3 rotateValue = new Vector3(0, mouseX * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if ((x != 0 || y != 0) && (!isAttacking && !runningAnim))
        {

            if (Input.GetKey(GameConstants.key_run) && useStamina(0.05F))
            {
                StartWalkingSound(0.08f);
                movSpeed += 25f;
                toggleRunAnim(true);
            }
            else
            {
                StartWalkingSound(0.23f);
                toggleRunAnim(false);
                //toggleWalkAnim(true);
            }

            toggleWalkAnim(true);
            anim.SetFloat("playerX", x);
            anim.SetFloat("playerZ", y);

            if (!cooldownDash && Input.GetKey(GameConstants.key_dash) && useStamina(10))
            {
                rb.AddForce(transform.forward + playerCamera.transform.forward * 30000f);
                cooldownDash = true;
                StartCoroutine(finishDash(2f));
            }

            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
        else
        {
            StopWalkingSound();
            toggleWalkAnim(false);
            anim.SetFloat("mouseX", mouseX * 5.6f);
        }
    }

    private void toggleWalkAnim(bool state)
    {
        if (anim.GetBool("walk") != state) anim.SetBool("walk", state);
    }

    private void toggleRunAnim(bool state)
    {
        anim.SetBool("run", state);
    }

    private void runAnimation(string name, float time)
    {
        runningAnim = true;
        anim.SetTrigger(name);
        StartCoroutine(finishAnimation(time));
    }

    IEnumerator finishAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        runningAnim = false;
    }

    protected void activateActions()
    {
        //activar/desactivar inventario
        if (Input.GetKeyDown(GameConstants.key_inventory))
        {
            isInventoryEnabled = !isInventoryEnabled;
            inventory.SetActive(isInventoryEnabled);
        }
        if (Input.GetKeyDown(GameConstants.key_barrier) && !isBarrierActive && canUseBarrier && useMana(0.05f))
        {
            isBarrierActive = true;
            StartCoroutine(delayActiveBarrier());
            runAnimation("barrier", 2.4f);
            canUseBarrier = false;
            StartCoroutine(delayBarrierKey());
        }
        if (Input.GetKeyUp(GameConstants.key_barrier) && isBarrierActive && canUseBarrier)
        {
            barrier.SetActive(false);
            isBarrierActive = false;
            canUseBarrier = false;
            StartCoroutine(delayBarrierKey());
        }
    }
    IEnumerator delayActiveBarrier()
    {
        yield return new WaitForSeconds(1.5f);
        barrier.SetActive(true);
    }

    IEnumerator delayBarrierKey()
    {
        yield return new WaitForSeconds(1.2f);
        canUseBarrier = true;
    }

    protected void FixedUpdate()
    {
        if (stamina < 100) stamina += 0.15f;
    }

    private void LateUpdate()
    {
        if (barrier.activeInHierarchy)
        {
            if (!useMana(0.025f))
            {
                barrier.SetActive(false);
                isBarrierActive = false;
            }
        }
    }

    //ataque mágico
    IEnumerator attack1(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject bolaClone = Instantiate(bola);
        bolaClone.SetActive(true);
        bolaClone.transform.position = bola.transform.position;
        isAttacking = true;

        if (crossHair.activeInHierarchy) anim.SetBool("magicCH", false);

        StartCoroutine(finishAttack1(0.15f));
    }

    IEnumerator finishAttack1(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownA1 = false;
        isAttacking = false;
    }

    protected IEnumerator finishDash(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownDash = false;
    }

    IEnumerator poisoned(float ticks, int dmg)
    {
        yield return new WaitForSeconds(poisonTime);
        doDamage(dmg, ticks);
    }

    public void doDamage(int dmg, float ticks)
    {
        doSingleDamage(dmg);
        ticks--;
        if (ticks > 0)
        {
            StartCoroutine(poisoned(ticks, dmg));
        }
    }

    //ataque físico
    private void attack2()
    {
        cooldownA2 = true;
        anim.SetTrigger("hit");
        isAttacking = true;

        StartCoroutine(finishAttack2Cooldown(1.6f));
    }

    IEnumerator finishAttack2Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownA2 = false;
        isAttacking = false;
    }

    private void OnTriggerStay(Collider c)
    {
        //TODO: no esta bien ponerlo aqui
        if (c.gameObject.CompareTag("bossFloor"))
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            SceneManager.LoadSceneAsync("Madriguera");
        }
        if (c.gameObject.CompareTag("SceneBoss"))
        {
            //bossBarrier.SetActive(true);

            SceneManager.LoadSceneAsync("FinalBoss");
        }
        Debug.Log("esta colisionando");
    }

    private void OnCollisionEnter(Collision c)
    {
        //TODO - KB
        //CHECKEO SI EL JUGADOR SE COLISIONA CON ALGUN OBJETO
        //SE EJECUTA ANIMACION DE "SE HA COLISIONADO CON ALGO" Y LO ECHA PARA ATRAS

        if (!runningAnim && c.gameObject.CompareTag("tree"))
        {
            runAnimation("blockWalk", 0.6f);
            rb.AddForce((-transform.forward * 2000f) * Time.deltaTime, ForceMode.Impulse);
        }

    }

    public void doSingleDamage(int dmg)
    {
        health -= dmg;
        //SE EJECUTA SONIDO DAÑO
        if (!isDead)
            audiosource.PlayOneShot(sonidos[4]);
        if (health <= 0) playerDead();


    }

    private void playerDead()
    {
        if (!isDead) { 
            audiosource.PlayOneShot(sonidos[5]);
            isDead = true;
            //SE EJECUTA SONIDO MUERTE
            anim.SetTrigger("death");
            StartCoroutine(playerDead(4f));
        }
    }

    IEnumerator playerDead(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("DeathMenu", LoadSceneMode.Additive);
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("bossFloor"))
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            Vector3 dir = transform.position - c.transform.position;
            rb.AddForce(-dir * 300f, ForceMode.Force);
        }
    }

    public int getHealth()
    {
        return health;
    }

    public void addHealth(int addedHealth)
    {
        if (addedHealth + health < 100) health += addedHealth;
        else health = 100;
    }

    public void addMana(int addedMana)
    {
        if (addedMana + mana < 100) mana += addedMana;
        else mana = 100;
    }

    public float getStamina()
    {
        return stamina;
    }

    public float getMana()
    {
        Debug.Log("este es el mana " + mana);
        return mana;
    }

    public string getHudText()
    {
        return hudText;
    }

    public void setHudText(string text)
    {
        hudText = text;
    }

    protected bool useStamina(float needed)
    {
        if (stamina >= needed)
        {
            float newStamina = stamina - needed;
            if (newStamina <= 0)
            {
                stamina = 0;
            }
            else
            {
                stamina = newStamina;
            }
            return true;
        }
        return false;
    }

    public bool useMana(float needed)
    {
        if (mana >= needed)
        {
            float newStamina = mana - needed;
            if (newStamina <= 0)
            {
                mana = 0;
            }
            else
            {
                mana = newStamina;
            }
            return true;
        }
        return false;
    }
    private void playSound()
    {
        //barrera
        if (Input.GetKey(GameConstants.key_barrier) && canUseBarrier && !audiosource.isPlaying && mana > 0.05f)
        {
            audiosource.PlayOneShot(sonidos[(int)enum_sounds.Barrier]);
        }
        //dash
        if (Input.GetKeyDown(GameConstants.key_dash) && stamina > 10f && !cooldownDash)
        {
            audiosource.PlayOneShot(sonidos[(int)enum_sounds.Dash]);
        }
        //atacar
        if (Input.GetKeyDown(GameConstants.key_attack) && !cooldownA2)
        {
            audiosource.PlayOneShot(sonidos[(int)enum_sounds.Attack]);
            Debug.Log("Ataca");
        }
        //bola fuego
        if (Input.GetKeyDown(GameConstants.key_magic) && mana > 8f && !cooldownA1)
        {
            audiosource.PlayOneShot(sonidos[(int)enum_sounds.Magic]);
        }
    }
}
