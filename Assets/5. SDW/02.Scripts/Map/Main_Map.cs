using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Main_Map : MonoBehaviour
{
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
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform lineParent;
    [SerializeField] private float lineThickness = 10f;

    [Header("Node Control")]
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

    [Header("Optional")]
    [SerializeField] private Vector2Int currentNodeXY; // 현재 노드가 뭔지 갱신 하죠
    [SerializeField] private GameObject motherMapObject;


    Dictionary<Vector2Int, GameObject> childPanelDictionary = new Dictionary<Vector2Int, GameObject>();
    private GameObject endNode;
    private Map_Node[] nodeScripts;
    bool isOnMap = false;
    private int nodeTypeCount;


    private void Start()
    {
        UI_MapRenderStart();

        nodeScripts = gameObject.GetComponentsInChildren<Map_Node>();


        StartCoroutine(StartMethod());
        //Invoke("LineDrawerLauncher", 0.1f);
        //Invoke("SettingNodeSystem", 0.2f); // Invoke 아니어도 될거같음
        ////SettingNodeSystem();
        //Invoke("DeleteNodeSystem", 0.3f);
        
    }
    IEnumerator StartMethod()
    {
        yield return new WaitForEndOfFrame();

        LineDrawerLauncher();
        SettingNodeSystem();
        DeleteNodeSystem();

        DisableMap();
    }

    void DisableMap()
    {
        motherMapObject.SetActive(false);
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
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UI_MapOnOff();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var nodeScript in nodeScripts)
            {
                if (nodeScript.prevNodeKeysProp != null)
                {
                    foreach (var prevNodeKey in nodeScript.prevNodeKeysProp)
                    {
                        LineDrawer(nodeScript.transform.position, childPanelDictionary[prevNodeKey].transform.position);
                    }
                }
            }
        }
    }
    /// <summary>
    /// motherPanelX,Y 는 본인의 너비 높이
    /// childrenPanelX,Y 는 자식으로 들어갈 cell패널의 크기
    /// </summary>

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

        GameObject endNodePanel = FindPanelToSearchDictionary(startNodeX, (mapLengthY - 1));
        endNodePanel.AddComponent<Map_Node>();
        endNode = endNodePanel;


        if (secondFloorNodeNumber > mapLengthX) { Debug.LogWarning("secondFloorNodeNumber > mapLengthX"); }

        int secondFloorNodeX = (int)(mapLengthX / secondFloorNodeNumber);

        int secondFloorFirstNodeX = (int)(secondFloorNodeX / 2);
        GameObject secoundFloorFirstNode = FindPanelToSearchDictionary(secondFloorFirstNodeX, startNodeHeight);
        secoundFloorFirstNode.AddComponent<Map_Node>();
        secoundFloorFirstNode.GetComponent<Map_Node>().AddPrevNodeKey(new Vector2Int(startNodeX, 0));
        for (int i = 0; i < createTreeCount; i++)
            CreateRandomNode(new Vector2Int(secondFloorFirstNodeX, startNodeHeight), createCopyTreeCount);


        for (int i = 1; i < secondFloorNodeNumber; i++)
        {
            GameObject secondNodePanel = FindPanelToSearchDictionary(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight);
            secondNodePanel.AddComponent<Map_Node>();

            secondNodePanel.GetComponent<Map_Node>().AddPrevNodeKey(new Vector2Int(startNodeX, 0));

            for (int j = 0; j < createTreeCount; j++)
                CreateRandomNode(new Vector2Int(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight), createCopyTreeCount);
        }

    }

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

    private void UI_MapOnOff() // 다 만들어지면 수정 요함 1101
    {

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
                    LineDrawer(nodeScript.transform.position, childPanelDictionary[prevNodeKey].transform.position);
                }
            }
        }
    }
    private void LineDrawer(Vector3 targetPanelVector, Vector3 currentPanelVector)
    {
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
}
