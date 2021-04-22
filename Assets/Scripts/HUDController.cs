using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Slider hpBar;
    public Slider staminaBar;
    public Slider manaBar;
    public Text hudText;
    private Player player;
    //Debug---------------------------
    private Inventory inventory;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug---------------------------
        inventory = GetComponent<Inventory>();
    }
    void Update()
    {
        hpBar.value = player.getHealth();
        staminaBar.value = player.getStamina();
        manaBar.value = player.getMana();
        hudText.text = player.getHudText();
        //Debug---------------------------
        if (Input.GetKeyDown(KeyCode.H)) inventory.addToInventory(GameConstants.it_healing_1);
        if (Input.GetKeyDown(KeyCode.J)) inventory.addToInventory(GameConstants.it_mana_1);
        //Debug---------------------------
    }
}
