using System.Collections;
using UnityEngine;

public class DoowellController : Enemy
{
    [SerializeField] GameObject cabeza;
    [SerializeField] GameObject bala;
    public int actionRadio = 30, attackRadio = 3;
    public float velocity = 1f;

    void Start()
    {
        health = GameConstants.Doowell_HP;
        bala.SetActive(false);
    }

    protected override void attack()
    {
        transform.LookAt(player.transform.position);
        bala.transform.LookAt(player.transform.position);
        cooldown = true;
        isAttacking = true;
        cabeza.SetActive(false);
        GameObject clon = Instantiate(bala, bala.transform.position, bala.transform.rotation);
        clon.SetActive(true);
        StartCoroutine(finishAttackCooldown(5f));
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
        cabeza.SetActive(true);
    }

    protected override void searchPlayer()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        int distance = (int)Vector3.Distance(pos1, pos2);
        if(distance <= actionRadio)
        {
            if (!cooldown)//puede atacar
            {
                if(distance<= attackRadio)
                {
                    //Ataca
                    agent.isStopped = true;
                    attack();
                }
                else
                {
                    //Persigue al jugador
                    agent.isStopped = false;
                    agent.SetDestination(pos2);
                }
            }
            else
            {
                if(distance < attackRadio)
                {
                    //Huir
                    agent.isStopped = false;
                    Vector3 runTo = pos1 + ((pos1 - pos2) * velocity);
                    agent.SetDestination(runTo);
                }
                else
                {
                    //Pararse
                    agent.isStopped = true;
                }
            }
        }
    }
}
