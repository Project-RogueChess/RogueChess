using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
        
public class InputController : MonoBehaviour
{
    public DragController dragController;

    public Map map;
    public TilemapManager tilemapManager;

    public LayerMask triggerLayer;

    [HideInInspector]
    public TilemapTriggerInfo triggerInfo = null;

    private Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        triggerInfo = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, triggerLayer, QueryTriggerInteraction.Collide))
        {
            //get trigger info of the  hited object
            triggerInfo = hit.collider.gameObject.GetComponent<TilemapTriggerInfo>();

            //this is a trigger
            if (triggerInfo != null)
            {
                //get indicator
                GameObject indicator = map.GetIndicatorFromTriggerInfo(triggerInfo);

                //set indicator color to active
                indicator.GetComponent<MeshRenderer>().material.color = tilemapManager.hexa_activeColor;
            }
            else
                map.resetIndicators(); //reset colors
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragController.StartDrag();
        }

        if(Input.GetMouseButtonUp(0))
        {
            dragController.StopDrag();
        }

        mousePosition = Input.mousePosition;
    }
    

}
