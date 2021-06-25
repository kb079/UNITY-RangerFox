using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;
    private bool canRun = true;
    private int count_cin = 0;

    private int health;
    private float stamina, mana;

    public int maxHealth = 100, maxMana = 100, maxStamina = 100;

    protected Rigidbody rb;
    public Animator anim;

    public Camera playerCamera;
    public GameObject bola;
    public GameObject barrier;
    public GameObject bossBarrier;
    public GameObject crossHair;

    private const float defaultSpeed = 10f;
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
    private bool cooldownA1, cooldownA2, cooldownDash, canUseBarrier;
    public bool isDead, runningAnim;

    public AudioSource audiosource;
    public AudioClip[] sonidos;
    public AudioSource loopAudiosource;

    public static Player getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(transform.gameObject);

        health = 100;
        stamina = 100;
        mana = 100;

        crossHair = GameObject.FindGameObjectWithTag("crosshair");
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
        runningAnim = false;
        hudText = "";

        if (SceneManager.GetActiveScene().name.Equals("FinalBoss"))
        {
            isDead = true;
            StartCoroutine(cor_EndCinematic(20f));
        }

        if (SceneManager.GetActiveScene().name.Equals("AfterBossFight"))
        {
            isDead = true;
            StartCoroutine(cor_EndCinematic(16.5f));
        }
  
        if (SceneManager.GetActiveScene().name.Equals("Madriguera") && PlayerSavingData.runLoadData)
        {
            PlayerSavingData.loadData();
        }
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
        if (Input.GetKeyDown(GameConstants.key_interact) && count_cin == 0 && SceneManager.GetActiveScene().name.Equals("FinalBoss"))
        {
            StopCoroutine(cor_EndCinematic(20f));
            playerCamera.gameObject.SetActive(true);
            isDead = false;
            count_cin++;

        }

        if (Input.GetKeyDown(GameConstants.key_interact) && count_cin == 1 && SceneManager.GetActiveScene().name.Equals("AfterBossFight"))
        {
            StopCoroutine(cor_EndCinematic(16.5f));
            playerCamera.gameObject.SetActive(true);
            isDead = false;
            count_cin++;
        }

        if (!isDead && !isPaused)
        {
            playerMoves();
            activateActions();

            if (!cooldownA1 && Input.GetKeyDown(GameConstants.key_magic) && useMana(8))
            {
                audiosource.PlayOneShot(sonidos[(int)enum_sounds.Magic]);
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

            if (Input.GetKeyDown(GameConstants.key_attack) && !anim.GetBool("crosshair") && !cooldownA2 && useStamina(5)) attack2();
        }

        #if UNITY_WEBGL
            if (Input.GetKeyDown(KeyCode.P) && !isPaused) pauseGame();
        #endif

        #if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) pauseGame();
        #endif
    }

    public void pauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        playerCamera.GetComponent<CameraManager>().isPaused = true;
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

        float a = (x * movSpeed);
        float b = (y * movSpeed);

        Vector3 move = new Vector3(a, 0, b);
        //ROTATE PLAYER WITH CAMERA
        Vector3 rotateValue = new Vector3(0, mouseX * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if (!isAttacking && !runningAnim)
        {
            if (anim.GetBool("run")) { 
                if (x > 0.1 && x < 0.5)
                {
                    x = 0.5f;
                }
                else if (x < -0.5)
                {
                    x = -1;
                }

                if (y > 0.5 && y < 0.5f)
                {
                    y = 0.5f;
                }
                else if (y < -0.5)
                {
                    y = -1;
                }
            }

            anim.SetFloat("playerX", x);
            anim.SetFloat("playerZ", y);

            if (Input.GetKey(GameConstants.key_run) && canRun && useStamina(0.07f))
            {
                toggleRunAnim(true);
                StartWalkingSound(0.08f);
                movSpeed += 25f;
            }
            else {
                toggleRunAnim(false);
            }

            if (!cooldownDash && Input.GetKey(GameConstants.key_dash) && useStamina(10))
            {
                audiosource.PlayOneShot(sonidos[(int)enum_sounds.Dash]);
                rb.AddForce(transform.forward + playerCamera.transform.forward * 30000f);
                cooldownDash = true;
                StartCoroutine(finishDash(2f));
            }

            if (!anim.GetBool("run") && (x != 0 || y != 0)) StartWalkingSound(0.30f);
            else if(!anim.GetBool("run") &&  x == 0 || y == 0) StopWalkingSound();

                
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
    }

    private void toggleRunAnim(bool state)
    {
        if (anim.GetBool("run") != state) anim.SetBool("run", state);
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
            audiosource.PlayOneShot(sonidos[(int)enum_sounds.Barrier]);
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
        isBarrierActive = true;
        barrier.SetActive(true);
    }

    IEnumerator delayBarrierKey()
    {
        yield return new WaitForSeconds(1.2f);
        canUseBarrier = true;
    }

    IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1f);
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
        audiosource.PlayOneShot(sonidos[(int)enum_sounds.Attack]);
        cooldownA2 = true;
        anim.SetTrigger("hit");
        isAttacking = true;

        StartCoroutine(finishAttack2Cooldown(1.52f));
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
        //COLISION CON LA MANO DEL LOBO -- ATAQUE

        if (c.gameObject.CompareTag("wolfHand") && c.GetComponentInParent<Wolf>().isAttacking)
        {
            //doSingleDamage(GameConstants.Wolf_Dmg); ESTA PUESTO EN LA CLASE WOLF -- attack()
            c.GetComponentInParent<Wolf>().doAttack();
        }
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
        if (!isDead)
        {
            audiosource.PlayOneShot(sonidos[5]);
            isDead = true;
            //SE EJECUTA SONIDO MUERTE
            anim.SetTrigger("death");
            if (crossHair.activeInHierarchy) { 
                crossHair.SetActive(false);
                anim.SetBool("crosshair", false);
            }
            StartCoroutine(playerDead(4f));
        }
    }

    IEnumerator playerDead(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("DeathMenu", LoadSceneMode.Additive);
    }

    public int getHealth()
    {
        return health;
    }

    public void addHealth(int addedHealth)
    {
        if (addedHealth + health < maxHealth) health += addedHealth;
        else health = maxHealth;
    }

    public void addStamina(int addedStamina)
    {
        if (addedStamina + stamina < maxStamina) stamina += addedStamina;
        else stamina = maxStamina;
    }

    public void setHealth(int v)
    {
        health = (v > 100) ? health = 100 : health = v;        
    }

    public void setMana(float v)
    {
        mana = (v > 100) ? mana = 100 : mana = v;
    }

    public void setStamina(float v)
    {
        stamina = (v > 100) ? stamina = 100 : stamina = v;
    }

    public void addMana(int addedMana)
    {
        if (addedMana + mana < maxMana) mana += addedMana;
        else mana = maxMana;
    }

    public float getStamina()
    {
        return stamina;
    }

    public float getMana()
    {
        //Debug.Log("este es el mana " + mana);
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
        canRun = false;
        StartCoroutine(cor_canRun());
        return false;
    }

    IEnumerator cor_canRun()
    {
        yield return new WaitForSeconds(2f);
        canRun = true;
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

    public IEnumerator cor_EndCinematic(float time)
    {
        yield return new WaitForSeconds(time);
        isDead = false;
        playerCamera.gameObject.SetActive(true);
    }
}
