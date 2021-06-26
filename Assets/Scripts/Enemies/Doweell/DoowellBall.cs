using System.Collections;
using UnityEngine;

public class DoowellBall : EnemyBall
{
    public GameObject explosion;
    public GameObject doowell;
    protected override void Start()
    {
        damage = GameConstants.Doowell_Dmg;
        impulse = 32f;
        base.Start();
    }
    new private void OnTriggerEnter(Collider c) { }


    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject != doowell)
        {
            GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
            exp.SetActive(true);
        }

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