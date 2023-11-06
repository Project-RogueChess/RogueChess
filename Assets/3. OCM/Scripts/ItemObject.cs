using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot[] inventorySlots;
    public InventorySlot currentSlot;
    public Transform[] inventorySlotsTrans;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public int num = 0;


    

    
    private void Awake()
    {
        inventorySlots = FindObjectsOfType<InventorySlot>();
        inventorySlotsTrans = new Transform[inventorySlots.Length];
        foreach(InventorySlot inventory in inventorySlots)
        {
            Transform transform = inventory.transform;
            inventorySlotsTrans[num] = transform;
            num++; 
        }
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;
        currentSlot = transform.parent.GetComponent<InventorySlot>();
        if(currentSlot != null)
        {
            currentSlot.OnItemDraggedOut();
        }
        previousParent = transform.parent;

        transform.SetParent(transform.parent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == previousParent)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
