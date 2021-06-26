using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOriginalPlace : StateMachineBehaviour
{
    public GameObject jiro;
    public GameObject jiroBody;

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jiroBody.transform.position = jiro.transform.position;
    }
}
