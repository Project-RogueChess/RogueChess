using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [Header("Input Children Panel Count X , Y")]
    [SerializeField] private int mapLengthX = 2;
    [SerializeField] private int mapLengthY = 2;
    [SerializeField] private int createResearchLengthX = 1;
    [SerializeField] private int createResearchLengthY = 1;
    [SerializeField] private int createMaxNord = 1;
    [SerializeField] private int secondFloorNordNumber = 3;

    [Header("Children Panel Prefab")]
    [SerializeField] private GameObject childrenPanelPrefab;

    [Header("CurrentNordInfomation")]
    [SerializeField] private Vector2Int currentNordXY; // 현재 노드가 뭔지 갱신 하죠


    Dictionary<Vector2Int, GameObject> childPanelDictionary = new Dictionary<Vector2Int, GameObject>();
    private Map_Nord nordScript;

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


        int startNordX = (int)(mapLengthX / 2);

        GameObject startNordPanel = FindPanelToSearchDictionary(startNordX, 0);

        if (startNordPanel == null) { Debug.LogWarning("Can Not Found Start Nord Possition"); }

        startNordPanel.AddComponent<Map_Nord>();

        if (secondFloorNordNumber > mapLengthX) { Debug.LogWarning("secondFloorNordNumber > mapLengthX"); }

        int secondFloorNordX = (int)(mapLengthX / secondFloorNordNumber);

        int secondFloorFirstNordX = (int)(secondFloorNordX / 2);
        GameObject secoundFloorFirstNord = FindPanelToSearchDictionary(secondFloorFirstNordX, 1);
        secoundFloorFirstNord.AddComponent<Map_Nord>();
        CreateRandomNord(new Vector2Int(secondFloorFirstNordX, 1));

        for (int i = 1; i < secondFloorNordNumber; i++)
        {
            GameObject secondNordPanel = FindPanelToSearchDictionary(secondFloorFirstNordX + (i * secondFloorNordX), 1);
            secondNordPanel.AddComponent<Map_Nord>();

            CreateRandomNord(new Vector2Int(secondFloorFirstNordX + (i * secondFloorNordX), 1));
        }

        //CreateRandomNord(new Vector2Int(secondFloorFirstNordX, 1));
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


    private void CreateRandomNord(Vector2Int startNordKey)
    {
        if (startNordKey.y != 1) { Debug.LogWarning("Wrong Access"); return; }

        List<Vector2Int> readyNord = new List<Vector2Int>();
        Vector2Int currentNordCreate = startNordKey;
        bool repeatWhile = true;
        int repeatCreateNord = Random.Range(0, createMaxNord) + 1;

        while (repeatWhile)
        {
            int randomListValue = 0;


            for (int nordY = (currentNordCreate.y + 1); nordY <= Mathf.Clamp((currentNordCreate.y + createResearchLengthY), 1, (mapLengthY - 2)); nordY++)
            {
                for (int nordX = Mathf.Clamp((currentNordCreate.x + createResearchLengthX), 0, mapLengthX - 1); nordX >= Mathf.Clamp((currentNordCreate.x - createResearchLengthX), 0, (mapLengthX - 1)); nordX--)
                {
                    readyNord.Add(new Vector2Int(nordX, nordY));
                    Debug.Log("AddRange");
                }

            }
            randomListValue = Random.Range(0, readyNord.Count);

            Debug.Log(readyNord[randomListValue]);
            GameObject createTargetPanel = FindPanelToSearchDictionary(readyNord[randomListValue]);

            if (createTargetPanel.GetComponent<Map_Nord>() == null)
            {
                createTargetPanel.AddComponent<Map_Nord>();
            }
            createTargetPanel.GetComponent<Map_Nord>().AddPrevNordKey(currentNordCreate);
            createTargetPanel.GetComponent<Map_Nord>().mykey = readyNord[randomListValue];

            currentNordCreate = readyNord[randomListValue];



            readyNord.Clear();


            Debug.Log("ClearWhile");
            if (currentNordCreate.y >= (mapLengthY - 2)) { repeatWhile = false; }
        }

    }


}
