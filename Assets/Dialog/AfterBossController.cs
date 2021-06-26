using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBossController : MonoBehaviour
{
    private Animator animator;
    //private Rigidbody rb;
    private Vector3 yForce;
    private Vector3 xForce;
    private float yFloatForce = 35.0f, xFloatForce = 0f;
    float constantXForce = 5.04f;
    public GameObject go_posFinal;
    private Rigidbody father;
    Vector3 posFinal;
    // Start is called before the first frame update
    void Start()
    {
        father = GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.speed = 0;
        StartCoroutine(waitToGetUp(11f));
        yForce = yFloatForce * Vector3.up;
        posFinal = new Vector3(go_posFinal.transform.position.x, 0, go_posFinal.transform.position.z);
    }
    IEnumerator waitToGetUp(float time)
    {
        yield return new WaitForSeconds(time);
        animator.speed = 1;
        Debug.Log(animator.speed);
        StartCoroutine(waitToJump(2f));
    }

    IEnumerator waitToJump(float time)
    {
        yield return new WaitForSeconds(time);
        transform.LookAt(posFinal);
        animator.SetTrigger("Trigger");
        //rb.AddForce(20, ForceMode.Impulse);
        yield return new WaitForSeconds(0.8f);
        jump();
    }

    private void jump()
    {
        Vector3 posJiro = new Vector3 (transform.position.x, 0, transform.position.z);
        

        float distance = Vector3.Distance(posJiro, posFinal);
        xFloatForce = distance / constantXForce;
        
        Vector3 xForce = xFloatForce * transform.forward;
       // xForce = transform.forward * 20f;
        father.AddForce(yForce, ForceMode.Impulse);
        father.AddForce(xForce, ForceMode.Impulse);
    }
}
