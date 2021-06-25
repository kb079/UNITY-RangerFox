using System.Collections;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    bool isAttacking = false;
    private Player player;
    void Start()
    {
        player = Player.getInstance();
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player") )
        {
            if (isAttacking)
            {
                isAttacking = false;
                if (player.barrier.activeInHierarchy)
                {
                    player.useMana(20);
                }
                else
                {
                    player.doDamage(GameConstants.Jiro_Dmg, 1);
                }
            }
        }
    }

    IEnumerator finishCooldown()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = true;
    }

}
