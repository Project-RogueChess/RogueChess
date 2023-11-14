using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.Progress;



//일단 상점 구현 할 때 써야할 필요 기물 정보

public class Pieces : MonoBehaviour
{
    public Sprite pieceImg;
    public new string name;
    public int id;
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

    public Canvas canvas;
    public HpBar hpbarScript;

    public int t_objectsNum;
    public ItemsImg itemImage;

    BoxCollider boxCollider;

    public PiecesCountManager piecesCountManager;
    public Sprite[] pieceGradeImgs;
    public void Parse(Piece piece)
    {
        pieceImg = piece.pieceImg;
        id = piece.id;
        gold = piece.gold;
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


        canvas = GetComponentInChildren<Canvas>();

        boxCollider = GetComponent<BoxCollider>();

        pieceGradeImgs = FindObjectOfType<PiecesCountManager>().GetComponent<PiecesCountManager>().piecesGradeImg;

        
    }
    public void EquipItem(ItemObject item)
    {
        for (int i=0; i < items.Length;i++)
        {
            if (items[i].itemName == string.Empty || items[i].itemName == null)
            {
                items[i] = item._item;
                maxHp += item._item.itemHp;
                hp += item._item.itemHp;
                attackDamage += item._item.itemAttackDamage;
                attackSpeed += item._item.itemAttackSpeed;
                mp += item._item.itemMp;
                for (int j = 0; j < items.Length; j++)
                {
                    if (i == j)
                    {
                        GameObject itemImg = canvas.gameObject.transform.GetChild(i).gameObject;
                        itemImg.SetActive(true);
                        itemImg.GetComponent<Image>().sprite = item._item.itemSprite;
                        return;
                    }
                }
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


    public void OnBoxCollider()
    {
        boxCollider.enabled = true;
    }


    public void SellPiece()
    {
        DataManager.instance.myGold += CalculateGold();
        UIManager.instance.UIRefresh();
        GivingItemInfo();
       
        Destroy(gameObject);
    }

    public int CalculateGold()
    {
        if(grade == 1)
        {
            return gold = gold;
        }
        else 
        {
            return gold = gold+2*(grade-1);
        }
    }


    public void MergePeice()
    {
        if (grade == 2)
        {
            Debug.Log(2);
            maxHp = 2 * maxHp;
            hp = 2 * hp;
            attackDamage = 2 * attackDamage;
            canvas.gameObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = pieceGradeImgs[1];
        }
        else if (grade == 3)
        {
            Debug.Log(3);
            maxHp += 3 * maxHp;
            hp = 3 * hp;
            attackDamage = 3 * attackDamage;
            canvas.gameObject.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = pieceGradeImgs[2];
        }
    }

}
