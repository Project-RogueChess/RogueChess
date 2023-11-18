using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip tooltip;

    void Awake()
    {
        tooltip = GetComponent<ToolTip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemObject item = GetComponent<ItemObject>();

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
