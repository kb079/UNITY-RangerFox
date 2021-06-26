using System.Collections;
using UnityEngine;

public class Fairy : Enemy
{
    public GameObject attackObj;
    public GameObject normalAnimation;
    public GameObject deadAnimation;
    private float attackCooldown = 1.5f;
    public AudioSource audio;
    public AudioClip ataque;
    public AudioClip muerte;
    public AudioClip aleteo;

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
        StartCoroutine("reproducirAleteo");
    }

    protected override void attack()
    {
        transform.LookAt(player.transform.position);
        GameObject attackObjClone = Instantiate(attackObj, attackObj.transform.position, attackObj.transform.rotation);
        attackObjClone.SetActive(true);
        cooldown = true;
        //audio.PlayOneShot(ataque);
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

    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            agent.isStopped = true;
            isDead = true;

            normalAnimation.SetActive(false);
            deadAnimation.SetActive(true);
            audio.PlayOneShot(muerte);
  
            StartCoroutine(dropItem(9));
            PlayerStats.getInstance().addXP(GameConstants.Fairy_Exp);
        }
    }

    IEnumerator reproducirAleteo()
    {
        audio.PlayOneShot(aleteo);
        yield return new WaitForSeconds(1f);
        StartCoroutine(reproducirAleteo());
    }
}
