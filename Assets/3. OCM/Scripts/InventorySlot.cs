using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour , IPointerEnterHandler,IDropHandler, IPointerExitHandler
{
    public Image image;
    public RectTransform rect;
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
        if (eventData.pointerDrag != null && isSlotEmpty)
        {
            //eventData.pointerDrag.transform.SetParent(transform);
            //eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            itemGO.GetComponent<ItemObject>()._item = eventData.pointerDrag.GetComponent<ItemObject>()._item;
            eventData.pointerDrag.GetComponent<ItemObject>()._item = new Item();
            isSlotEmpty = false;
        }
    }

    public void OnItemDraggedOut()
    {
        isSlotEmpty = true;
    }
}
