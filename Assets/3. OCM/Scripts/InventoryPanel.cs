using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventoryPanel : MonoBehaviour 
{
    public Transform[] inventorySlots = new Transform[9];

    private void Awake()
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            inventorySlots[i] = transform.GetChild(i);
        }
    }
}
