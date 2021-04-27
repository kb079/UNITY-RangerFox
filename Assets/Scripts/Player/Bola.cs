using UnityEngine;

public class Bola : MonoBehaviour
{
    private Camera player;
    private Rigidbody rb;
    [SerializeField] int damage = 8;

    void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("playerCam").GetComponent<Camera>();
        Vector3 originalPos = transform.position;
        //originalPos.x = player.transform.position.x;
       // originalPos.z = player.transform.position.z;
        transform.localPosition = originalPos + player.transform.forward;
        rb.AddForce((player.transform.forward + new Vector3(0, 0.25f, 0)) * 40f, ForceMode.Impulse);

        // Vector3 originalPos = transform.eulerAngles;
        //originalPos.y = playerVision;
        //transform.eulerAngles = originalPos;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("enemy"))
        {
            c.gameObject.GetComponent<Enemy>().doDamage(damage);
            Destroy(gameObject);
        }
    }
}
