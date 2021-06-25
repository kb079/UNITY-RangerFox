using UnityEngine;

public class Fairy_Attack : EnemyBall
{
    public GameObject fairy;

    protected override void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        damage = GameConstants.Fairy_Dmg;
        impulse = 15f;
        Destroy(gameObject, 5);
        rb.AddForce(fairy.transform.forward * impulse, ForceMode.Impulse);
    }
}
