using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    /*
    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player") )
        {
            if (player.barrier.activeInHierarchy)
            {
                player.useMana(20);
            }else
            {
                player.doDamage(20, 1);
            }
        }
    }*/
}
