using System.Collections;
using UnityEngine;

public class DoowellController : Enemy
{
    [SerializeField] GameObject[] animations;
    [SerializeField] GameObject[] cabezas;
    [SerializeField] GameObject bala;
    private Rigidbody rb;
    public int actionRadio = 30, attackRadio = 15;
    public float velocity = 10f;
    private bool isRunning = false;
    private bool isIdle = false;
    [SerializeField] GameObject cabeza;
    [SerializeField] GameObject cabezaRock;
    private bool isActive = false;
    private Vector3 pos1;
    private Vector3 pos2;
    private bool isHeadEnabled = true;
    private bool hasDiedOnce = false;

    public AudioSource audiosource;
    [SerializeField] AudioClip[] sonidos;
    private enum enum_sounds { Dead = 0, Run = 1 }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isAttacking = false;
        cooldown = false;
        health = GameConstants.Doowell_HP;
        bala.SetActive(false);
        changeAnimation(0);
    }

    protected override void Update()
    {
        if (!isDead)
        {
            searchPlayer();
        } else
        {
            Debug.Log("isdfeadddd");
        }
    }
    private void playSound(enum_sounds sonido)
    {
        audiosource.PlayOneShot(sonidos[(int)sonido]);
    }
    private void changeAnimation(int i)
    {
        
        animations[0].SetActive(false);
        animations[1].SetActive(false);
        animations[2].SetActive(false);
        animations[3].SetActive(false);
        animations[4].SetActive(false);
        animations[5].SetActive(false);

        animations[i].SetActive(true);
    }


    protected override void attack()
    {
        isAttacking = true;
        cooldown = true;
        changeAnimation(2);
        StartCoroutine(waitTo(actions.FINISH_ATTACK, 0.9f));
        // Escondo la cabeza al lanzarla
        StartCoroutine(waitTo(actions.HEAD_ACTIVITY, 0.9f));
        transform.LookAt(player.transform.position);
        bala.transform.LookAt(player.transform.position);
        // Tras este tiempo la cabeza volverá a ser visible
        StartCoroutine(waitTo(actions.HEAD_ACTIVITY, 3f));
        StartCoroutine(waitTo(actions.FINISH_COOLDOWN, 5f));
    }

    IEnumerator waitTo(actions i, float time)
    {
        Debug.Log("mira la corrutiiiina");
        yield return new WaitForSeconds(time);
        if (!isDead || hasDiedOnce)
        {
            switch (i)
            {
                case actions.FINISH_ATTACK:
                    isAttacking = false;
                    GameObject clon = Instantiate(bala, bala.transform.position, bala.transform.rotation);
                    clon.transform.LookAt(player.transform);
                    clon.SetActive(true);
                    break;
                case actions.FINISH_COOLDOWN:
                    cooldown = false;
                    break;
                case actions.FINISH_IDLE:
                    isIdle = false;
                    break;
                case actions.HEAD_ACTIVITY:
                    isHeadEnabled = !isHeadEnabled;
                    break;
                case actions.REVIVE_PART_1:
                    playSound(enum_sounds.Dead);
                    changeAnimation(5);
                    StartCoroutine(waitTo(actions.REVIVE_PART_2, 1f));
                    break;
                case actions.REVIVE_PART_2:
                    cabeza.SetActive(true);
                    isDead = false;
                    isAttacking = false;
                    cooldown = false;
                    health = GameConstants.Doowell_HP;
                    healthBar.fillAmount = 1;
                    changeAnimation(0);
                    rb.isKinematic = false;
                    break;
                default:
                    break;
            }
        }

    }

    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            playSound(enum_sounds.Dead);
            StopAllCoroutines();
            agent.isStopped = true;
            isDead = true;
            rb.isKinematic = true;
            changeAnimation(4);
            if (!hasDiedOnce)
            {
                cabeza.SetActive(false);
                cabezaRock.SetActive(false);
                hasDiedOnce = true;
                StartCoroutine(waitTo(actions.REVIVE_PART_1, 2f));
            } else
            {
                cabeza.SetActive(false);
                cabezaRock.SetActive(false);
                Destroy(gameObject, 3);
                Destroy(agent);
            }
        }

    }
    protected override void searchPlayer()
    {
        if (isIdle)
        {
            cabezaRock.SetActive(isHeadEnabled);
            cabeza.SetActive(false);
        } else
        {
            cabeza.SetActive(isHeadEnabled);
            cabezaRock.SetActive(false);
        }
        pos1 = transform.position;
        pos2 = player.transform.position;
        int distance = (int)Vector3.Distance(pos1, pos2);
        if (distance <= actionRadio)
        {
            agent.SetDestination(pos2);
            isActive = true;
            
            //count = 0;

            if (!cooldown)//puede atacar
            {
                agent.SetDestination(pos2);
                if (distance <= attackRadio)
                {
                    //Debug.Log("Debería atacar");
                    //Ataca
                    isRunning = false;
                    isIdle = false;
                    agent.isStopped = true;
                    attack();
                }
                else
                {
                    //Debug.Log("Debería perseguir");
                    //Persigue al jugador
                    if (!audiosource.isPlaying)
                        playSound(enum_sounds.Run);

                    isIdle = false;
                    if (isRunning == false && isAttacking == false)
                    {
                        changeAnimation(1);
                        isRunning = true;
                        agent.isStopped = false;
                    }
                }
            }
            else
            {
                if (distance < attackRadio)
                {
                    //Debug.Log("Debería huír");
                    //Huir
                    Vector3 runTo = pos1 + pos1 - pos2;
                    agent.SetDestination(runTo);
                    isIdle = false;
                    if (!audiosource.isPlaying)
                        playSound(enum_sounds.Run);
                    if (isRunning == false && isAttacking == false)
                    {
                        changeAnimation(1);
                        isRunning = true;
                        agent.isStopped = false;
                    }

                }
                else
                {
                    //Debug.Log("Debería pararme");
                    //Pararse
                    isRunning = false;
                    if (isIdle == false && isAttacking == false)
                    {
                        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                        changeAnimation(0);
                        isIdle = true;
                        agent.isStopped = true;
                    }
                }
            }
        }
       /* else
        {
            isActive = false;
            isRunning = false;
            if (isIdle == false)
            {
                isIdle = true;
                changeAnimation(0);
            }
            //rocking();
        }*/
    }
    /*private void rocking()
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
    }*/
}
enum actions
{
    FINISH_ATTACK,
    FINISH_COOLDOWN,
    FINISH_IDLE,
    HEAD_ACTIVITY,
    REVIVE_PART_1,
    REVIVE_PART_2
}