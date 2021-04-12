using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private int health = 15;

    private NavMeshAgent agent;
    private GameObject player;
    private bool isDeath;
    public GameObject enemyHand;

    private bool cooldown = false;
    public bool isAttacking;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        isAttacking = false;
    }

    void Update()
    {
        if (!isDeath) {
            searchPlayer();
        }
    }

    private void attack()
    {
        cooldown = true;
        Vector3 originalPos = enemyHand.transform.eulerAngles;
        originalPos.x = -50;
        enemyHand.transform.eulerAngles = originalPos;
        isAttacking = true;

        StartCoroutine(finishAttack(0.4f));
        StartCoroutine(finishAttackCooldown(1f));
    }

    IEnumerator finishAttack(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 originalPos = enemyHand.transform.eulerAngles;
        originalPos.x = 0;
        enemyHand.transform.eulerAngles = originalPos; 
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
    }

    private void searchPlayer()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        int distance = (int) Vector3.Distance(pos1, pos2);
        if (distance <= 30 && distance > 3)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else if(distance < 3)
        {
            agent.isStopped = true;
            if (!cooldown)
            {
                attack();
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("playerHand"))
        {
            if (c.gameObject.GetComponentInParent<Player>().isAttacking)
            {
                doDamage();
                c.gameObject.GetComponentInParent<Player>().isAttacking = false;
            }
        }
    }

    private void checkHP()
    {
        if(health <= 0 && !isDeath)
        {
            Destroy(agent);
            isDeath = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
        }
    }

    public void doDamage()
    {
        health -= 5;
        checkHP();
    }
}
