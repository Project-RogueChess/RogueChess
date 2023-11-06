using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanel : MonoBehaviour , IDropHandler
{
    public RectTransform invPanel;

    private void Awake()
    {
        invPanel = GetComponent<RectTransform>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        
        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            Debug.Log("??");
        }
        
    }
}
