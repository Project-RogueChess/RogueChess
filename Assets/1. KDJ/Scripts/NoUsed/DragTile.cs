using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTile : MonoBehaviour
{

    public GameObject otherGO;

    DragObject dragObject;

    private void Start()
    {
        dragObject = GetComponent<DragObject>();
    }

    private void OnMouseOver()
    {
        if (dragObject.IsOnDrag)
        {
            DragObject.checkPosition = true;
            
            DragObject.GO = this.gameObject;
        }
    }
    private void OnMouseExit()
    {
        DragObject.checkPosition = false;

        
    }

}
