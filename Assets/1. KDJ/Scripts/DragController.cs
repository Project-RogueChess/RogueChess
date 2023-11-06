using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStage { Preparation, Combat, Loss };
public class DragController : MonoBehaviour
{

    public InputController inputController;
    public GameStage currentGameStage;
    
    private TriggerInfo dragStartTrigger = null;
    private GameObject draggedUnit = null;

    [HideInInspector]
    public GameObject[] ownChampionInventoryArray;
    [HideInInspector]
    public GameObject[,] gridChampionsArray;

    /// <summary>
    /// When we start dragging champions on map
    /// </summary>
    public void StartDrag()
    {
        if (currentGameStage != GameStage.Preparation)
            return;

        //get trigger info
        TriggerInfo triggerinfo = inputController.triggerInfo;
        //if mouse cursor on trigger
        if (triggerinfo != null)
        {
            dragStartTrigger = triggerinfo;

            GameObject unitGO = GetChampionFromTriggerInfo(triggerinfo);

            if (unitGO != null)
            {
                //show indicators
                //map.ShowIndicators();
                draggedUnit = unitGO;

                //isDragging = true;

                unitGO.GetComponent<DragObject>().IsOnDrag = true;
                //Debug.Log("STARTDRAG");
            }

        }
    }

    /// <summary>
    /// When we stop dragging champions on map
    /// </summary>
/*    public void StopDrag()
    {
        //hide indicators
        //map.HideIndicators();

        //int championsOnField = GetChampionCountOnHexGrid();


        if (draggedUnit != null)
        {
            //set dragged
            draggedUnit.GetComponent<DragObject>().IsOnDrag = false;

            //get trigger info
            TriggerInfo triggerinfo = inputController.triggerInfo;

            //if mouse cursor on trigger
            if (triggerinfo != null)
            {
                //get current champion over mouse cursor
                GameObject currentTriggerChampion = GetChampionFromTriggerInfo(triggerinfo);

                //there is another champion in the way
                if (currentTriggerChampion != null)
                {
                    //store this champion to start position
                    StoreChampionInArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ, currentTriggerChampion);

                    //store this champion to dragged position
                    StoreChampionInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedChampion);
                }
                else
                {
                    //we are adding to combat field
                    if (triggerinfo.gridType == Map.GRIDTYPE_HEXA_MAP)
                    {
                        //only add if there is a free spot or we adding from combatfield
                        if (championsOnField < currentChampionLimit || dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                        {
                            //remove champion from dragged position
                            RemoveChampionFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                            //add champion to dragged position
                            StoreChampionInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedChampion);

                            if (dragStartTrigger.gridType != Map.GRIDTYPE_HEXA_MAP)
                                championsOnField++;
                        }
                    }
                    else if (triggerinfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
                    {
                        //remove champion from dragged position
                        RemoveChampionFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                        //add champion to dragged position
                        StoreChampionInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedChampion);

                        if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            championsOnField--;
                    }



                }




            }


            //CalculateBonuses();

            //currentChampionCount = GetChampionCountOnHexGrid();

            //update ui
            //uIController.UpdateUI();


            draggedUnit = null;
        }


    }*/


    /// <summary>
    /// Get champion gameobject from triggerinfo
    /// </summary>
    /// <param name="triggerinfo"></param>
    /// <returns></returns>
    private GameObject GetChampionFromTriggerInfo(TriggerInfo triggerinfo)
    {
        GameObject championGO = null;

        if (triggerinfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
        {
            championGO = ownChampionInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            championGO = gridChampionsArray[triggerinfo.gridX, triggerinfo.gridZ];
        }

        return championGO;
    }

    
}
