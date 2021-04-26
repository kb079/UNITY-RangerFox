using UnityEngine;

public class SetaBall : EnemyBall
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        damage = GameConstants.Seta_Dmg;
        impulse = 10f;
        ticks = 5;
        Destroy(gameObject, 3);
        rb.AddForce(transform.forward * impulse, ForceMode.Impulse);
    }
}
