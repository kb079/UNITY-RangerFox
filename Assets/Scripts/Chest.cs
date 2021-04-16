using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpened;
    public GameObject chestDoor;
    
    void Start()
    {
        isOpened = false;
    }

    public void openChest()
    {
        if (!isOpened)
        {
            Vector3 originalPos = chestDoor.transform.eulerAngles;
            originalPos.x = -50;
            chestDoor.transform.eulerAngles = originalPos;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");

            isOpened = true;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && !isOpened)
        {
            c.GetComponent<Player>().setHudText("Press [E] to open chest");
        }
    }

}
