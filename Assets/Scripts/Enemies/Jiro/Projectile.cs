using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private int damage = 20;
    public GameObject jiro;

    void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(jiro.transform.forward * 35f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.gameObject.GetComponent<Player>().doDamage(damage);
            Destroy(gameObject);
        }
    }
}
