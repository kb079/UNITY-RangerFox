using System.Collections;
using UnityEngine;

public class Wolf : Enemy
{
    private float attackCooldown = 1.9f;
    protected int damage = GameConstants.Wolf_Dmg;

    protected Animator animator;
    private AudioSource audiosource;

    private int counterBreath = 0;
    private enum enum_sounds { Attack = 0, Breath = 1, Dead = 2 }
    
    [SerializeField] AudioClip[] sonidos;

    protected virtual void Start()
    {
        audiosource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        health = GameConstants.Wolf_HP;
        isAttacking = false;
        attackRadius = 3;
    }

    private void playSound(enum_sounds sonido)
    {
        audiosource.PlayOneShot(sonidos[(int)sonido]);
    }

    protected override void Update()
    {
        if (!isDead && player.GetComponent<Player>().getHealth() > 0) searchPlayer();
    }

    protected override void searchPlayer()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        counterBreath++;
        int distance = (int)Vector3.Distance(pos1, pos2);
        int random = UnityEngine.Random.Range(200, 1000);
        if (distance <= searchRadius && counterBreath % random == 0)
        {
            counterBreath = 0;
            playSound(enum_sounds.Breath);
        }

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
        transform.LookAt(player.transform);
        animator.SetInteger("id", 2);
        cooldown = true;
        isAttacking = true;
        playSound(enum_sounds.Attack);
        
        StartCoroutine(finishAttackCooldown(attackCooldown));
    }

    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            agent.isStopped = true;
            isDead = true;
            playSound(enum_sounds.Dead);
            animator.SetInteger("id", 3);
            if (PlayerStats.getInstance() != null)
            {
                PlayerStats.getInstance().addXP(GameConstants.Wolf_Exp);
            }
            Destroy(healthBarClone.gameObject, 1f);
            StartCoroutine(dropItem(3f));
            
        }
    }

    IEnumerator damageAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        doPlayerDamage(damage);
    }

    IEnumerator finishAttackCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
        isAttacking = false;
        animator.SetInteger("id", 0);
    }

    public void doAttack()
    {
        isAttacking = false;
        doPlayerDamage(damage);
        //StartCoroutine(damageAnimation(0.15f));
    }
}
