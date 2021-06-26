public class SetaBall : EnemyBall
{
    protected override void Start()
    {
        damage = GameConstants.Seta_Dmg;
        impulse = 10f;
        ticks = 5;
        Destroy(gameObject, 3);
        base.Start();
    }
}
