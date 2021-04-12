using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Slider hpBar;
    public Slider staminaBar;
    public Slider manaBar;

    void Update()
    {
        hpBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getHealth();
        staminaBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStamina();
        manaBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getMana();
    }
}
