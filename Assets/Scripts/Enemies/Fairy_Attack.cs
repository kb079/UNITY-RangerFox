using UnityEngine;

public class Fairy_Attack : MonoBehaviour
{
    private Rigidbody rb;
    private uint damage = 8;
    public GameObject fairy;

    void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(fairy.transform.forward * 15f, ForceMode.Impulse);
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
