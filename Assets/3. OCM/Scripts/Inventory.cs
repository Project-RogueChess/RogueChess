using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour , IPointerEnterHandler,IDropHandler, IPointerExitHandler
{
    public Image image;
    public RectTransform rect;

    public void Awake()
    {
       image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    
}
