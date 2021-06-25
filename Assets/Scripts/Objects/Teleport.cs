using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public AudioClip used;

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(GameConstants.key_interact))
            {
                GetComponent<AudioSource>().PlayOneShot(used);

                PlayerSavingData.savePlayerData();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.GetComponent<Player>().setHudText("Pulsa [" + GameConstants.key_interact.ToString() + "] para guardar");
        }
    }

    private void OnTriggerExit(Collider c)
    {
        Player p = c.GetComponent<Player>();
        if (p != null) p.setHudText("");
    }
}
