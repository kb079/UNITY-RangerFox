using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jiro : Enemy
{
    public GameObject body;
    public GameObject hand;
    public GameObject projectile;
    public GameObject rock;

    public List<GameObject> rockPositions;
    private bool searchingRock;
    public bool hasRock;
    private System.Random rand = new System.Random();

    private float attackingTime = 0.4f;
    private float attackCooldown = 4f;
    private int lastRock = 0;

    private void Start()
    {
        searchingRock = false;
        hasRock = false;
        attackRadius = 25;
        searchRadius = 70;
        health = GameConstants.Wolf_HP;
    }
    IEnumerator startHandAttack(float time)
    {
        yield return new WaitForSeconds(time);
        Vector3 originalPos = hand.transform.eulerAngles;
        originalPos.x = -50;
        hand.transform.eulerAngles = originalPos;
        isAttacking = true;
        StartCoroutine(finishAttack(attackingTime));
    }

    protected override void attack()
    {
        attackRadius = rand.Next(1, 60);
        if (attackRadius <= 10)
        {
            attackRadius = 10;
            agent.SetDestination(player.transform.position);

            StartCoroutine(startHandAttack(3f));
        }else if(attackRadius > 20 && attackRadius < 30)
        {
            attackProjectile();
        }
        else if(lastRock < 5)
        {
            attackRock();
        }
        else
        {
            attackProjectile();
        }

        cooldown = true;
        StartCoroutine(finishAttackCooldown(attackCooldown));
    }

    private void attackProjectile()
    {
        transform.LookAt(player.transform.position);
        chargeItem(projectile);
        body.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        StartCoroutine(prepareProjectileAttack(1.2f));
    }

    private void attackRock()
    {
        agent.isStopped = false;
        agent.SetDestination(rockPositions[lastRock].transform.position);
        searchingRock = true;
    }

    private void chargeItem(GameObject item)
    {
        GameObject attackObjClone = Instantiate(item, item.transform.position, item.transform.rotation, transform);
        attackObjClone.SetActive(true);
    }

    IEnumerator prepareProjectileAttack(float time)
    {
        yield return new WaitForSeconds(time);
        body.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
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
     protected override void searchPlayer()
    {
        if (!searchingRock && !hasRock)
        {
            base.searchPlayer();
        }
        else
        {
            if (hasRock)
            {
               agent.SetDestination(player.transform.position);
            }
            else
            {
                Vector3 playerPos = transform.position;
                Vector3 rockPos = rockPositions[lastRock].transform.position;
                if ((int)Vector3.Distance(playerPos, rockPos) < 5)
                {
                    chargeItem(rock);
                    Destroy(rockPositions[lastRock]);
                    lastRock++;
                    searchingRock = false;
                    hasRock = true;
                }
            }
        }
    }

    protected override void OnTriggerStay(Collider c)
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
    
    public void stopAgent()
    {
        agent.isStopped = true;
        hasRock = false;
    }
}
