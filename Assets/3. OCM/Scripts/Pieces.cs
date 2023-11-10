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
    
    public int gold;
    public string spieces;
    public string classes;
    public int grade;
    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;

    public Transform pos;
    public Item[] items;


    public void Parse(Piece piece)
    {
        pieceImg = piece.pieceImg;
        name = piece.name;
        grade = piece.grade;
        maxHp = piece.maxHp;
        hp = piece.hp;
        maxMp = piece.maxMp;
        mp = piece.mp;
        attackDamage = piece.attackDamage;
        attackSpeed = piece.attackSpeed;
        attackRange = piece.attackRange;
        moveSpeed = piece.moveSpeed;
    }
    private void Awake()
    {
        pos = GetComponent<Transform>();
        items = new Item[3];

        for(int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(string.Empty,0,0,0,0,0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GivingItemInfo();
            Destroy(gameObject);
            
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
                attackDamage += item._item.itemAttackDamage;
                attackSpeed += item._item.itemAttackSpeed;
                mp += item._item.itemMp;
                return;
            }
        }
    }

    public void GivingItemInfo()
    {
        for(int i=0;i < items.Length; i++)
        {
            if (items[i].itemName != string.Empty)
            {
                UIManager.instance.AddTheItem(items[i]);
            }
        }
        
    }
}
