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
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject[] itmSlts = GameObject.FindGameObjectsWithTag("Item");
        items = new List<MyItem>();
        images = new List<Image>();
        texts = new List<Text>();
        foreach (GameObject go in itmSlts)
        {
            images.Add(go.GetComponent<Image>());
            texts.Add(go.GetComponentInChildren<Text>());
        }
        createItemDB();
        updateAllSlotTexts();
    }

    //"database" de items
    private void createItemDB()
    {
        //(id, tipo, nombre del sprite, valor, stack actual, stack máximo, ¿está ya en el inventario?)
        items.Add(new MyItem(0, 0, GameConstants.it_no_item, 0, 0, 0, false));
        items.Add(new MyItem(1, 1, GameConstants.it_healing_1, 20, 0, 5, false));
        items.Add(new MyItem(2, 2, GameConstants.it_mana_1, 20, 0, 3, false));
        items.Add(new MyItem(3, 1, GameConstants.it_healing_2, 50, 0, 1, false));
    }

    //el item se encuentra con la imagen actual en el slot
    //se define la acción a realizar dependiendo del tipo de item (curar, aumentar magia...)
    //y se baja el stack de dicho item
    //si no hay ningún item, no se realiza ninguna acción
    public void useItem(int position)
    {
        Image image = images[position];
        if (image.sprite.name == GameConstants.it_no_item) return;
        MyItem item = getItem(image.sprite.name);
        removeFromStack(position, item.Id);
        // 1: item de curación; 2: item de maná 
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
        if (item.IsInInventory == false)
        {
            image.sprite = Resources.Load<Sprite>("Sprites/Items/" + GameConstants.it_no_item);
            //si el slot no es el último, los items que se encontrasen a la derecha se desplazan 1 casilla a la izquierda
            for (int i = position; i < images.Count - 1; i++)
            {
                if (images[i].sprite.name == GameConstants.it_no_item && images[i + 1].sprite.name != GameConstants.it_no_item)
                {
                    swapSlots(images[i], images[i + 1]);
                }
            }
            updateAllSlotTexts();
        }
    }
    public void addToInventory(string sprName)
    {
        int i = -1;
        foreach (Image img in images)
        {
            i++;
            if (img.sprite.name == GameConstants.it_no_item)
            {
                img.sprite = Resources.Load<Sprite>("Sprites/Items/" + sprName);
                int itemId = getItem(sprName).Id;
                items[itemId].IsInInventory = true;
                addToStack(i, itemId);
                return;
            }
            else if (img.sprite.name == sprName)
            {
                int itemId = getItem(sprName).Id;
                addToStack(i, itemId);
                return;
            }
        }
    }

    public MyItem getItem(string sprName)
    {
        return items.Find(item => item.SprName == sprName);
    }

    public void addToStack(int position, int itemId)
    {
        if (items[itemId].Stack == items[itemId].MaxStack) return;
        items[itemId].Stack += 1;
        updateSlotText(position);
    }

    public void removeFromStack(int position, int itemId)
    {
        items[itemId].Stack -= 1;
        if (items[itemId].Stack == 0)
        {
            items[itemId].IsInInventory = false;
            return;
        }
        updateSlotText(position);
    }

    private void swapSlots(Image img1, Image img2)
    {
        string aux = img1.sprite.name;
        img1.sprite = Resources.Load<Sprite>("Sprites/Items/" + img2.sprite.name);
        img2.sprite = Resources.Load<Sprite>("Sprites/Items/" + aux);
    }

    private void updateSlotText(int pos)
    {
        string name = images[pos].sprite.name;
        string nText = "";
        if (name != GameConstants.it_no_item)
        {
            MyItem item = getItem(name);
            nText = "x" + item.Stack;
        }
        texts[pos].text = nText;
    }

    private void updateAllSlotTexts()
    {
        for (int i = 0; i < images.Count; i++)
        {
            updateSlotText(i);
        }
    }
}