using System.Collections;
using System.Collections.Generic;
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
    public void Parse(Piece piece)
    {
        pieceImg = piece.pieceImg;
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
        canvas = FindObjectOfType<Canvas>();
        hpbarScript = FindObjectOfType<HpBar>().GetComponent<HpBar>();
        itemImage = FindObjectOfType<ItemsImg>();
        for ( int i = 0;i<hpbarScript.t_objects.Length;i++)
        {
            if (!hpbarScript.t_objects[i].GetComponent<Pieces>())
            {
                hpbarScript.t_objects[i] = gameObject;
                t_objectsNum = i;
                hpbarScript.m_hpBarsList[i].SetActive(true);
                hpbarScript.m_ItemsList[i*3+0].SetActive(true);
                hpbarScript.m_ItemsList[i*3+1].SetActive(true);
                hpbarScript.m_ItemsList[i*3+2].SetActive(true);
                return;
            }
        }

        //hpbarScript.m_objectList.Add(gameObject.transform);
        //hpbarScript.t_HpBar = Instantiate(hpbarScript.m_goPrefab, gameObject.transform.position, Quaternion.identity, canvas.transform);
        //hpbarScript.m_hpBarsList.Add(hpbarScript.t_HpBar);
        boxCollider = GetComponent<BoxCollider>();
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
                mp += item._item.itemMp ;
                if (i==0)
                {
                    itemImage.transform.GetChild(t_objectsNum*3).gameObject.GetComponent<Image>().sprite = item._item.itemSprite;
                    Color color = itemImage.transform.GetChild(t_objectsNum * 3).gameObject.GetComponent<Image>().color;
                    color.a = 1;
                    itemImage.transform.GetChild(t_objectsNum * 3).gameObject.GetComponent<Image>().color = color;
                }
                else if (i==1)
                {
                    itemImage.transform.GetChild(t_objectsNum * 3 + 1).gameObject.GetComponent<Image>().sprite = item._item.itemSprite;
                    Color color = itemImage.transform.GetChild(t_objectsNum * 3+1).gameObject.GetComponent<Image>().color;
                    color.a = 1;
                    itemImage.transform.GetChild(t_objectsNum * 3+1).gameObject.GetComponent<Image>().color = color;
                }
                else
                {
                    itemImage.transform.GetChild(t_objectsNum * 3+2).gameObject.GetComponent<Image>().sprite = item._item.itemSprite;
                    Color color = itemImage.transform.GetChild(t_objectsNum * 3+2).gameObject.GetComponent<Image>().color;
                    color.a = 1;
                    itemImage.transform.GetChild(t_objectsNum * 3+2).gameObject.GetComponent<Image>().color = color;
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
        hpbarScript.t_objects[t_objectsNum] = hpbarScript.t_HpBar;
        hpbarScript.m_hpBarsList[t_objectsNum].SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            itemImage.transform.GetChild(t_objectsNum * 3 + i).gameObject.GetComponent<Image>().sprite = null;
            Color color = itemImage.transform.GetChild(t_objectsNum * 3 + i).gameObject.GetComponent<Image>().color;
            color.a = 0;
            itemImage.transform.GetChild(t_objectsNum * 3 + i).gameObject.GetComponent<Image>().color = color;
            hpbarScript.m_ItemsList[t_objectsNum * 3 + i].SetActive(false);
        }
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
            return gold = gold+2*grade;
        }
    }
}
