using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//일단 상점 구현 할 때 써야할 필요 기물 정보

public class Pieces : MonoBehaviour
{
    public Sprite pieceImg;
    public new string name;
    public string synergy;
    public int gold;


    public Transform pos;
    public Item[] items;

    public int tier;

    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int attack;
    public int attackSpeed;
    
    
    private void Awake()
    {
        pos = GetComponent<Transform>();
        items = new Item[3];

        for(int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(string.Empty,0,0,0,0);
        }
    }


    public void EquipItem(ItemObject item)
    {

        for (int i=0; i < items.Length;i++)
        {
            
            if (items[i].itemName == string.Empty || items[i].itemName == null)
            {
                items[i] = item._item;
                hp += item._item.itemHp;
                attack += item._item.itemAttack;
                attackSpeed += item._item.itemAttackSpeed;
                mp += item._item.itemMp;
                return;
            }
        }
    }

    


}
