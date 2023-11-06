using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public HexaGridTilemapManager gridTileManager;

    //그리드의 타입
    public static int GRIDTYPE_OWN_INVENTORY = 0;
    public static int GRIDTYPE_HEXA_MAP = 1;

    //인벤토리타일크기
    public static int inventoryTileSize = 9;

    //그리드타일 시작지점
    public Transform ownInventoryStartposition;
    public Transform mapStartPosition;

    public Plane m_Plane;

    //기물을 어디에 배치해야 하는지 보여주는 표시
    public GameObject squareIndicator;
    public GameObject hexaIndicator;


    //타일 색깔 설정
    public Color indicatorDefaultColor;
    public Color indicatorActiveColor;




    // Start is called before the first frame update
    void Start()
    {
        CreateGridPostiion();
        CreateIndicators();
        //HideIndicators();

        m_Plane = new Plane(Vector3.up, Vector3.zero);
        gridTileManager = GetComponent<HexaGridTilemapManager>();
    }

    //목록에 그리드 위치 저장
    [HideInInspector]
    public Vector3[] ownInventoryStartpositions;
    [HideInInspector]
    public Vector3[,] mapGridPositions;

    private void CreateGridPostiion()
    {
        ownInventoryStartpositions = new Vector3[inventoryTileSize];

        for (int i = 0; i < inventoryTileSize; i++)
        {
            //calculate position x offset for this slot
            float offsetX = i * -2.5f;

            //calculate and store the position
            Vector3 position = GetMapHitPoint(ownInventoryStartposition.position + new Vector3(offsetX, 0, 0));

            //add position variable to array
            ownInventoryStartpositions[i] = position;
        }
    }

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
    private void CreateIndicators()
    {
        GameObject ColliderContainer = new GameObject();
        ColliderContainer.name = "ColliderContainer";

        indicatorContainer = new GameObject();
        indicatorContainer.name = "IndicatorContainer";

        unitInventoryArray = new GameObject[inventoryTileSize];
        invColliderArray = new TriggerInfo[inventoryTileSize];

        //iterate own grid position
        for (int i = 0; i < inventoryTileSize; i++)
        {
            //create indicator gameobject
            GameObject indicatorGO = Instantiate(squareIndicator);

            //set indicator gameobject position
            indicatorGO.transform.position = ownInventoryStartpositions[i];

            //set indicator parent
            indicatorGO.transform.parent = indicatorContainer.transform;

            //store indicator gameobject in array
            unitInventoryArray[i] = indicatorGO;

            //create trigger gameobject
            GameObject trigger = CreateBoxTrigger(GRIDTYPE_OWN_INVENTORY, i);

            //set trigger parent
            trigger.transform.parent = ColliderContainer.transform;

            //set trigger gameobject position
            trigger.transform.position = ownInventoryStartpositions[i];

            //store triggerinfo
            invColliderArray[i] = trigger.GetComponent<TriggerInfo>();
        }
    }


    /*private void HideIndicators()
    {
        throw new NotImplementedException();
    }*/


    /// <summary>
    /// Creates a trigger collider gameobject and returns it
    /// </summary>
    /// <returns></returns>
    private GameObject CreateBoxTrigger(int type, int x)
    {
        //create primitive gameobject
        GameObject trigger = new GameObject();

        //add collider component
        BoxCollider collider = trigger.AddComponent<BoxCollider>();

        //set collider size
        collider.size = new Vector3(2, 0.5f, 2);

        //set collider to trigger 
        collider.isTrigger = true;

        //add and store trigger info
        TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        trigerInfo.gridType = type;
        trigerInfo.gridX = x;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }

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
    public GameObject GetIndicatorFromTriggerInfo(TriggerInfo triggerinfo)
    {
        GameObject triggerGo = null;

        if (triggerinfo.gridType == GRIDTYPE_OWN_INVENTORY)
        {
            triggerGo = unitInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == GRIDTYPE_HEXA_MAP)
        {
            triggerGo = gridTileManager.hexaGrid[triggerinfo.gridX, triggerinfo.gridZ];
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

        for (int x = 0; x < 9; x++)
        {
            unitInventoryArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            // oponentIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
