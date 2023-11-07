using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDB : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void ReadCharData(string v, List<Item> items)
    {
        items.Clear();
        List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
        dicList.Clear();
        dicList = CSVReader.Read(v);
        for (int i = 0; i < dicList.Count; i++)
        {
            Item item = new Item();
            item.itemName = dicList[i]["name"].ToString();
            item.itemAttack = int.Parse(dicList[i]["F_Atk"].ToString());
            item.itemAttackSpeed = int.Parse(dicList[i]["BAtk"].ToString());
            item.itemHp = int.Parse(dicList[i]["hp"].ToString());
            item.itemMana = int.Parse(dicList[i]["mana"].ToString());
            item.itemSprite = Resources.Load<Sprite>("RPG icons/512X512/" + dicList[i].ToString());
            items.Add(item);
        }
    }
}
