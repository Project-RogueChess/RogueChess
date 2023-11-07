using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public List<Item> itemsDB = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        itemsDB.Add(new Item("sword1", 0,   10, 0, 0));
        itemsDB.Add(new Item("sword2", 0,   20, 0, 0));
        itemsDB.Add(new Item("stone1", 50,  0,  0, 0));
        itemsDB.Add(new Item("stone2", 100, 0,  0, 0));
        itemsDB.Add(new Item("amulet1",0,   0,  1, 0));
        itemsDB.Add(new Item("amulet1",0,   0,  2, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    

    //public string name;
    //public int hp;
    //public int attack;
    //public int mana;
    //public int attackSpeed;
}
