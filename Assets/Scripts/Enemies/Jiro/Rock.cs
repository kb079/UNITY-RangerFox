using System.Collections;
using UnityEngine;

public class Rock : EnemyBall
{
    public GameObject jiroGO;
    private JiroController jiro;

    protected override void Start()
    {
        damage = 18;
        jiro = jiroGO.GetComponent<JiroController>();
        float t = Random.Range(0f, 1.4f);
        StartCoroutine(cor_init(t));
        //(Temporal)
        Destroy(gameObject, 10);
    }

    IEnumerator cor_init(float time)
    {
        yield return new WaitForSeconds(time);
        thrown();
    }

    private void thrown()
    {
        if (!jiro.isJiroDead())
        {
            rb.isKinematic = false;
            float yImpulse = Random.Range(16.0f, 40.0f);
            float xImpulse = Random.Range(18.0f, 36.0f);
            float scale = Random.Range(0.6f, 8.5f);
            transform.localScale = new Vector3(scale, scale, scale);
            rb.AddForce(Vector3.up * yImpulse, ForceMode.Impulse);
            rb.AddForce(transform.forward * xImpulse, ForceMode.Impulse);
        }
    }
}
