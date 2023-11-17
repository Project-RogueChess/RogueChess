using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Map_Node;
using Random = UnityEngine.Random;

public class Main_Map : MonoBehaviour
{
    #region InputValue
    [Header("Panel Control")]
    [SerializeField] private GameObject childrenPanelPrefab;
    [SerializeField] private int mapLengthX = 2;
    [SerializeField] private int mapLengthY = 2;
    [SerializeField] private int startNodeHeight = 1;
    [SerializeField] private int xCreateResearchLength = 1;
    [SerializeField] private int YMaxCreateResearchLength = 1;
    [SerializeField] private int YMinCreateResearchLength = 1;
    [SerializeField] private int secondFloorNodeNumber = 3;
    [SerializeField] private int createTreeCount = 1;
    [SerializeField] private int createCopyTreeCount = 1;
    [SerializeField] private float addTreePercent = 15f;

    [Header("Line Control")]
    [SerializeField] private GameObject linePrefabBlack;
    [SerializeField] private GameObject linePrefabRed;
    [SerializeField] private Transform lineParent;
    [SerializeField] private float lineThickness = 10f;

    [Header("Node Control")]
    [SerializeField] private Sprite startNodeSprite;
    [SerializeField] private Sprite endNodeSprite;
    [SerializeField] private float normalMonsterPer = 0f;
    [SerializeField] private Sprite normalMonsterSprite;
    [SerializeField] private float eliteMonsterPer = 0f;
    [SerializeField] private Sprite eliteMonsterSprite;
    [SerializeField] private float storePer = 0f;
    [SerializeField] private Sprite storeSprite;
    [SerializeField] private float treasurePer = 0f;
    [SerializeField] private Sprite treasureSprite;
    [SerializeField] private float randomEventPer = 0f;
    [SerializeField] private Sprite randomEventSprite;
    [SerializeField] private float shelterPer = 0f;
    [SerializeField] private Sprite shelterSprite;
    [SerializeField] private float clickedNodeSize = 20f;
    [SerializeField] private GameObject currentStateMarkerO;
    [SerializeField] private Sprite currentStateMarkerX;

    [Header("Node Setting")]
    [SerializeField] private GameObject randomEventUI;
    [SerializeField] private GameObject ShelterUI;
    [SerializeField] private GameObject StoreUI;

    [Header("Optional")]
    [SerializeField] public Vector2Int currentNodeXY; // 현재 노드가 뭔지 갱신 하죠
    [SerializeField] private GameObject motherMapObject;

    [Header("Scroll Control")]
    [SerializeField] private GameObject scroll;
    [SerializeField] ScrollRect scrollRect;


    Dictionary<Vector2Int, GameObject> childPanelDictionary = new Dictionary<Vector2Int, GameObject>();
    private GameObject prevPanel;
    private GameObject endNode;
    private Map_Node[] nodeScripts;
    bool isOnMap = false;
    public bool isAccent = false;
    Vector3 basicScrollRect;
    Vector3 basicTransform;

    Vector2Int startNodeXY;
    Vector2Int endNodeXY;
    #endregion

    #region External Call
    public void UI_MapOnOff()
    {
        scrollRect.content.localScale = basicScrollRect;
        gameObject.transform.position = basicTransform;

        if (isOnMap == false)
        {
            motherMapObject.SetActive(true);
            isOnMap = true;
        }

        else if (isOnMap == true)
        {
            motherMapObject.SetActive(false);
            isOnMap = false;
        }
    }
    #endregion

