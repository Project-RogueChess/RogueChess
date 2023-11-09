using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventoryPanel : MonoBehaviour 
{
    public Transform[] inventorySlots = new Transform[9];
    public ItemsDB itemsDB;
    private void Awake()
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            inventorySlots[i] = transform.GetChild(i);
        }
        itemsDB = GetComponent<ItemsDB>();
    }

    //public void AddItem()
    //{
    //    for (int i =0;i< transform.childCount; i++)
    //    {
    //        if (inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty)
    //        {
    //            inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item  = itemsDB.items[i];
    //            inventorySlots[i].GetComponent<InventorySlot>().isSlotEmpty = false;
    //            return;
    //        }
    //    }
    //}

    //public void AddRandomItem()
    //{
    //    int j =  Random.Range(0, itemsDB.items.Count);
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        if (inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
    //            inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
    //        {
    //            inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = itemsDB.items[j];
    //            inventorySlots[i].GetComponent<InventorySlot>().isSlotEmpty = false;
    //            return;
    //        }
    //    }
    //}
}
