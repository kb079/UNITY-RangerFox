using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemObject", menuName = "Inventory/Item Object")]
public class ItemObject : ScriptableObject
{
    public int value;
    public Sprite sprite;
    public ItemType type;
    public int maxStack;
}

public enum ItemType
{
    Health,
    Mana,
    AttackBuff
}
