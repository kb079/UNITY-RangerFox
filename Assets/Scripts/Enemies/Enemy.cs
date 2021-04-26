using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected int health;

    protected bool isAttacking;
    protected bool isDead;
    protected bool cooldown;

    protected GameObject player;
    protected NavMeshAgent agent;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (GetComponent<NavMeshAgent>() != null) {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    protected virtual void doPlayerDamage(int dmg) {
        player.GetComponent<Player>().doDamage(dmg);
       
    }

    public virtual void doDamage(int dmg) {
        Debug.Log("doDamage");
        health -= dmg;
        Debug.Log("esta es la vida que tiene ahora" + health);
        checkHP();
    }

    protected abstract void attack();

    protected virtual void searchPlayer() {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        uint distance = (uint)Vector3.Distance(pos1, pos2);
        if (distance <= 30 && distance > 3)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform);
        }
        else if (distance < 3)
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
        Debug.Log("estoy mirando la vida");
        if (health <= 0 && !isDead)
        {
            Debug.Log("muerto");
            if (agent != null)
            {
                Destroy(agent);
            }
            Debug.Log("doDamage");
            isDead = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
        }
    }


    protected void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("playerHand"))
        {
            if (c.gameObject.GetComponentInParent<Player>().isAttacking)
            {
                doDamage(5);
                c.gameObject.GetComponentInParent<Player>().isAttacking = false;
            }
        }
    }
}