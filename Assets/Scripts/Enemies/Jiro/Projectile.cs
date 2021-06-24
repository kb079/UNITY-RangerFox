using System.Collections;
using UnityEngine;

public class Projectile : EnemyBall
{
    public GameObject jiro;
    private GameObject player;
    private bool isGrowing = true;
    private float scaleFactor = 0.06f;
    private float adjustYPosition;

    private void Update()
    {
        if (isGrowing)
        {
            float scale = (float)(transform.localScale.x + scaleFactor);
            Vector3 s = new Vector3(scale, scale, scale);
            Vector3 p = new Vector3(transform.position.x, transform.position.y + adjustYPosition, transform.position.z);
            transform.localScale = s;
            transform.position = p;
        }
    }

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        adjustYPosition = scaleFactor / 2;
        damage = 50;
        impulse = 25f;
        StartCoroutine(cor_prepare_attack());
    }

    IEnumerator cor_prepare_attack()
    {
        yield return new WaitForSeconds(2.6f);
        isGrowing = false;
        StartCoroutine(cor_shot());
    }

    public IEnumerator cor_shot()
    {
        yield return new WaitForSeconds(0.6f);
        shot();
    }

    public void shot()
    {
        rb.isKinematic = false;
        transform.LookAt(player.transform);
        rb.AddForce(transform.forward * impulse, ForceMode.Impulse);
    }
}
