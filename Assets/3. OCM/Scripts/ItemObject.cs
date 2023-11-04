using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform canvas;
    public Transform previousParent;
    public RectTransform rect;
    public CanvasGroup canvasGroup;

    public void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
        if(transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
