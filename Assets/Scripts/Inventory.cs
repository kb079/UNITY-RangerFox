using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    protected Player player;
    protected List<Image> images;
    protected List<Text> texts;
    protected List<MyItem> items;
    private void Start()
    {;
        items = new List<MyItem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject[] itmSlts = GameObject.FindGameObjectsWithTag("Item");
        images = new List<Image>();
        texts = new List<Text>();
        foreach (GameObject go in itmSlts)
        {
            images.Add(go.GetComponent<Image>());
            texts.Add(go.GetComponentInChildren<Text>());
        }

        //"database" de items (id, tipo, nombre del sprite, valor, stack actual, stack máximo, ¿está ya en el inventario?)
        items.Add(new MyItem(0, 0, "emptySlot", 0, 0, 0, false));
        items.Add(new MyItem(1, 1, "healing1", 20, 0, 5, false));
        items.Add(new MyItem(2, 2, "mana1", 20, 0, 3, false));
    }
    
    //el item se encuentra con la imagen actual en el slot
    //se define la acción a realizar dependiendo del tipo de item (curar, aumentar magia...)
    //y se baja el stack de dicho item
    //si no hay ningún item, no se realiza ninguna acción
    public void useItem(int position, Image image)
    {
        if (image.sprite.name == "emptySlot") return;
        MyItem item = getItem(image.sprite.name);
        removeFromStack(position, item.Id);
        switch (item.Type)
        {
            case 1:
                player.addHealth(item.Value);
                break;
            case 2:
                player.addMana(item.Value);
                break;
            default:
                return;
        }
        
        //si no quedan items de ese tipo, el slot se vacía
        if (item.Stack <= 0)
        {
            image.sprite = Resources.Load<Sprite>("Sprites/Items/emptySlot");
        }
    }
    public void addToInventory(string sprName, int itemId)
    {
        int i = -1;
        MyItem aux = items[itemId];
        foreach (Image img in images)
        {
            i++;
            if (img.sprite.name == "emptySlot" && aux.IsInInventory == false)
            {
                img.sprite = Resources.Load<Sprite>("Sprites/Items/" + sprName);
                items[itemId].IsInInventory = true;
                addToStack(i, aux.Id);
                return;
            }
            else if (img.sprite.name == sprName)
            {
                addToStack(i, aux.Id);
                return;
            }
        }
    }

    public MyItem getItem(string sprName)
    {
        return items.Find(item => item.SprName == sprName);
    }

    public void addToStack(int position, int id)
    {
        if (items[id].Stack == items[id].MaxStack) return;
        items[id].Stack += 1;
        string nText = "x" + items[id].Stack.ToString();
        texts[position].text = nText;
    }

    public void removeFromStack(int position, int id)
    {
        items[id].Stack -= 1;
        uint currentStack = items[id].Stack;
        string nText = "x" + currentStack;
        if (currentStack <= 0)
        {
            nText = "";
            items[id].IsInInventory = false;
        }
        texts[position].text = nText;
    }

}