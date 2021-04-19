using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    private Inventory inventory;
    private Image image;
    [SerializeField] int position;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("UIManager").GetComponent<Inventory>();
        image = GetComponent<Image>();
    }
    public void useItem()
    {
        inventory.useItem(position, image);
    }
    
}
