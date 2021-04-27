using UnityEngine;

public class ChestBall : EnemyBall
{
    protected override void Start()
    {
        damage = GameConstants.Chest_Dmg;
        impulse = 430f;
        ticks = 5;
        Destroy(gameObject, 3);
        base.Start();
    }
}
