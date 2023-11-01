using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{
    private Transform canvas;
    private Transform previousParent;
    
    private bool isOnDrag;

    Vector3 backObject;

    private void Update()
    {
       if(isOnDrag)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
        }

    }

    public void OnMouseDown()
    {
        isOnDrag = true;

        backObject = transform.position;
    }

    public void OnMouseUp()
    {
        isOnDrag = false;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.position = hit.transform.position;
            Debug.Log("oo");
        }
        else
        {
            transform.position = backObject;
        }
    }


}
