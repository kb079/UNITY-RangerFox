using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObject : MonoBehaviour
{
    private static InventoryObject instance;

    [SerializeField] GameObject[] UISlots;
    public List<ItemObject> itemTypes;
    [SerializeField] Player player;
    [SerializeField] Sprite nullSprite;
    private List<Image> images = new List<Image>();
    private List<Text> texts = new List<Text>();
    public List<InventorySlot> container = new List<InventorySlot>();
    public AudioClip used;

    public static InventoryObject getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        int i = 0;
        foreach (GameObject go in UISlots)
        {
            images.Add(go.GetComponent<Image>());
            texts.Add(go.GetComponentInChildren<Text>());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(GameConstants.key_inv1))
        {
            GetComponent<AudioSource>().PlayOneShot(used);
            useItem(0);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv2))
        {
            GetComponent<AudioSource>().PlayOneShot(used);
            useItem(1);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv3))
        {
            GetComponent<AudioSource>().PlayOneShot(used);
            useItem(2);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv4))
        {
            GetComponent<AudioSource>().PlayOneShot(used);
            useItem(3);
        }
        else if (Input.GetKeyDown(GameConstants.key_inv5))
        {
            GetComponent<AudioSource>().PlayOneShot(used);
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

    // Dropear item al matar enemigo
    public void OnEnemyDead(int dropProbabilitySuccess, int dropProbabilityMax, Vector3 position)
    {
        int result = Random.Range(0, dropProbabilityMax);
        if (result < dropProbabilitySuccess)
        {
            Dictionary<ItemObject, int> items = new Dictionary<ItemObject, int>();
            foreach (ItemObject it in itemTypes)
            {
                items.Add(it, it.dropWeight);
            }
            ItemObject item = ExtRandom.ChooseWeighted(items);
            Vector3 nPos = new Vector3(position.x, position.y + 2, position.z);
            GameObject itemGO = Instantiate(item.itemGameObject, position, item.itemGameObject.transform.rotation, transform);
            itemGO.SetActive(true);
        }
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
                Debug.Log(">" + container.Count);
                Debug.Log("<" + container[position].ToString());
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

    public List<InventorySlot> getInventory()
    {
        return container;
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