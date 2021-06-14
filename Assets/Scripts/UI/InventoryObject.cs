using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObject : MonoBehaviour
{
    [SerializeField] GameObject[] UISlots;
    [SerializeField] List<ItemObject> itemTypes;
    [SerializeField] Player player;
    [SerializeField] Sprite nullSprite;
    private List<Image> images = new List<Image>();
    private List<Text> texts = new List<Text>();
    public List<InventorySlot> container = new List<InventorySlot>();

    private void Awake()
    {
        int i = 0;
        foreach (GameObject go in UISlots)
        {
            images.Add(go.GetComponent<Image>());
            texts.Add(go.GetComponentInChildren<Text>());
        }
    }
    private void Update()
    {
        //**********
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            addItem(itemTypes[0], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            addItem(itemTypes[1], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            addItem(itemTypes[2], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            addItem(itemTypes[3], 7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            addItem(itemTypes[4], 4);
        }
        // DEBUG
        //**********


        if (Input.GetKeyDown(GameConstants.key_inv1))
        {
            useItem(0);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv2))
        {
            useItem(1);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv3))
        {
            useItem(2);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv4))
        {
            useItem(3);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv5))
        {
            useItem(4);
        }
    }
    public void addItem(ItemObject item, int stack)
    {
        // Compruebo si el item ya estaba dentro del inventario
        for(int i = 0; i < container.Count; i++)
        {
            InventorySlot io = container[i];
            // Si es así:
            if (io.item.Equals(item))
            {
                // Si me paso del stack máximo, lo dejo en el stack máximo
                int nStack = item.maxStack;
                if (io.stack + stack < nStack)
                {
                    io.addStack(stack);
                } else
                {
                    io.setStack(nStack);
                }
                updateStackCount(i);
                return;
            }
        }
        // Si no estaba, hago la misma comprobación del stack y añado el item
        if(stack >= item.maxStack)
        {
            stack = item.maxStack;
        }
        // Lo añado gráficamente (sprite + 'x' stack)
        addInfoToInventory(container.Count, item.sprite, stack);
        container.Add(new InventorySlot(item, stack, container.Count));
    }

    // Info gráfica (sprite + 'x' stack)
    private void addInfoToInventory(int position, Sprite sprite, int stack)
    {
        images[position].sprite = sprite;
        texts[position].text = "x" + stack;
    }

    // Info gráfica ('x' stack)
    private void updateStackCount(int position)
    {
        texts[position].text = "x" + container[position].stack;
    }

    // Borro info gráfica actual (sprite + 'x' stack)
    private void clearGraphicSlot(int i)
    {
        images[i].sprite = nullSprite;
        texts[i].text = "";
    }

    private void useItem(int position)
    {
        // Si estoy intentando usar un slot vacío, no hago nada
        if (position >= container.Count) return;

        InventorySlot slot = container[position];
        ItemObject item = slot.item;
        // Realizo la acción según qué tipo de item sea el usado
        switch (item.type)
        {
            case ItemType.Health:
                player.addHealth(item.value);
                break;
            case ItemType.Mana:
                player.addMana(item.value);
                break;
            default:
                break;
        }

        slot.reduceStack();

        // Si tras usarlo el stack es = 0, lo borro
        if(slot.stack <= 0)
        {
            container.RemoveAt(position);
            int contCount = container.Count;

            // Muevo los items que estaban a su derecha a la izquierda
            for (int i = position; i < contCount; i++)
            {
                InventorySlot inv = container[i];
                addInfoToInventory(i, inv.item.sprite, inv.stack);
            }

            // Vacío los slots que resten (que ahora no tienen nada)
            for(int i = contCount; i < images.Count; i++)
            {
                clearGraphicSlot(i);
            }
        } else
        {
            // Actualizo su info gráfica (cantidad de stack)
            updateStackCount(position);
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int stack;
    public int position;
    public InventorySlot(ItemObject item, int stack, int position)
    {
        this.item = item;
        this.stack = stack;
        this.position = position;
    }

    public void addStack(int value)
    {
        stack += value;
    }
    public void reduceStack()
    {
        stack -= 1;
    }
    public void setStack(int value)
    {
        stack = value;
    }
}