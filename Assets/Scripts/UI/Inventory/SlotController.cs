using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] int position;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("UIManager").GetComponent<Inventory>();
    }
    public void useItem()
    {
        inventory.useItem(position);
    }
    
}
