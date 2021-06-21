using System.Collections;
using UnityEngine;

public class WolfTutorial : Wolf
{
    private bool attacked = false;

    protected override void Start()
    {
        base.Start();
        damage = 50;
    }

    protected override void Update()
    {
       if (!attacked) base.Update();
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
}