    private void Start()
    {
        UI_MapRenderStart();

        nodeScripts = gameObject.GetComponentsInChildren<Map_Node>();

        basicScrollRect = scrollRect.content.localScale;
        basicTransform = gameObject.transform.position;

        StartCoroutine(StartMethod());
        
    }
    IEnumerator StartMethod()
    {
        yield return new WaitForEndOfFrame();

        scroll.transform.SetParent(lineParent.transform);
        LineDrawerLauncher();
        SettingNodeSystem();
        DeleteNodeSystem();

        DisableMap();
    }
    private void Update()
    {
        ZoomMap();

        if(gameObject.transform.position.y < -100)
        {
            gameObject.transform.position = new Vector3 (gameObject.transform.position.x,-100,gameObject.transform.position.z);
        }

        if (gameObject.transform.position.y > 1000)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1000, gameObject.transform.position.z);
        }

    }

    
    float zoomSpeed = 0.05f;
    float minZoom = 1f;
    float maxZoom = 2.0f;

    void ZoomMap()
    {
        float scrollDelta = Input.mouseScrollDelta.y;

        // 스크롤 입력이 감지되면 확대/축소 수행
        if (scrollDelta != 0)
        {
            // 현재 확대 수준 가져오기
            float currentZoom = scrollRect.content.localScale.y;

            // 새로운 확대 수준 계산
            float newZoom = Mathf.Clamp(currentZoom - (-scrollDelta) * zoomSpeed, minZoom, maxZoom);

            // 지도에 새로운 확대 수준 적용
            scrollRect.content.localScale = new Vector3(newZoom, newZoom, 1.0f);

        }
    }

    #region Starting Methods
    private void UI_MapRenderStart()
    {
        float motherPanelX = gameObject.GetComponent<RectTransform>().rect.width;
        float motherPanelY = gameObject.GetComponent<RectTransform>().rect.height;

        float childrenPanelX = motherPanelX / mapLengthX;
        float childrenPanelY = motherPanelY / mapLengthY;



        GetComponent<GridLayoutGroup>().cellSize = new Vector2(childrenPanelX, childrenPanelY);


        for (int Y = 0; Y < mapLengthY; Y++)
        {
            for (int X = 0; X < mapLengthX; X++)
            {
                GameObject childPanel = Instantiate(childrenPanelPrefab, gameObject.transform);
                childPanelDictionary.Add(new Vector2Int(X, Y), childPanel);
            }
        }


        int startNodeX = (int)(mapLengthX / 2);

        GameObject startNodePanel = FindPanelToSearchDictionary(startNodeX, 0);
        startNodePanel.AddComponent<Map_Node>();
        startNodePanel.GetComponent<Map_Node>().mykey = new Vector2Int(startNodeX, 0);

        Instantiate(currentStateMarkerO, startNodePanel.transform);

        currentNodeXY = new Vector2Int(startNodeX, 0); // this
        startNodeXY = new Vector2Int(startNodeX, 0);

        GameObject endNodePanel = FindPanelToSearchDictionary(startNodeX, (mapLengthY - 1));
        endNodePanel.AddComponent<Map_Node>();
        endNodePanel.GetComponent<Map_Node>().mykey = new Vector2Int(startNodeX, (mapLengthY - 1));
        endNode = endNodePanel;

        endNodeXY = new Vector2Int(startNodeX, (mapLengthY - 1));

        if (secondFloorNodeNumber > mapLengthX) { Debug.LogWarning("secondFloorNodeNumber > mapLengthX"); }

        int secondFloorNodeX = (int)(mapLengthX / secondFloorNodeNumber);

        int secondFloorFirstNodeX = (int)(secondFloorNodeX / 2);
        GameObject secoundFloorFirstNode = FindPanelToSearchDictionary(secondFloorFirstNodeX, startNodeHeight);
        secoundFloorFirstNode.AddComponent<Map_Node>();
        secoundFloorFirstNode.GetComponent<Map_Node>().AddPrevNodeKey(new Vector2Int(startNodeX, 0));
        secoundFloorFirstNode.GetComponent<Map_Node>().mykey = new Vector2Int(secondFloorFirstNodeX, startNodeHeight);
        for (int i = 0; i < createTreeCount; i++)
            CreateRandomNode(new Vector2Int(secondFloorFirstNodeX, startNodeHeight), createCopyTreeCount);


        for (int i = 1; i < secondFloorNodeNumber; i++)
        {
            GameObject secondNodePanel = FindPanelToSearchDictionary(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight);
            secondNodePanel.AddComponent<Map_Node>();
            secondNodePanel.GetComponent<Map_Node>().mykey = new Vector2Int(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight);
            secondNodePanel.GetComponent<Map_Node>().AddPrevNodeKey(new Vector2Int(startNodeX, 0));

            for (int j = 0; j < createTreeCount; j++)
                CreateRandomNode(new Vector2Int(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight), createCopyTreeCount);
        }

    }
    private void CreateRandomNode(Vector2Int startNodeKey, int repeatCount)
    {
        //if (startNodeKey.y != startNodeHeight) { Debug.LogWarning("Wrong Access"); return; }

        List<Vector2Int> readyNode = new List<Vector2Int>();
        Vector2Int currentNodeCreate = startNodeKey;
        Vector2Int treeCreate = startNodeKey;
        bool repeatWhile = true;

        while (repeatWhile)
        {
            for (int nodeY = Mathf.Clamp((currentNodeCreate.y + YMinCreateResearchLength), startNodeHeight, (mapLengthY - 2)); nodeY <= Mathf.Clamp((currentNodeCreate.y + YMaxCreateResearchLength), startNodeHeight, (mapLengthY - 2)); nodeY++)
            {
                for (int nodeX = Mathf.Clamp((currentNodeCreate.x + xCreateResearchLength), 0, mapLengthX - 1); nodeX >= Mathf.Clamp((currentNodeCreate.x - xCreateResearchLength), 0, (mapLengthX - 1)); nodeX--)
                {
                    readyNode.Add(new Vector2Int(nodeX, nodeY));
                }
            }
            int randomListValue = Random.Range(0, readyNode.Count);

            GameObject createTargetPanel = FindPanelToSearchDictionary(readyNode[randomListValue]);

            float addTreeValue = Random.Range(0, 100);
            if (addTreePercent > addTreeValue)
            {
                treeCreate = readyNode[randomListValue];
            }

            if (createTargetPanel.GetComponent<Map_Node>() == null)
            {
                createTargetPanel.AddComponent<Map_Node>();
            }
            createTargetPanel.GetComponent<Map_Node>().AddPrevNodeKey(currentNodeCreate);
            createTargetPanel.GetComponent<Map_Node>().mykey = readyNode[randomListValue];

            currentNodeCreate = readyNode[randomListValue];

            if (treeCreate != startNodeKey && repeatCount > 0)
            {
                CreateRandomNode(treeCreate, repeatCount-1);
            }

            if (currentNodeCreate.y == (mapLengthY - 2))
            {
                repeatWhile = false;
                endNode.GetComponent<Map_Node>().AddPrevNodeKey(readyNode[randomListValue]);
            }
            readyNode.Clear();
        }

    }
    private void LineDrawerLauncher()
    {
        foreach (var nodeScript in nodeScripts)
        {
            if (nodeScript.prevNodeKeysProp != null)
            {
                foreach (var prevNodeKey in nodeScript.prevNodeKeysProp)
                {
                    LineDrawer(nodeScript.mykey, prevNodeKey, linePrefabBlack);
                }
            }
        }
    }
    private void SettingNodeSystem()
    {
        foreach(var nodeScript in nodeScripts)
        {
            float randomValue = Random.Range(0, normalMonsterPer + eliteMonsterPer + storePer + treasurePer + randomEventPer + shelterPer);

            if (randomValue < normalMonsterPer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.NormalMonster;
                nodeScript.ChangeImageSprite(normalMonsterSprite);
            }

            else if (randomValue < normalMonsterPer + eliteMonsterPer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.EliteMonster;
                nodeScript.ChangeImageSprite(eliteMonsterSprite);
            }

            else if (randomValue < normalMonsterPer + eliteMonsterPer + storePer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.Store;
                nodeScript.ChangeImageSprite(storeSprite);
            }

            else if (randomValue < normalMonsterPer + eliteMonsterPer + storePer + treasurePer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.Treasure;
                nodeScript.ChangeImageSprite(treasureSprite);
            }

            else if (randomValue < normalMonsterPer + eliteMonsterPer + storePer + treasurePer + randomEventPer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.RandomEvent;
                nodeScript.ChangeImageSprite(randomEventSprite);
            }

            else if (randomValue <= normalMonsterPer + eliteMonsterPer + storePer + treasurePer + randomEventPer + shelterPer)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.Shelter;
                nodeScript.ChangeImageSprite(shelterSprite);
            }

            if(nodeScript.mykey == startNodeXY)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.Start;
                nodeScript.ChangeImageSprite(startNodeSprite);
            }

            if(nodeScript.mykey == endNodeXY)
            {
                nodeScript.myNodeType = Map_Node.currentNodeTypeEnum.End;
                nodeScript.ChangeImageSprite(endNodeSprite);
            }
        }
    }
    private void DeleteNodeSystem()
    {
        if (gameObject.GetComponent<GridLayoutGroup>() != null)
        {
            Destroy(gameObject.GetComponent<GridLayoutGroup>());
        }

        for (int y = 0; y < mapLengthY; y++)
        {
            for (int x = 0; x < mapLengthX; x++)
            {
                if (FindPanelToSearchDictionary(x, y) != null)
                {
                    GameObject panel = FindPanelToSearchDictionary(x, y);
                    if (panel.GetComponent<Map_Node>() == null)
                    {
                        Destroy(panel);
                    }
                }
            }
        }
    }
    void DisableMap()
    {
        motherMapObject.SetActive(false);
    }
    #endregion

    #region Click System

    public void ClickedTrueNodeAndMove(Vector2Int trueNode)
    {
        Vector2Int prevNodeKey = currentNodeXY;

        FindPanelToSearchDictionary(currentNodeXY).GetComponent<Map_Node>().ChangeStateImage(currentStateMarkerX);

        currentNodeXY = trueNode;

        Instantiate(currentStateMarkerO, FindPanelToSearchDictionary(trueNode).transform);


        NodeAction();

        LineDrawer(prevNodeKey, currentNodeXY, linePrefabRed);
        
    }

    public void ShowMap()
    {
        scrollRect.content.localScale = basicScrollRect;
        gameObject.transform.position = basicTransform;
        motherMapObject.SetActive(true);
        isOnMap = true;
    }

    public void HideMap()
    {
        scrollRect.content.localScale = basicScrollRect;
        gameObject.transform.position = basicTransform;
        motherMapObject.SetActive(false);
        isOnMap = false;
    }

    public void AccentNode(GameObject panelNode)
    {
        if (isAccent == false && prevPanel == null)
        {
            Debug.Log("isAccent F , prevPanel N");
            isAccent = true;
            prevPanel = panelNode;
            panelNode.GetComponent<Map_Node>().ChangeNodeRectSizeUp(clickedNodeSize);
        }

        else if (isAccent == true && prevPanel == panelNode)
        {
            Debug.Log("isAccent T , prevPanel P");
            ClickedTrueNodeAndMove(panelNode.GetComponent<Map_Node>().mykey);
            prevPanel.GetComponent<Map_Node>().ChangeNodeRectSizeDown();

            isAccent = false;
            prevPanel = null;
        }

        else if (isAccent == true && prevPanel != panelNode)
        {
            Debug.Log("isAccent T , prevPanel !P");
            prevPanel.GetComponent<Map_Node>().ChangeNodeRectSizeDown();
            isAccent = false;
            prevPanel = null;
        }

        else
        {
            Debug.Log(prevPanel + " + " + isAccent + " + " + prevPanel);
        }

    }

    public void FalseAccentNode(GameObject panelNode)
    {
        if (prevPanel != null)
        {
            prevPanel.GetComponent<Map_Node>().ChangeNodeRectSizeDown();
        }
        isAccent = false;
        prevPanel = null;
    }

    private void NodeAction()
    {
        switch (FindPanelToSearchDictionary(currentNodeXY).GetComponent<Map_Node>().myNodeType)
        {
            case currentNodeTypeEnum.Start:
                break;
            case currentNodeTypeEnum.End:
                break;
            case currentNodeTypeEnum.NormalMonster:
                CreepSpawnManager.instance.LoadCreepToField(Random.Range(1, 8));
                GameManager.instance.RunPhase();
                break;
            case currentNodeTypeEnum.EliteMonster:
                CreepSpawnManager.instance.LoadCreepToField(Random.Range(8, 16));
                GameManager.instance.RunPhase();
                break;
            case currentNodeTypeEnum.RandomEvent:
                randomEventUI.GetComponent<RdmEvt_Main>().RandomEventStart();
                GameManager.instance.ForceChangePhaseAndInvoke(Phase.SelectMapNode);
                break;
            case currentNodeTypeEnum.Shelter:
                ShelterUI.SetActive(true);
                break;
            case currentNodeTypeEnum.Store:
                StoreUI.GetComponent<Store_Main>().StoreNodeStart();
                break;
            case currentNodeTypeEnum.Treasure:
                randomEventUI.GetComponent<RdmEvt_Main>().TreasureEventStart();
                break;
            case currentNodeTypeEnum.None:
                break;
        }
    }

    #endregion

    #region DeveloperTools
    private GameObject FindPanelToSearchDictionary(int X, int Y)
    {
        GameObject resultGameObject;
        Vector2Int key = new Vector2Int(X, Y);

        if (childPanelDictionary.TryGetValue(key, out resultGameObject))
        {
            return resultGameObject;
        }

        return null;
    }
    private GameObject FindPanelToSearchDictionary(Vector2Int xy)
    {
        GameObject resultGameObject;
        Vector2Int key = xy;

        if (childPanelDictionary.TryGetValue(key, out resultGameObject))
        {
            return resultGameObject;
        }

        return null;
    }

    private Vector2Int ChangePossibleIndexVector2Int(Vector2Int key)
    {
        key.x = Mathf.Clamp(key.x, 0, mapLengthX);
        key.y = Mathf.Clamp(key.y, 0, mapLengthY);

        return key;
    }

    private int ChangePossibleIndexintX(int key)
    {
        key = Mathf.Clamp(key, 0, mapLengthX);

        return key;
    }

    private int ChangePossibleIndexintY(int key)
    {
        key = Mathf.Clamp(key, 0, mapLengthY);

        return key;
    }
    private void LineDrawer(Vector2Int targetPanelKey, Vector2Int currentPanelKey, GameObject linePrefab)
    {
        Vector3 targetPanelVector = FindPanelToSearchDictionary(targetPanelKey).transform.position;
        Vector3 currentPanelVector = FindPanelToSearchDictionary(currentPanelKey).transform.position;
        GameObject targetPanelObject = FindPanelToSearchDictionary(targetPanelKey);

        Vector3 linePos = Vector3.Lerp(targetPanelVector, currentPanelVector, 0.5f);
        Vector3 differenceVector = targetPanelVector - currentPanelVector;

        float angleRadian = Mathf.Atan2(differenceVector.y, differenceVector.x);
        float degrees = angleRadian * Mathf.Rad2Deg;

        GameObject lineObject = Instantiate(linePrefab, lineParent);
        RectTransform lineRect = lineObject.GetComponent<RectTransform>();

        lineObject.GetComponent<RawImage>().uvRect = new Rect(0, 0, differenceVector.magnitude / 20, 1);

        lineRect.sizeDelta = new Vector2(differenceVector.magnitude, lineThickness);
        lineRect.rotation = Quaternion.Euler(0, 0, degrees);
        lineRect.position = linePos;

    }
    #endregion





}
