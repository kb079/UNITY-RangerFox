using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoowellBall : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float impulse = 6f;
    private uint damage = 20;


    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Player"))
            c.gameObject.GetComponent<Player>().doDamage(damage);
        Destroy(gameObject);

    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * impulse, ForceMode.Force);
    }
}
