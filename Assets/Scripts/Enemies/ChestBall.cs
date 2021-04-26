using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBall : MonoBehaviour
{
    private Rigidbody rb;
    private float impulse = 700f;
    private int damage = 5;
    private float ticks = 5;

    void Start()
    {
        Destroy(gameObject, 3);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * impulse, ForceMode.Force);
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.gameObject.GetComponent<Player>().poisonDamage(ticks, damage);
            Destroy(gameObject);
        }
    }
}
