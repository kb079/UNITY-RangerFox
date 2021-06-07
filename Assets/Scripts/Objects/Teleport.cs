using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public Transform teleportTarget;
    public GameObject thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(GameConstants.key_interact))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
                thePlayer.transform.position = teleportTarget.transform.position;
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.GetComponent<Player>().setHudText("Press [" + GameConstants.key_interact.ToString() + "] to teleport to home");
        }
    }

    private void OnTriggerExit(Collider c)
    {
        Player p = c.GetComponent<Player>();
        if (p != null) p.setHudText("");
    }
}
