using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetaBall : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float impulse = 6f;
    private int damage = 3;
    private float ticks = 5;
    public GameObject barrier;

    void Start()
    {
        Destroy(gameObject, 3);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * impulse, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if(barrier.activeInHierarchy == false) { 
                c.gameObject.GetComponent<Player>().poisonDamage(ticks, damage);
                Destroy(gameObject);
            } else
            {
                c.gameObject.GetComponent<Player>().useMana(ticks*damage);
                Destroy(gameObject);
            }
        }
    }
}
