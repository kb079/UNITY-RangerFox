using UnityEngine;

public class DoowellBall : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float impulse = 6f;
    private uint damage = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * impulse, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Player"))
            c.gameObject.GetComponent<Player>().doDamage(damage);

        DoowellController body = c.gameObject.GetComponent<DoowellController>();
        if (body == null)
        {
            Destroy(gameObject);
        }     
    }
}