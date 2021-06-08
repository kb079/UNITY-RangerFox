using System.Collections;
using UnityEngine;

public class DoowellController : Enemy
{

    public GameObject[] animations;
    [SerializeField] GameObject[] cabezas;
    [SerializeField] GameObject bala;
    public int actionRadio = 30, attackRadio = 3;
    public float velocity = 10f;
    private int id;
    private int count = 0;
    private bool isRunning = false;
    private bool isIdle = false;
    private bool isActive = false;
    private Vector3 pos1;
    private Vector3 pos2;

    void Start()
    {
        isAttacking = false;
        cooldown = false;
        health = GameConstants.Doowell_HP;
        bala.SetActive(false);
        changeAnimation(0);
        changeCabeza(0);
    }
    private void changeAnimation(int i)
    {
        animations[0].SetActive(false);
        animations[1].SetActive(false);
        animations[2].SetActive(false);
        animations[3].SetActive(false);
        animations[4].SetActive(false);
        animations[i].SetActive(true);
        id = i;
    }
    private void changeCabeza(int i)
    {
        cabezas[0].SetActive(false);
        cabezas[1].SetActive(false);
        cabezas[2].SetActive(false);
        cabezas[3].SetActive(false);
        if (cooldown == false)
        {
            cabezas[i].SetActive(true);
        }

    }
    protected override void attack()
    {
        isAttacking = true;
        cooldown = true;
        changeAnimation(2);
        StartCoroutine(waitTo(actions.FINISH_ATTACK, 1.2f));
        transform.LookAt(player.transform.position);
        bala.transform.LookAt(player.transform.position);

        StartCoroutine(waitTo(actions.FINISH_COOLDOWN, 5f));
    }

    IEnumerator waitTo(actions i, float time)
    {
        yield return new WaitForSeconds(time);
        if (!isDead)
        {
            switch (i)
            {
                case actions.FINISH_ATTACK:
                    isAttacking = false;
                    GameObject clon = Instantiate(bala, bala.transform.position, bala.transform.rotation);
                    clon.SetActive(true);
                    break;
                case actions.FINISH_COOLDOWN:
                    cooldown = false;
                    break;
                case actions.FINISH_IDLE:
                    isIdle = false;
                    break;
                default:
                    break;
            }
        }

    }

    protected override void checkHP()
    {
        base.checkHP();
        if (isDead == true)
        {
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 0;
            transform.localEulerAngles = originalRot;
            changeAnimation(4);
            Destroy(gameObject, 3);
        }

    }
    protected override void searchPlayer()
    {
        pos1 = transform.position;
        pos2 = player.transform.position;
        changeCabeza(id);
        int distance = (int)Vector3.Distance(pos1, pos2);
        if (distance <= actionRadio)
        {
            isActive = true;
            count = 0;
            if (!cooldown)//puede atacar
            {
                if (distance <= attackRadio)
                {
                    //Ataca
                    isRunning = false;
                    isIdle = false;
                    agent.isStopped = true;
                    attack();
                }
                else
                {
                    //Persigue al jugador
                    isIdle = false;
                    if (isRunning == false && isAttacking == false)
                    {
                        changeAnimation(1);
                        isRunning = true;
                        agent.isStopped = false;
                        agent.SetDestination(pos2);
                    }

                }
            }
            else
            {
                if (distance < attackRadio)
                {
                    //Huir
                    isIdle = false;
                    if (isRunning == false && isAttacking == false)
                    {
                        changeAnimation(1);
                        isRunning = true;
                        agent.isStopped = false;
                        Vector3 runTo = pos1 + ((pos1 - pos2) * velocity);
                        agent.SetDestination(runTo); ;
                    }

                }
                else
                {
                    //Pararse
                    isRunning = false;
                    if (isIdle == false && isAttacking == false)
                    {
                        changeAnimation(0);
                        isIdle = true;
                        agent.isStopped = true;
                    }
                }
            }
        }
        else
        {
            isActive = false;
            isRunning = false;
            if (isIdle == false)
            {
                isIdle = true;
                changeAnimation(0);
                animations[0].SetActive(true);
            }
            rocking();
        }
    }
    private void rocking()
    {
        if (isActive == false)
        {
            count++;
            if (count % 100 == 0)
            {

                changeAnimation(3);
                changeCabeza(3);
                StartCoroutine(waitTo(actions.FINISH_IDLE, 1f));
            }
        }
    }
}
enum actions
{
    FINISH_ATTACK,
    FINISH_COOLDOWN,
    FINISH_IDLE
}
