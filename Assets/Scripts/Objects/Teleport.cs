using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public AudioClip used;
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
                GetComponent<AudioSource>().PlayOneShot(used);
                
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
                SceneManager.LoadSceneAsync("Madriguera");
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
