using UnityEngine;

public class GameConstants : MonoBehaviour
{
    //nombre de archivo de items
    public const string it_no_item = "emptySlot";
    public const string it_healing_1 = "healing1";
    public const string it_mana_1 = "mana1";
    public const string it_healing_2 = "healing2";

    //teclas
    public const KeyCode key_inventory = KeyCode.I;
    public const KeyCode key_run = KeyCode.Space;
    public const KeyCode key_dash = KeyCode.R;
    public const KeyCode key_interact = KeyCode.E;
    public const KeyCode key_barrier = KeyCode.B;

    //enemigos
    public const uint Wolf_HP = 20;
    public const uint Seta_HP = 20;
    public const uint Doowell_HP = 20;
    public const uint Chest_HP = 20;
    public const uint Fairy_HP = 20;
}
