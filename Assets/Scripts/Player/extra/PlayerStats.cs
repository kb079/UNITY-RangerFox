using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats
{
    private static PlayerStats instance;

    public Image xpBar;
    public GameObject xpBarLevelUP;

    private int level;
    private int xp;
    private int neededXP;

    private int health;
    private int mana;
    private int vigor;
    private float attackDamage;
    private float magicDamage;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public int XP
    {
        get { return xp; }
        set { xp = value; }
    }

    public int NeededXP
    {
        get { return neededXP; }
        set { neededXP = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    public int Vigor
    {
        get { return vigor; }
        set { vigor = value; }
    }

    public float AttackDmg
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    public float MagicDmg
    {
        get { return magicDamage; }
        set { magicDamage = value; }
    }

    public static PlayerStats getInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
        level = 1;
        health = 0;
        mana = 0;
        vigor = 0;
        attackDamage = 1;
        magicDamage = 1;
    }
     
    public void addXP(int amount)
    {
        xp += amount;
        
        if(xp >= neededXP)
        {
            levelUp();
        }

        if(xp > neededXP)
        {
            xp = neededXP - xp;
        }
        else
        {
            xp = 0;
        }

        xpBar.fillAmount = xp / 100;
    }

    private void levelUp()
    {
        xpBarLevelUP.SetActive(true);
        //TODO: ADD SOUND
        //TODO: ADD EXPLOSION
        //TODO: ADD MESSAGE ON SCREEN WHEN LEVEL UP

        StartCoroutine(endLevelUPAnimation(2.5f));
    }
    private void updateStats()
    {

    }

    IEnumerator endLevelUPAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        xpBarLevelUP.SetActive(false);

    }
}
