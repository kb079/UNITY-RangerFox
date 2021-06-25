using System.Collections.Generic;
using UnityEngine;

public class PlayerSavingData : MonoBehaviour
{
    private static PlayerSavingData instance;

    public static bool runLoadData;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void savePlayerData()
    {
        Player p = Player.getInstance();
        PlayerStats pStats = PlayerStats.getInstance();

        if (p == null || pStats == null) return;

        //PLAYER MAIN SAVING
        var playerStatusObj = new PlayerStatus();
        playerStatusObj.health = p.getHealth();
        playerStatusObj.mana = (int) p.getMana();
        playerStatusObj.stamina = p.getStamina();

        Save("player", playerStatusObj);

        //

        //INVENTORY SAVING
        if (InventoryObject.getInstance() != null)
        {
            List<InventorySlot> inv = InventoryObject.getInstance().getInventory();

            var inventoryObj = new Inventory();
            if (inv.Count != 0) {
                if (inv.Count > 0)
                {
                    inventoryObj.item0 = inv[0].item;
                }

                if (inv.Count > 1)
                {
                    inventoryObj.item1 = inv[1].item;
                }

                if (inv.Count > 2)
                {
                    inventoryObj.item2 = inv[2].item;
                }

                if (inv.Count > 3)
                {
                    inventoryObj.item3 = inv[3].item;
                }

                if (inv.Count > 4)
                {
                    inventoryObj.item4 = inv[4].item;
                }

                Save("inv", inventoryObj);
            }
        }
        //

        //LEVEL STATS SAVING
        var statsObj = new Stats();
        statsObj.level = pStats.Level;
        statsObj.xp = pStats.XP;
        statsObj.neededXP = pStats.NeededXP;

        statsObj.health = pStats.Health;
        statsObj.mana = pStats.Mana;
        statsObj.vigor = pStats.Vigor;
        statsObj.attackDamage = pStats.AttackDmg;
        statsObj.magicDamage = pStats.MagicDmg;

        Save("stats", statsObj);
        
        PlayerPrefs.Save();   
    }

    public static void loadData()
    {
        Player p = Player.getInstance();
       
        if (p == null ) return;

        //LOAD PLAYERS
        var playerStatusObj = new PlayerStatus();
        Load("player", ref playerStatusObj);

        p.setHealth(playerStatusObj.health);
        p.setMana(playerStatusObj.mana);
        p.setStamina(playerStatusObj.stamina);

        //LOAD INVENTORY
        if (InventoryObject.getInstance() != null)
        {
            var inv = InventoryObject.getInstance();

            var inventoryObj = new Inventory();
            Load("inv", ref inventoryObj);

            if(inventoryObj.item0 != null)
            {
                inv.addItem(inventoryObj.item0, inventoryObj.item0.value);
            }
            if (inventoryObj.item1 != null)
            {
                inv.addItem(inventoryObj.item1, inventoryObj.item1.value);
            }
            if (inventoryObj.item2 != null)
            {
                inv.addItem(inventoryObj.item2, inventoryObj.item2.value);
            }
            if (inventoryObj.item3 != null)
            {
                inv.addItem(inventoryObj.item3, inventoryObj.item3.value);
            }
            if (inventoryObj.item4 != null)
            {
                inv.addItem(inventoryObj.item4, inventoryObj.item4.value);
            }
        }

        //LOAD LEVEL STATS
        var statsObj = new Stats();
        Load("stats", ref statsObj);

        PlayerStats pStats = PlayerStats.getInstance();

        pStats.Level = statsObj.level;
        pStats.XP = statsObj.xp;
        pStats.NeededXP = statsObj.neededXP;

        pStats.Health = statsObj.health;
        pStats.Mana = statsObj.mana;
        pStats.Vigor = statsObj.vigor;
        pStats.AttackDmg = statsObj.attackDamage;
        pStats.MagicDmg = statsObj.magicDamage;

        GameConstants.attack_damage = statsObj.attackDamage;
        GameConstants.magic_damage = statsObj.magicDamage;

        pStats.updateUI();
    }

    private static void Save(string keyname, object data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(keyname, json);
    }

    private static void Load<T>(string keyname, ref T data)
    {
        string json = PlayerPrefs.GetString(keyname, "{}");
        JsonUtility.FromJsonOverwrite(json, data);
    }

    public static bool checkExistsData()
    {
        var playerStatusObj = new PlayerStatus();
        Load("player", ref playerStatusObj);

        if(playerStatusObj.health != 0)
        {
            return true;
        }

        return false;
    }

    public static void deleteData()
    {
        PlayerPrefs.DeleteAll();
    }

}

public class Stats
{
    public int level;
    public int xp;
    public int neededXP;

    public int health;
    public int mana;
    public int vigor;
    public float attackDamage;
    public float magicDamage;
}

public class Inventory
{
    public ItemObject item0;
    public ItemObject item1;
    public ItemObject item2;
    public ItemObject item3;
    public ItemObject item4;
}

public class PlayerStatus
{
    public int health;
    public float mana;
    public float stamina;
}