using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Jiro : Enemy
{
    public GameObject hand;
    public GameObject projectile;
    public GameObject rock;

    private float attackingTime = 0.4f;
    private float attackCooldown = 1f;

    private void Start()
    {
        attackRadius = 16;
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
        attackProjectile();
        cooldown = true;
        /*
        Vector3 originalPos = hand.transform.eulerAngles;
        originalPos.x = -50;
        hand.transform.eulerAngles = originalPos;
        isAttacking = true;
        */
        StartCoroutine(finishAttack(attackingTime));
        StartCoroutine(finishAttackCooldown(attackCooldown));
    }

    private void attackProjectile()
    {
        transform.LookAt(player.transform.position);

        GameObject attackObjClone = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);
        attackObjClone.SetActive(true);
        //cooldown = true;
    }

    IEnumerator finishAttack(float time)
    {
        yield return new WaitForSeconds(time);

        Vector3 originalPos = hand.transform.eulerAngles;
        originalPos.x = 0;
        hand.transform.eulerAngles = originalPos;
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.Equals(player))
        {
            Vector3 ray_start = hand.transform.position + new Vector3(0f, .5f, 0f);
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
