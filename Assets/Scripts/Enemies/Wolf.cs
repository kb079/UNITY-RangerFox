using System.Collections;
using UnityEngine;

public class Wolf : Enemy
{
    public GameObject enemyHand;
    private float attackingTime = 0.4f;
    private float attackCooldown = 1f;

    private void Start()
    {
        health = GameConstants.Wolf_HP;
    }

    void Update()
    {
        if (!isDead)
        {
            searchPlayer();
        }
    }

    protected override void attack()
    {
        cooldown = true;
        Vector3 originalPos = enemyHand.transform.eulerAngles;
        originalPos.x = -50;
        enemyHand.transform.eulerAngles = originalPos;
        isAttacking = true;

        StartCoroutine(finishAttack(attackingTime));
        StartCoroutine(finishAttackCooldown(attackCooldown));
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

    new private void OnTriggerStay(Collider c)
    {
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
