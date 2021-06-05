using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArm : MonoBehaviour
{
    public GameObject playerGO;
    private Player player;
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(playerGO))
        {
            if (player.barrier.activeInHierarchy)
            {
                player.useMana(20);
            } else
            {
                player.doDamage(20, 1);
            }
        }
    }
}
