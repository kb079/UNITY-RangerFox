using UnityEngine;

[CreateAssetMenu(fileName = "New ItemObject", menuName = "Inventory/Item Object")]
public class ItemObject : ScriptableObject
{
    public int value;
    public Sprite sprite;
    public ItemType type;
    public int maxStack;
    public int dropWeight;
    public GameObject itemGameObject;
}

public enum ItemType
{
    Health,
    Mana,
    AttackBuff
}
