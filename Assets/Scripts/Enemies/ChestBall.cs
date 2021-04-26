using UnityEngine;

public class ChestBall : EnemyBall
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        damage = GameConstants.Chest_Dmg;
        impulse = 430f;
        ticks = 5;
        Destroy(gameObject, 3);
        rb.AddForce(transform.forward * impulse, ForceMode.Force);
    }
}
