using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyItem
{
    private string sprName;
    private int value, stack, maxStack;
    private Sprite sprite;
    private int id, type;
    private bool isInInventory;

    public MyItem(int id, int type, string sprName, int value, int stack, int maxStack, bool isInInventory)
    {
        this.isInInventory = isInInventory;
        this.id = id;
        this.type = type;
        this.stack = stack;
        this.sprName = sprName;
        this.value = value;
        this.maxStack = maxStack;
        sprite = Resources.Load<Sprite>("Sprites/Items/" + sprName);
    }
    public string SprName { get => sprName; set => sprName = value; }
    public int Value { get => value; set => this.value = value; }
    public int Type { get => type; set => type = value; }
    public int Stack { get => stack; set => stack = value; }
    public int Id { get => id; set => id = value; }
    public int MaxStack { get => maxStack; set => maxStack = value; }
    public bool IsInInventory { get => isInInventory; set => isInInventory = value; }
}
