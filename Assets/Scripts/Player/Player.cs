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
   
    private const float defaultSpeed = 7.5f;
    private float movSpeed;
    public bool isAttacking;
    public bool isBarrierActive = false;
    private string hudText;
    private float poisonTime = 1.2f;
    protected GameObject inventory;
    protected bool isInventoryEnabled = true;

    private bool cooldownA1, cooldownA2, cooldownDash, runningAnim, canUseBarrier;

    private void Start()
    {
        //El cursor no se sale de la pantalla
        Cursor.lockState = CursorLockMode.Confined;

        inventory = GameObject.FindGameObjectWithTag("Inventory");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isAttacking = false;
        canUseBarrier = true;
        health = 100;
        stamina = 100;
        mana = 100;
        hudText = "";
    }

    void Update()
    {

        playerMoves();
        activateActions();

        if (!cooldownA1 && Input.GetMouseButtonDown(1) && useMana(8)) StartCoroutine(attack1(1.2f));
        if (!cooldownA2 && Input.GetMouseButtonDown(0) && useStamina(5)) attack2();
        
    }

    protected void playerMoves()
    {
        movSpeed = defaultSpeed;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * GameConstants.camMovementSpeed;

        Vector3 move = transform.right * x * movSpeed + transform.forward * y * movSpeed;
        Vector3 rotateValue = new Vector3(0, mouseX * -1, 0);

        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if ((x != 0 || y != 0) && (!isAttacking && !runningAnim))
        {
            if (Input.GetKey(GameConstants.key_run) && useStamina(0.2f))
            {
                movSpeed += 25f;
                toggleRunAnim(true);
            }
            else
            {
                toggleWalkAnim(true);
            }

            if (!cooldownDash && Input.GetKey(GameConstants.key_dash) && useStamina(10))
            {
                rb.AddForce(transform.forward + playerCamera.transform.forward * 30000f);
                cooldownDash = true;
                StartCoroutine(finishDash(2f));
            }
            else
            {
                rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
            }
        }
        else
        {
            toggleWalkAnim(false);
        }
    }

    private void toggleWalkAnim(bool state)
    {
        if (anim.GetBool("run"))
        {
            toggleRunAnim(false);
        }
        anim.SetBool("walk", state);
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
        yield return new WaitForSeconds(1f);
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
        cooldownA1 = true;
        isAttacking = true;
        anim.SetTrigger("magic");

        StartCoroutine(finishAttack1(2.6f));
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

        StartCoroutine(finishAttack2Cooldown(2.64f));
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
            SceneManager.LoadSceneAsync(2);
        }
        if (c.gameObject.CompareTag("bossFloor"))
        {
            bossBarrier.SetActive(true);
        }
    }

    public void doSingleDamage(int dmg)
    {
        health -= dmg;
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
}