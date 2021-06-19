using UnityEngine;

public class ChestBall : EnemyBall
{
    protected override void Start()
    {
        damage = GameConstants.Chest_Dmg;
        impulse = 600f;
        ticks = 5;
        Destroy(gameObject, 3);
        rb.AddForce(transform.forward * impulse, ForceMode.Force);
    }
}
