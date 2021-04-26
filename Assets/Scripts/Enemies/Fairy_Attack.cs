using UnityEngine;

public class Fairy_Attack : EnemyBall
{
    public GameObject fairy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        damage = GameConstants.Fairy_Dmg;
        impulse = 15f;
        Destroy(gameObject, 5);
        rb.AddForce(fairy.transform.forward * impulse, ForceMode.Impulse);
    }
}
