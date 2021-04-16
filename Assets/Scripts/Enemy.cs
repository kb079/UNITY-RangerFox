using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public virtual void doDamage(int dmg) { }

    protected void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("playerHand"))
        {
            if (c.gameObject.GetComponentInParent<Player>().isAttacking)
            {
                doDamage(5);
                c.gameObject.GetComponentInParent<Player>().isAttacking = false;
            }
        }
    }

}