using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//일단 상점 구현 할 때 써야할 필요 기물 정보

public class Pieces : MonoBehaviour
{
    public Sprite piecesImg;
    public string name;
    public string synergy;
    public int gold;


    public Transform pos;
    public Item[] items;

    private void Awake()
    {
        pos = GetComponent<Transform>();
        items = new Item[3];
    }

    private void Start()
    {
        
    }


    public void EquipItem(ItemObject item)
    {
        for (int i=0; i < items.Length;i++)
        {
            if (items[i] == null)
            {
                items[i] = item._item;
                return;
            }
        }
    }
  
}
