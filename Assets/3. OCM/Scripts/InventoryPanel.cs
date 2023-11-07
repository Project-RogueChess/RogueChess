using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventoryPanel : MonoBehaviour 
{
    public Transform[] inventorySlots = new Transform[9];
    public ItemDB itemDB;
    private void Awake()
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            inventorySlots[i] = transform.GetChild(i);
        }
        itemDB = GetComponent<ItemDB>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddItem();
        }
    }
    public void AddItem()
    {
        for (int i =0;i< transform.childCount; i++)
        {
            if (inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventorySlots[i].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item  = itemDB.itemsDB[i];
                return;
            }
        }
        
    }
}
