using System.Collections;
using UnityEngine;

public class Projectile : EnemyBall
{
    public GameObject jiro;

    protected override void Start()
    {
        damage = 20;
        impulse = 35f;
        //Destroy(gameObject, 5);
        rb.isKinematic = true;
        StartCoroutine(shot(1.4f));  
    }

    IEnumerator shot(float time)
    {
        yield return new WaitForSeconds(time);
        rb.isKinematic = false;
        rb.AddForce(jiro.transform.forward * impulse, ForceMode.Impulse);
    }
}
