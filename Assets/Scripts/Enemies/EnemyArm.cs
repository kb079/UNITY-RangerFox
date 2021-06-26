using System.Collections;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    public bool isAttacking = false;
    private Player player;
    void Start()
    {
        player = Player.getInstance();
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player") && !isAttacking)
        {
           if (player.barrier.activeInHierarchy)
           {
                player.useMana(20);
           }
           else
           {
                isAttacking = true;
                player.doDamage(GameConstants.Jiro_Dmg, 1);
                StartCoroutine(finishCooldown());
           }
        }
    }

    IEnumerator finishCooldown()
    {
        yield return new WaitForSeconds(1.045f);
        isAttacking = false;
    }

}
