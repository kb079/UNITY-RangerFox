using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpened;
    public GameObject chestDoor;
    private Inventory inventory;
    
    void Start()
    {
        inventory  = GameObject.FindGameObjectWithTag("UIManager").GetComponent<Inventory>();
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
            inventory.addToInventory(GameConstants.it_healing_2);
            isOpened = true;
        }
    }
    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(GameConstants.key_interact))
            {
                openChest();
            }
        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && !isOpened)
        {
            c.GetComponent<Player>().setHudText("Press [" + GameConstants.key_interact.ToString() + "] to open chest");
        }
    }
    private void OnTriggerExit(Collider c)
    {
        c.GetComponent<Player>().setHudText("");
    }
}
