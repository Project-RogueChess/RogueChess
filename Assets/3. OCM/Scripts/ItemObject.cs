using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public InventorySlot[] inventorySlots;
    public InventorySlot currentSlot;
    public Transform[] inventorySlotsTrans;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public int num = 0;
    
    public Item _item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
            SwitchImage();
        }
    }

    [SerializeField] private Item item;

    

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
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        OnCollider();
        if (item.itemName != null && item.itemName != string.Empty)
        {
            previousParent = transform.parent;
            currentSlot = transform.parent.GetComponent<InventorySlot>();
            if (currentSlot != null)
            {
                currentSlot.OnItemDraggedOut();
            }
            previousParent = transform.parent;

            transform.SetParent(transform.parent);
            transform.SetAsLastSibling();

            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(item.itemName != null && item.itemName != string.Empty)
        {
            rect.position = eventData.position;
            transform.position = Input.mousePosition;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            // 조건에 맞는 오브젝트인지 확인
            
            Pieces equipableItem = hitInfo.transform.GetComponent<Pieces>();
            if (equipableItem != null)
            {
                if (equipableItem.items[2].itemName != string.Empty)
                {

                }
                else
                {
                    equipableItem.EquipItem(this);
                    _item = new Item();
                }
            }
        }

        if (transform.parent == previousParent)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        OffCollider();
    }

    void SwitchImage()
    {
        if(item.itemSprite != null)
        {
            image.sprite = item.itemSprite;
        }
        else
        {
            image.sprite = null;
        }
    }


    public void OnCollider()
    {
        for (int i = 0; i < InvSpawnManager.instance.hexaTiles.Count; i++)
        {
            if (InvSpawnManager.instance.hexaTiles[i].piece != null)
            {
                InvSpawnManager.instance.hexaTiles[i].piece.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {
                InvSpawnManager.instance.invTiles[i].piece.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }
    public void OffCollider()
    {
        for (int i = 0; i < InvSpawnManager.instance.hexaTiles.Count; i++)
        {
            if (InvSpawnManager.instance.hexaTiles[i].piece != null)
            {
                InvSpawnManager.instance.hexaTiles[i].piece.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {
                InvSpawnManager.instance.invTiles[i].piece.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }
}
