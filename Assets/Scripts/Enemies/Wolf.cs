using System.Collections;
using UnityEngine;

public class Wolf : Enemy
{
    public GameObject enemyHand;
    private float attackingTime = 1.2f;
    private float attackCooldown = 1f;
    [SerializeField] Animator animator;

    private void Start()
    {
        health = GameConstants.Wolf_HP;
        animator.SetInteger("id", 0);
    }

    protected override void searchPlayer()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;


        int distance = (int)Vector3.Distance(pos1, pos2);
        if (distance <= searchRadius && distance > attackRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            animator.SetInteger("id", 1);
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

    protected override void attack()
    {
        animator.SetInteger("id", 2);
        cooldown = true;
        Vector3 originalPos = enemyHand.transform.eulerAngles;
        originalPos.x = -50;
        enemyHand.transform.eulerAngles = originalPos;
        isAttacking = true;

        StartCoroutine(finishAttack(attackingTime));
        StartCoroutine(finishAttackCooldown(attackCooldown));
    }

    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            Destroy(gameObject, 2.8f);
            animator.SetInteger("id", 3);
            if (agent != null)
            {
                Destroy(agent);
            }
            isDead = true;
        }
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

    protected override void OnTriggerStay(Collider c)
    {
        base.OnTriggerStay(c);

        if (c.gameObject.Equals(player))
        {
            Vector3 ray_start = enemyHand.transform.position + new Vector3(0f, .5f, 0f);
            bool collision_front = Physics.Raycast(ray_start, transform.forward, 3f);

            if (collision_front && isAttacking)
            {
                if (!player.GetComponent<Player>().barrier.activeInHierarchy)
                {
                    base.doPlayerDamage(10);
                }
                else
                {
                    player.GetComponent<Player>().useMana(10);
                }

                isAttacking = false;
            }
        }
    }
}
