using UnityEngine;

public class Bola : MonoBehaviour
{
    private Camera playerCam;
    private Rigidbody rb;

    void Start()
    {
        Destroy(gameObject, 5);

        playerCam = GameObject.FindGameObjectWithTag("playerCam").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        
        //rb.AddForce(transform.forward + playerCam.transform.forward * 2000f, ForceMode.Acceleration);
        rb.AddForce((playerCam.transform.forward + new Vector3(0, 0.35F, 0)) * 30F, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("enemy"))
        {
            c.gameObject.GetComponent<Enemy>().doDamage(GameConstants.ballDamage);
            Destroy(gameObject);
        }
    }
}