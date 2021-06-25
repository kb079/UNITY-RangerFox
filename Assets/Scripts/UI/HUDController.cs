using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image staminaBar;

    public Text hudText;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    //  Para la versión final, evitar usar el update
    void Update()
    {
        float hp = (float)player.getHealth() / player.maxHealth;
        float mp = player.getMana() / player.maxMana;
        float sp = player.getStamina() / player.maxStamina;

        hpBar.fillAmount = hp;
        mpBar.fillAmount = mp;
        staminaBar.fillAmount = sp;
        hudText.text = player.getHudText();
    }
    
}
