using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour , IPointerEnterHandler,IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;
    public bool isSlotEmpty = true;
    public GameObject itemGO;
    public ToolTip tooltip;
    public ItemObject itemObject;

    public void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        itemObject = transform.GetChild(0).GetComponent<ItemObject>();

        tooltip.gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
        if (itemObject._item.itemName != string.Empty && itemObject._item.itemName !=null)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.SetupItemToolTip(itemObject._item.itemSprite,itemObject._item.itemName, itemObject._item.itemHp, itemObject._item.itemAttackDamage, itemObject._item.itemAttackSpeed);
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
        tooltip.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if ( eventData.pointerDrag != null && isSlotEmpty == true)
        {
            if (eventData.pointerDrag.GetComponent<ItemObject>()._item == itemGO.GetComponent<ItemObject>()._item)
            {
                itemGO.GetComponent<ItemObject>()._item = eventData.pointerDrag.GetComponent<ItemObject>()._item;
            }
            else 
            {
                itemGO.GetComponent<ItemObject>()._item = eventData.pointerDrag.GetComponent<ItemObject>()._item;
                eventData.pointerDrag.GetComponent<ItemObject>()._item = new Item();
            }
            isSlotEmpty = false;
        }
        else if(eventData.pointerDrag != null && isSlotEmpty == false)
        {
            //GameObject temp;
            //temp = itemGO;
            Item tempItem = itemGO.GetComponent<ItemObject>()._item;
            itemGO.GetComponent<ItemObject>()._item = eventData.pointerDrag.GetComponent<ItemObject>()._item;
            eventData.pointerDrag.GetComponent<ItemObject>()._item = tempItem;
            isSlotEmpty = false;
        }
    }
    
    public void OnItemDraggedOut()
    {
        isSlotEmpty = true;
    }
}
