using UnityEngine;

public abstract class EnemyBall : MonoBehaviour
{
    protected int damage;
    
    protected float ticks = 1;
    protected float impulse;

    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        rb.AddForce(transform.forward * impulse, ForceMode.Impulse);
    }

    protected void OnTriggerEnter(Collider c)
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
    }
}
