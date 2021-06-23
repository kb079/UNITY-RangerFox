using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemObject itemType;
    public int stack = 1;
    private InventoryObject inventory;
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("UIManager").GetComponent<InventoryObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        inventory.addItem(itemType, stack);
        Destroy(gameObject);
    }
}
