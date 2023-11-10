using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ItemsDB : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    
    private void Awake()
    {
        ReadCharData("itemDB", items);
        
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
            item.id = int.Parse(dicList[i]["id"].ToString());
            item.itemHp = int.Parse(dicList[i]["hp"].ToString());
            item.itemAttackDamage = int.Parse(dicList[i]["itemAttackDamage"].ToString());
            item.itemAttackSpeed = float.Parse(dicList[i]["itemAttackSpeed"].ToString());
            item.itemMp = int.Parse(dicList[i]["mp"].ToString());
            item.itemSprite = Resources.Load<Sprite>("RPG icons/512X512/" + dicList[i]["name"].ToString());
            items.Add(item);
        }
    }

   



}
