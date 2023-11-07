using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Main_Map : MonoBehaviour
{
    [Header("Panel Control")]
    [SerializeField] private int mapLengthX = 2;
    [SerializeField] private int mapLengthY = 2;
    [SerializeField] private int startNodeHeight = 1;
    [SerializeField] private int xCreateResearchLength = 1;
    [SerializeField] private int YMaxCreateResearchLength = 1;
    [SerializeField] private int YMinCreateResearchLength = 1;
    [SerializeField] private int createTreeCount = 1;
    [SerializeField] private int secondFloorNodeNumber = 3;

    [Header("Children Panel Prefab")]
    [SerializeField] private GameObject childrenPanelPrefab;

    [Header("CurrentNodeInfomation")]
    [SerializeField] private Vector2Int currentNodeXY; // 현재 노드가 뭔지 갱신 하죠


    Dictionary<Vector2Int, GameObject> childPanelDictionary = new Dictionary<Vector2Int, GameObject>();
    private GameObject endNode;
    private Map_Node nodeScript;

    private void Start()
    {
        UI_MapRenderStart();

        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log(i);
        //    //결과값 0 1 2 3 4
        //}
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            UI_MapOnOff();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

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
        CreateRandomNode(new Vector2Int(secondFloorFirstNodeX, startNodeHeight));

        for (int i = 1; i < secondFloorNodeNumber; i++)
        {
            GameObject secondNodePanel = FindPanelToSearchDictionary(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight);
            secondNodePanel.AddComponent<Map_Node>();

            secondNodePanel.GetComponent<Map_Node>().AddPrevNodeKey(new Vector2Int(startNodeX, 0));

        for (int j = 0; j < createTreeCount; j++)
            CreateRandomNode(new Vector2Int(secondFloorFirstNodeX + (i * secondFloorNodeX), startNodeHeight));
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
        bool isOnMap = false;

        if (isOnMap == false)
        {
            this.enabled = true;
            isOnMap = true;
        }

        else if (isOnMap == true)
        {
            this.enabled = false;
            isOnMap = false;
        }
    }


    private void CreateRandomNode(Vector2Int startNodeKey)
    {
        if (startNodeKey.y != startNodeHeight) { Debug.LogWarning("Wrong Access"); return; }

        List<Vector2Int> readyNode = new List<Vector2Int>();
        Vector2Int currentNodeCreate = startNodeKey;
        bool repeatWhile = true;

        while (repeatWhile)
        {
            int randomListValue = 0;


            for (int nodeY = Mathf.Clamp((currentNodeCreate.y + YMinCreateResearchLength), startNodeHeight, (mapLengthY - 2)); nodeY <= Mathf.Clamp((currentNodeCreate.y + YMaxCreateResearchLength), startNodeHeight, (mapLengthY - 2)); nodeY++)
            {
                for (int nodeX = Mathf.Clamp((currentNodeCreate.x + xCreateResearchLength), 0, mapLengthX - 1); nodeX >= Mathf.Clamp((currentNodeCreate.x - xCreateResearchLength), 0, (mapLengthX - 1)); nodeX--)
                {
                    readyNode.Add(new Vector2Int(nodeX, nodeY));
                    Debug.Log("AddRange");
                }

            }
            randomListValue = Random.Range(0, readyNode.Count);

            Debug.Log(readyNode[randomListValue]);
            GameObject createTargetPanel = FindPanelToSearchDictionary(readyNode[randomListValue]);

            if (createTargetPanel.GetComponent<Map_Node>() == null)
            {
                createTargetPanel.AddComponent<Map_Node>();
            }
            createTargetPanel.GetComponent<Map_Node>().AddPrevNodeKey(currentNodeCreate);
            createTargetPanel.GetComponent<Map_Node>().mykey = readyNode[randomListValue];

            currentNodeCreate = readyNode[randomListValue];

            if (currentNodeCreate.y == (mapLengthY - 2))
            {
                repeatWhile = false;
                endNode.GetComponent<Map_Node>().AddPrevNodeKey(readyNode[randomListValue]);
            }
            readyNode.Clear();
        }

    }


}
