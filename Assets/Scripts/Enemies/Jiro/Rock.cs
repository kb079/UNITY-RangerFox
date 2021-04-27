using UnityEngine;

public class Rock : EnemyBall
{
    public GameObject jiro;

    private GameObject player;
    private bool thrown;

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damage = 12;
        impulse = 35f;
        thrown = false;
        //Destroy(gameObject, 5);
    }

    private void Update()
    {
        if(!thrown) checkPlayerNear();
    }

    private void checkPlayerNear()
    {
        Vector3 pos1 = jiro.transform.position;
        Vector3 pos2 = player.transform.position;

        if ((uint)Vector3.Distance(pos1, pos2) < 24)
        {
            jiro.transform.LookAt(player.transform.position);
            jiro.GetComponent<Jiro>().stopAgent();
            rb.isKinematic = false;
            rb.AddForce(jiro.transform.forward * impulse, ForceMode.Impulse); 
            thrown = true;
        }
    }
}
