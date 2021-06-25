using System.Collections;
using UnityEngine;

public class WolfTutorial : Wolf
{
    private bool attacked = false;
    public tutorial pegar;
    private bool cambioTexto = false;

    protected override void Start()
    {
        base.Start();
        damage = 50;
        health = 15;
    }

    protected override void Update()
    {
       if (!attacked) base.Update();
       if(health <= 0 && !cambioTexto)
        {
            pegar.changeText(KeyCode.None);
            cambioTexto = true;
        }
    }

    protected override void attack()
    {
        base.attack();
        StartCoroutine(checkAttack(0.63f));
    }

    IEnumerator checkAttack(float time)
    {
        yield return new WaitForSeconds(time);
        if (player.GetComponent<Player>().getHealth() <= 50)
        {
            attacked = true;
            StartCoroutine(endAtack(0.35f));
        }
    }

    IEnumerator endAtack(float time)
    {
        yield return new WaitForSeconds(time);
        agent.isStopped = true;
        animator.SetInteger("id", 0);
    }

    protected override void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("yuki_hand"))
        {
            if (c.gameObject.GetComponentInParent<Player>().isAttacking)
            {
                if (pegar.cont == 3)
                {
                    pegar.changeText(GameConstants.key_attack);
                    doDamage(4);
                }
                if(pegar.cont >= 9)
                {
                    doDamage(4);
                }
                
                c.gameObject.GetComponentInParent<Player>().isAttacking = false;
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
       
        if (c.gameObject.name.Equals("Yukki_ball(Clone)"))
        {
            if(pegar.cont == 5)
            {
                pegar.changeText(GameConstants.key_cameraZoom);
                doDamage(2);
            }
            if(pegar.cont == 7)
            {
                pegar.changeText(GameConstants.key_barrier);
            }
            if(pegar.cont >= 9)
            {
                
                doDamage(2);
            }
            
            Destroy(c.gameObject);
        }
    }
}
