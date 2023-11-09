using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public TilemapManager gridTileManager;

    //그리드타일 시작지점
    public Transform ownInventoryStartposition;
    public Transform mapStartPosition;

    public Plane m_Plane;

    //기물을 어디에 배치해야 하는지 보여주는 표시
    public GameObject squareIndicator;
    public GameObject hexaIndicator;


    // Start is called before the first frame update
    void Start()
    {
                
        //HideIndicators();

        m_Plane = new Plane(Vector3.up, Vector3.zero);
        gridTileManager = GetComponent<TilemapManager>();
    }

    //목록에 그리드 위치 저장
    [HideInInspector]
    public Vector3[] ownInventoryStartpositions;
    [HideInInspector]
    public Vector3[,] mapGridPositions;


    //인디케이터를 저장할 배열 선언
    [HideInInspector]
    public GameObject[] unitInventoryArray;
    [HideInInspector]
    public GameObject[,] gridUnitsArray;

    [HideInInspector]
    public TriggerInfo[] invColliderArray;
    [HideInInspector]
    public TriggerInfo[,] mapColliderArray;

    private GameObject indicatorContainer;




    /// <summary>
    /// Get a point with accurate y axis
    /// 정확한 y좌표에서 포인트를 가져옴 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMapHitPoint(Vector3 p)
    {
        Vector3 newPos = p;

        RaycastHit hit;

        if (Physics.Raycast(newPos + new Vector3(0, 10, 0), Vector3.down, out hit, 15))
        {
            newPos = hit.point;
        }

        return newPos;
    }

    /// <summary>
    /// 트리거 인포 스크립트로 부터 그리드 인디케이터를 리턴함
    /// </summary>
    /// <param name="triggerinfo"></param>
    /// <returns></returns>
    public GameObject GetIndicatorFromTriggerInfo(TilemapTriggerInfo triggerinfo)
    {
        GameObject triggerGo = null;

        if (triggerinfo.type == TileType.Inv)
        {
            triggerGo = unitInventoryArray[triggerinfo.x];
        }
        else if (triggerinfo.type == TileType.Hexa)
        {
            triggerGo = gridUnitsArray[triggerinfo.x, triggerinfo.y];
        }

        return triggerGo;
    }

    /// <summary>
    /// Resets all indicator colors to default
    /// </summary>
    public void resetIndicators()
    {

        /* for (int x = 0; x < hexMapSizeX; x++)
         {
             for (int z = 0; z < hexMapSizeZ / 2; z++)
             {
                 mapIndicatorArray[x, z].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
             }
         }*/

        /*for (int x = 0; x < 9; x++)
        {
            unitInventoryArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            // oponentIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
        }*/
    }

    /// <summary>
    /// Make all map indicators visible
    /// </summary>
    public void ShowIndicators()
    {
        indicatorContainer.SetActive(true);
    }

    /// <summary>
    /// Make all map indicators invisible
    /// </summary>
    public void HideIndicators()
    {
        indicatorContainer.SetActive(false);
    }
}
