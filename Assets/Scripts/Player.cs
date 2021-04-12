using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private uint health;
    private float stamina, mana;

    private Rigidbody rb;

    public Camera playerCamera, initialCamera;
    public GameObject bola;
    public GameObject hand;

    private float camMovementSpeed = 1.6f;
    private const float defaultSpeed = 7.5f;
    private float movSpeed;
    private bool canAttack;
    public bool isAttacking;
  

    private bool cooldownA1, cooldownA2, cooldownDash;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        canAttack = false;
        isAttacking = false;
        health = 100;
        stamina = 100;
        mana = 100;
    }

    void Update()
    {
        movSpeed = defaultSpeed;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * camMovementSpeed;
        //float mouseX = Input.GetAxis("Mouse Y") * camMovementSpeed;

        if (Input.GetKey(KeyCode.Space) && useStamina(0.2f))
        {
            movSpeed += 10f;
        }

        Vector3 move = transform.right * x * movSpeed + transform.forward * y * movSpeed;
        Vector3 rotateValue = new Vector3(0, mouseX * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if (!cooldownDash && Input.GetKey(KeyCode.R) && useStamina(10))
        {
            rb.AddForce(transform.forward + playerCamera.transform.forward * 40000f);
            cooldownDash = true;
            StartCoroutine(finishDash(2f));
        }
        else
        {
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
        }
        
        //transform.Rotate(new Vector3(0, y * camMovementSpeed * Time.deltaTime));
        //mouseY = Mathf.Clamp(mouseY, -120, 120);
        

        if (canAttack && !cooldownA1 && Input.GetMouseButtonDown(1) && useMana(8))
        {
            attack1();
        }

        if (canAttack && !cooldownA2 && Input.GetMouseButtonDown(0) && useStamina(5))
        {
            attack2();
        }
    }

    private void FixedUpdate()
    {
        if(stamina < 100)
        {
            stamina += 0.15f;
        }

        if (mana < 100)
        {
            mana += 0.1f;
        }
    }

    private void attack1()
    {
        GameObject bolaClone = Instantiate(bola);
        bolaClone.SetActive(true);
        bolaClone.transform.position = bola.transform.position;
        cooldownA1 = true;

        StartCoroutine(finishAttack1(0.6f));
    }

    IEnumerator finishAttack1(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownA1 = false;
    }

    IEnumerator finishDash(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownDash = false;
    }

    private void attack2()
    {
        cooldownA2 = true;
        Vector3 originalPos = hand.transform.eulerAngles;
        originalPos.x = -50;
        hand.transform.eulerAngles = originalPos;
        isAttacking = true;

        StartCoroutine(finishAttack2(0.3f));
        StartCoroutine(finishAttack2Cooldown(0.6f));
    }

    IEnumerator finishAttack2(float time)
    {
        yield return new WaitForSeconds(time);

        Vector3 originalPos = hand.transform.eulerAngles;
        originalPos.x = 0;
        hand.transform.eulerAngles = originalPos;
    }

    IEnumerator finishAttack2Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldownA2 = false;
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("enemyHand"))
        {
            if (c.gameObject.GetComponentInParent<Enemy>().isAttacking)
            {
                doDamage(10);
                c.gameObject.GetComponentInParent<Enemy>().isAttacking = false;
            }
        }

        if (c.gameObject.CompareTag("madriguera"))
        {
            canAttack = false;
            initialCamera.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(false);
        }
    }

    public void doDamage(uint dmg)
    {
        health -= dmg;
        //TODO
    }
    
    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("madriguera"))
        {
            canAttack = true;
            initialCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
        }
    }

    public uint getHealth()
    {
        return health;
    }

    public float getStamina()
    {
        return stamina;
    }

    public float getMana()
    {
        return mana;
    }

    private bool useStamina(float needed)
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

    private bool useMana(float needed)
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