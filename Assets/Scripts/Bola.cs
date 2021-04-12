using UnityEngine;

public class Bola : MonoBehaviour
{
    private Camera player;
    private Rigidbody rb;

    void Start()
    {
        Destroy(gameObject, 5);
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("playerCam").GetComponent<Camera>();
        Vector3 originalPos = transform.position;
        //originalPos.x = player.transform.position.x;
       // originalPos.z = player.transform.position.z;
        transform.localPosition = originalPos + player.transform.forward;

        
       // Vector3 originalPos = transform.eulerAngles;
       //originalPos.y = playerVision;
       //transform.eulerAngles = originalPos;

    }

    void Update()
    {
        rb.AddForce(player.transform.forward * 40f);
        

       // float step = 8 * Time.deltaTime;
        //transform.localPosition = player.transform.position + player.transform.forward * step;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("enemy"))
        {
            c.gameObject.GetComponent<Enemy>().doDamage();
            Destroy(gameObject);
        }
    }
}
