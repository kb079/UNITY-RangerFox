using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle_lookat : MonoBehaviour
{
    public Animator parentAn;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (parentAn.GetInteger("id") == 1)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }
}
