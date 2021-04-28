using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Fairy : Enemy
{
    public GameObject attackObj;
    private float attackCooldown = 1.5f;

    /*
    private float hSpeed = 4f;
    private float vSpeed = 3f;
    private float amplitude = 1.2f;
    */

    void Start()
    {
        health = GameConstants.Fairy_HP;
        attackRadius = 13;
        searchRadius = 25;
        createNavAgent();
    }

    protected override void attack()
    {
       // transform.LookAt(player.transform.position);

        GameObject attackObjClone = Instantiate(attackObj, attackObj.transform.position, attackObj.transform.rotation);
        attackObjClone.SetActive(true);
        cooldown = true;
        StartCoroutine(finishAttackCooldown(attackCooldown));
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
    }
    /*
    protected override void searchPlayer()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        uint distance = (uint)Vector3.Distance(pos1, pos2);
        if (distance <= 30 && distance > 13)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform.position);

            //float y = Mathf.Sin(Time.realtimeSinceStartup * vSpeed) * amplitude;
            //float x = position.x += hSpeed;
        }
        //distance > 6 && 
        else if (distance < 13)
        {
            agent.isStopped = true;

            if (!cooldown)
            {
                attack();
            }
            /*
             * TODO: Si el hada consigue ponerse encima del personaje por arriba
            }else if(distance <= 6)
            {
                transform.position = Vector3.Lerp(Vector3.back, player.transform.forward, Time.deltaTime);
            }
        }
    }
*/


    private void createNavAgent()
    {
        Vector3 sourcePostion = transform.position;
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1))
        {
            transform.position = closestHit.position;
            gameObject.AddComponent<NavMeshAgent>();
            agent = GetComponent<NavMeshAgent>();
            agent.baseOffset = 5;
        }
    }
}
