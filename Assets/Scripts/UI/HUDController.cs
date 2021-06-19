using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image staminaBar;

    private int maxHealth;
    private float maxMana;
    private float maxStamina;

    public Text hudText;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxHealth = player.getHealth();
        maxMana = player.getMana();
        maxStamina = player.getStamina();
    }
    //  Para la versión final, evitar usar el update
    void Update()
    {
        float hp = (float)player.getHealth() / (float)maxHealth;
        float mp = player.getMana() / maxMana;
        float sp = player.getStamina() / maxStamina;
        hpBar.fillAmount = hp;
        mpBar.fillAmount = mp;
        staminaBar.fillAmount = sp;
        hudText.text = player.getHudText();
    }
    
}
