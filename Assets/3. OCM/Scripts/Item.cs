using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public Sprite itemSprite;
    public string itemName;
    public int id;
    public int itemHp;
    public int itemAttackDamage;
    public float itemAttackSpeed;
    public int itemMp;

    public Item(string name,int id, int hp, int itemAttackDamage, float itemAttackSpeed, int mana)
    {
        this.id = id;
        this.itemName = name;
        this.itemHp = hp;
        this.itemAttackDamage = itemAttackDamage;
        this.itemAttackSpeed = itemAttackSpeed;
        this.itemMp = mana;
    }

    public Item()
    {

    }
}
