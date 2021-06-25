using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    //Player
    public static float attack_damage = 5;
    public static float magic_damage = 8f;
    public static float camMovementSpeed = 1.6f;

    //nombre de archivo de items
    public const string it_no_item = "emptySlot";
    public const string it_healing_1 = "healing1";
    public const string it_mana_1 = "mana1";
    public const string it_healing_2 = "healing2";

    //teclas
    public static KeyCode key_attack = KeyCode.Mouse0;
    public static KeyCode key_magic = KeyCode.Space;
    public static KeyCode key_inventory = KeyCode.I;
    public static KeyCode key_run = KeyCode.LeftShift;
    public static KeyCode key_dash = KeyCode.R;
    public static KeyCode key_interact = KeyCode.E;
    public static KeyCode key_barrier = KeyCode.B;
    public static KeyCode key_cameraZoom = KeyCode.Mouse1;

    public static KeyCode key_inv1 = KeyCode.Alpha1;
    public static KeyCode key_inv2 = KeyCode.Alpha2;
    public static KeyCode key_inv3 = KeyCode.Alpha3;
    public static KeyCode key_inv4 = KeyCode.Alpha4;
    public static KeyCode key_inv5 = KeyCode.Alpha5;

    //enemigos
    public const int Wolf_HP = 18;
    public const int Seta_HP = 12;
    public const int Doowell_HP = 12;
    public const int Chest_HP = 24;
    public const int Fairy_HP = 12;
    public const int Jiro_HP = 400;

    //daño ataques enemigos
    public const int Wolf_Dmg = 6;
    public const int Seta_Dmg = 4;
    public const int Doowell_Dmg = 8;
    public const int Chest_Dmg = 6;
    public const int Fairy_Dmg = 4;
    public const int Jiro_Dmg = 16;

    //experiencia enemigos
    public const int Wolf_Exp = 25;
    public const int Seta_Exp = 20;
    public const int Doowell_Exp = 100;
    public const int Fairy_Exp = 30;
    public const int Jiro_Exp = 400;

    //misc
    public static int volume = 100;


    public static List<GameObject> instanciedObjects = new List<GameObject>();
}


