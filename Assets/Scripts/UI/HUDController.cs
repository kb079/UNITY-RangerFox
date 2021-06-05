using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Slider hpBar;
    public Slider staminaBar;
    public Slider manaBar;
    public Text hudText;
    private Player player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void Update()
    {
        hpBar.value = player.getHealth();
        staminaBar.value = player.getStamina();
        manaBar.value = player.getMana();
        hudText.text = player.getHudText();
    }
}
