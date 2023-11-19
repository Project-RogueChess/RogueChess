using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ToolTip itemToolTip;

    public ItemsDB itemDB;
    public int itemObjectNum;
    void Awake()
    {
        //itemDB = FindObjectOfType<ItemsDB>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        itemToolTip.gameObject.transform.position = new Vector3(1710, 810, 0);
        itemToolTip.SetupItemToolTip(itemDB.items[itemObjectNum].itemSprite, itemDB.items[itemObjectNum].itemName, itemDB.items[itemObjectNum].itemHp, itemDB.items[itemObjectNum].itemAttackDamage, itemDB.items[itemObjectNum].itemAttackSpeed);
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemToolTip.gameObject.transform.position = new Vector3(2300, 400, 0);
    }
}
