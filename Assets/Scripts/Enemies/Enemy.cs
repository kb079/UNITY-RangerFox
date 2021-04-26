using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected uint health;

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

    protected virtual void doPlayerDamage(uint dmg) {
        player.GetComponent<Player>().doDamage(dmg);
    }

    public virtual void doDamage(uint dmg) {
        health -= dmg;
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
        if (health == 0 && !isDead)
        {
            if(agent != null)
            {
                Destroy(agent);
            }
            isDead = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
        }
    }


    private void OnTriggerStay(Collider c)
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