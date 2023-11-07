using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStage { Preparation, Combat, Loss };
public class DragController : MonoBehaviour
{

    public InputController inputController;
    public GameStage currentGameStage;

    private GameObject draggedUnit = null;
    private TilemapTriggerInfo dragStartTrigger = null;

    [HideInInspector]
    public int currentUnitLimit = 3;
    [HideInInspector]
    public int currentUnitCount = 0;

    [HideInInspector]
    public GameObject[] ownUnitsInventoryArray;
    [HideInInspector]
    public GameObject[,] gridUnitsArray;

    /// <summary>
    /// When we start dragging champions on map
    /// </summary>
    public void StartDrag()
    {
        if (currentGameStage != GameStage.Preparation)
            return;

        //get trigger info
        TilemapTriggerInfo triggerinfo = inputController.triggerInfo;
        //if mouse cursor on trigger
        if (triggerinfo != null)
        {
            dragStartTrigger = triggerinfo;

            GameObject unitGO = GetUnitFromTriggerInfo(triggerinfo);

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
    public void StopDrag()
    {
        //hide indicators
        //map.HideIndicators();

        //int unitsOnField = GetUnitsCountOnHexGrid();


        if (draggedUnit != null)
        {
            //set dragged
            draggedUnit.GetComponent<DragObject>().IsOnDrag = false;

            //get trigger info
            TilemapTriggerInfo triggerinfo = inputController.triggerInfo;

            //if mouse cursor on trigger
            if (triggerinfo != null)
            {
                //get current champion over mouse cursor
                GameObject currentTriggerChampion = GetUnitFromTriggerInfo(triggerinfo);

                //there is another champion in the way
                if (currentTriggerChampion != null)
                {
                    //store this champion to start position
                    StoreChampionInArray(dragStartTrigger.type, dragStartTrigger.x, dragStartTrigger.y, currentTriggerChampion);

                    //store this champion to dragged position
                    StoreChampionInArray(triggerinfo.type, triggerinfo.x, triggerinfo.y, draggedUnit);
                }
                else
                {
                    /*//we are adding to combat field
                    if (triggerinfo.gridType == Map.GRIDTYPE_HEXA_MAP)
                    {
                        //only add if there is a free spot or we adding from combatfield
                        if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                        {
                            //remove champion from dragged position
                            RemoveChampionFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                            //add champion to dragged position
                            StoreChampionInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedUnit);

                            if (dragStartTrigger.gridType != Map.GRIDTYPE_HEXA_MAP)
                                unitsOnField++;
                        }
                    }*/
                    if (triggerinfo.type == TileType.Inv)
                    {
                        //remove champion from dragged position
                        RemoveChampionFromArray(dragStartTrigger.type, dragStartTrigger.x, dragStartTrigger.y);

                        //add champion to dragged position
                        StoreChampionInArray(triggerinfo.type, triggerinfo.x, triggerinfo.y, draggedUnit);

                        /*if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            unitsOnField--;*/
                    }
                }
            }
            //CalculateBonuses();

            //currentUnitCount = GetUnitsCountOnHexGrid();

            //update ui
            //uIController.UpdateUI();
            draggedUnit = null;
        }

    }


    /// <summary>
    /// Get champion gameobject from triggerinfo
    /// </summary>
    /// <param name="triggerinfo"></param>
    /// <returns></returns>
    private GameObject GetUnitFromTriggerInfo(TilemapTriggerInfo triggerinfo)
    {
        GameObject unitGO = null;
        
        if (triggerinfo.type == TileType.Inv)
        {
            Debug.Log(triggerinfo.type);
            unitGO = ownUnitsInventoryArray[triggerinfo.x];
            
        }
        else if (triggerinfo.type == TileType.Hexa)
        {
            unitGO = gridUnitsArray[triggerinfo.x, triggerinfo.y];
        }

        return unitGO;
    }


    /// <summary>
    /// Store champion gameobject in array
    /// </summary>
    /// <param name="triggerinfo"></param>
    /// <param name="champion"></param>
    private void StoreChampionInArray(TileType gridType, int gridX, int gridZ, GameObject unit)
    {
        //assign current trigger to champion
        DragObject dragObject = unit.GetComponent<DragObject>();
        //dragObject.SetGridPosition(gridType, gridX, gridZ);

        if (gridType == TileType.Inv)
        {
            ownUnitsInventoryArray[gridX] = unit;
        }
        else if (gridType == TileType.Hexa)
        {
            gridUnitsArray[gridX, gridZ] = unit;
        }
    }

    /*    /// <summary>
        /// Returns the number of champions we have on the map
        /// </summary>
        /// <returns></returns>
        private int GetUnitsCountOnHexGrid()
        {
            int count = 0;
            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    //there is a champion
                    if (gridUnitsArray[x, z] != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }*/

    /// <summary>
    /// Remove champion from array
    /// </summary>
    /// <param name="triggerinfo"></param>
    private void RemoveChampionFromArray(TileType type, int gridX, int gridZ)
    {
        if (type == TileType.Inv)
        {
            ownUnitsInventoryArray[gridX] = null;
        }
        else if (type == TileType.Hexa)
        {
            gridUnitsArray[gridX, gridZ] = null;
        }
    }
}
