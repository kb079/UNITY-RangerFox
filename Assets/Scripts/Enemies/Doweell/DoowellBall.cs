using UnityEngine;

public class DoowellBall : EnemyBall
{
    protected override void Start()
    {
        damage = GameConstants.Doowell_Dmg;
        impulse = 32f;
        base.Start();
    }
    new private void OnTriggerEnter(Collider c) { }
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            Player p = c.gameObject.GetComponent<Player>();
            if (!p.isBarrierActive)
            {
                p.doDamage(damage, ticks);
            }
            else
            {
                p.useMana(ticks * damage);
            }
            Destroy(gameObject);
        }
        DoowellController body = c.gameObject.GetComponent<DoowellController>();
        if (body == null) Destroy(gameObject);   
    }
}