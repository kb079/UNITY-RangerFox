 using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    protected float health;
    protected Image healthBar;
    public GameObject healthBarGO;
    protected GameObject healthBarClone;
    protected int searchRadius = 30;
    protected int attackRadius = 3;

    protected bool isAttacking;
    protected bool isDead;
    protected bool cooldown;

    protected GameObject player;
    protected NavMeshAgent agent;
    protected float maxHealth = 9999;

    protected int dropProbabilityMax = 4;
    protected int dropProbabilitySuccess = 4;
    protected InventoryObject inventory;
    protected float healthBarYPosition = 5;
    protected Vector3 healthBarScale = new Vector3(1, 1, 1);

    private void Awake()
    {
        healthBarGO = GameObject.FindGameObjectWithTag("EnemyHealthBar");
        inventory = GameObject.FindGameObjectWithTag("UIManager").GetComponent<InventoryObject>();
        DontDestroyOnLoad(healthBarGO.transform.gameObject);
        healthBarClone = Instantiate(healthBarGO, transform.position, transform.rotation);
        //Segundo componente image (barra roja)
        healthBar = healthBarClone.GetComponentsInChildren<Image>()[1];
        healthBarClone.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        healthBar.fillAmount = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        if (GetComponent<NavMeshAgent>() != null) {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isDead)
        {
            healthBarClone.transform.position = new Vector3(transform.position.x, transform.position.y + healthBarYPosition, transform.position.z);
            healthBarClone.transform.rotation = transform.rotation;
        }
    }

    protected virtual void Update()
    {
        if (!isDead) searchPlayer();
    }

    protected virtual void doPlayerDamage(int dmg) {
        player.GetComponent<Player>().doDamage(dmg, 1);
    }

    public virtual void doDamage(float dmg) {
        if (isDead) return;

        if (!isDead)
        {
            healthBar.gameObject.SetActive(true);
        }

        //
        if (maxHealth == 9999) {
            maxHealth = health;
        }
        //

        health -= dmg;
        healthBarClone.gameObject.SetActive(true);
        healthBar.fillAmount = health / maxHealth;
        Debug.Log("esta es la vida que tiene ahora" + health);
        checkHP();

        if (isDead) healthBar.gameObject.SetActive(false);
    }

    protected abstract void attack();

    protected void OnDestroy()
    {
        inventory.OnEnemyDead(dropProbabilitySuccess, dropProbabilityMax, transform.position);
    }

    protected virtual void searchPlayer() {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        int distance = (int)Vector3.Distance(pos1, pos2);
        if (distance <= searchRadius && distance > attackRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            //transform.LookAt(player.transform);
        }
        else if (distance < attackRadius)
        {
            agent.isStopped = true;
            if (!cooldown)
            {
                attack();
            }
        }
    }

    protected virtual void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            if (agent != null) Destroy(agent);

            GetComponent<Rigidbody>().isKinematic = true;

            /* OLD, YA NO SE USA XQ ESTAN LAS ANIMACIONES
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
            */
        }
    }

    protected virtual void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("yuki_hand"))
        {
            if (c.gameObject.GetComponentInParent<Player>().isAttacking)
            {
                doDamage(5);
                c.gameObject.GetComponentInParent<Player>().isAttacking = false;
            }
        }
    }
}