using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoowellController : Enemy

{
    [SerializeField] GameObject cabeza;
    [SerializeField] GameObject bala;
    private int health = 15;
    public int actionRadio = 30, attackRadio = 3;
    private GameObject head;
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    private bool isDeath;
    public float velocity = 1f;
    private bool cooldown = false;
    public bool isAttacking;
    private Vector3 pos1, pos2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        isAttacking = false;
        bala.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeath)
        {
            pos1 = transform.position;
            pos2 = player.transform.position;
            searchPlayer();
        }
    }

    private void attack()
    {
        transform.LookAt(pos2);
        bala.transform.LookAt(pos2);
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

    private void searchPlayer()
    {


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

    private void checkHP()
    {
        if (health <= 0 && !isDeath)
        {
            StopAllCoroutines();
            Destroy(agent);
            isDeath = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
        }
    }

    public override void doDamage(int dmg)
    {
        health -= dmg;
        checkHP();
    }
}
