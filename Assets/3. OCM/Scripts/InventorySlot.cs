using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour , IPointerEnterHandler,IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;
    public bool isSlotEmpty = true;
    public GameObject itemGO;
    
    public void Awake()
    {
       image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
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
