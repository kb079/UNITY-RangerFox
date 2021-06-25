using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;

    private Image xpBar;
    public Image xpLevelUPImg;

    public Text levelText, healthText, manaText, vigorText, attackDmgText, magicDmgText;

    private int maxLevel = 5;

    private int level = 1;
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
        DontDestroyOnLoad(this);

        instance = this;

        xpBar = GameObject.FindGameObjectWithTag("xpBar").GetComponent<Image>();
  
        attackDamage = GameConstants.attack_damage;
        magicDamage = GameConstants.magic_damage;

        attackDmgText.text = attackDamage + "";
        magicDmgText.text = magicDamage + "";
        neededXP = 50;

        health = 100;
        mana = 100;
        vigor = 100;
    }

    public void addXP(int amount)
    {
        xp += amount;
        updateXPBar();

        if (xp >= neededXP)
        {
            levelUp();
            if (xp > neededXP)
            {
                xp -= neededXP;                
            }
            else
            {
                xp = 0;
            }   
        }
    }

    private void updateXPBar()
    {
        xpBar.fillAmount = (float)xp / 100;
    }

    private void levelUp()
    {
        level++;
        if (level == maxLevel) return;

        StartCoroutine(FadeImage(false));
        //TODO: ADD SOUND
        //TODO: ADD EXPLOSION

        neededXP *= 2;
        updateStats();
        StartCoroutine(endLevelUPAnimation(3f));
    }
    private void updateStats()
    { 
        health += 25;
        float manaCalc = mana * 1.2f;
        mana = (int) manaCalc;
        vigor += 10;
        attackDamage *= 1.2f;
        magicDamage *= 1.4f;
        updateUI();

        Player.getInstance().maxHealth = health;
        Player.getInstance().addHealth(25);

        Player.getInstance().maxMana = mana;
        Player.getInstance().addMana(25);

        Player.getInstance().maxStamina = vigor;
        Player.getInstance().addStamina(25);
    }

    private void updateUI()
    {
        levelText.text = level + "";
        healthText.text = health + "";
        manaText.text = mana + "";
        vigorText.text = vigor + "";
        attackDmgText.text = attackDamage + "";
        magicDmgText.text = magicDamage + "";
    }

    IEnumerator endLevelUPAnimation(float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(FadeImage(true));
        updateXPBar();
    }


    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                xpLevelUPImg.color = new Color(1, 1, 1, i);
                yield return null;
            }
            xpLevelUPImg.gameObject.SetActive(false);
        }
        // fade from transparent to opaque
        else
        {
            xpLevelUPImg.gameObject.SetActive(true);
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                xpLevelUPImg.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}