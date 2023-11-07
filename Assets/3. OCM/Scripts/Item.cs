using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public Sprite itemSprite;
    public string itemName;
    public int itemHp;
    public int itemAttack;
    public int itemAttackSpeed;
    public int itemMana;

    public Item(string name, int hp, int attack, int attackSpeed, int mana)
    {
        this.itemName = name;
        this.itemHp = hp;
        this.itemAttack = attack;
        this.itemAttackSpeed = attackSpeed;
        this.itemMana = mana;
    }

    public Item()
    {

    }
}
