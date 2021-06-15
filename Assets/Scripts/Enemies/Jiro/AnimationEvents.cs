using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject jiroGO;
    private JiroController jiro;
    private void Start()
    {
        jiro = jiroGO.GetComponent<JiroController>();
    }
    public void substract()
    {
        jiro.substractActionCounter();
    }
}
