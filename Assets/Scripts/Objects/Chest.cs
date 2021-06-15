using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpened;
    private InventoryObject inventory;
    private Animator animator;

    void Start()
    {
        inventory  = GameObject.FindGameObjectWithTag("UIManager").GetComponent<InventoryObject>();
        isOpened = false;
        animator = GetComponent<Animator>();
    }

    public void openChest()
    {
        if (!isOpened)
        {
           
            animator.SetBool("boton", true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
            inventory.addItem(inventory.itemTypes[1], 1);
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
        Player p = c.GetComponent<Player>();
        if(p != null) p.setHudText("");
    }
}
