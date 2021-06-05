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
        }
        else if (distance <= attackRadius && !cooldown)
        {
            attack();
            agent.isStopped = true;
        }
        else if (!agent.isStopped)
        {
            agent.isStopped = true;
            animator.SetInteger("id", 0);
        }
    }

    protected override void attack()
    {
        animator.SetInteger("id", 2);
        cooldown = true;
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
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
    }
}
