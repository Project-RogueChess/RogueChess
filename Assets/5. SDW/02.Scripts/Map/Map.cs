using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Map : MonoBehaviour
{
    [Header("Input Children Panel Count X , Y")]
    [SerializeField] private int mapLengthX = 2;
    [SerializeField] private int mapLengthY = 2;

    [Header("Children Panel Prefab")]
    [SerializeField] private GameObject childrenPanelPrefab;

    Dictionary<Vector2Int, GameObject> childPanelDictionary = new Dictionary<Vector2Int, GameObject>();
    private Map_Nord nordScript;

    private void Start()
    {
        UI_MapRenderStart();

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


        for (int Y = 0;  Y < mapLengthY; Y++)
        {
            for (int X = 0; X < mapLengthX; X++)
            {
                GameObject childPanel = Instantiate(childrenPanelPrefab, gameObject.transform);
                //childPanel.AddComponent<Map_Nord>();
                //childPanel.GetComponent<Map_Nord>().locationX = X;
                //childPanel.GetComponent<Map_Nord>().locationY = Y;
                childPanelDictionary.Add(new Vector2Int(X,Y), childPanel);
            }
        }


        int startNordX = (int)(mapLengthX / 2);

        GameObject startNordPanel = FindPanelToSearchDictionary(startNordX, 0);

        if (startNordPanel == null) { Debug.LogWarning("Can Not Found Start Nord Possition"); }

        startNordPanel.AddComponent<Map_Nord>();
    }

    private GameObject FindPanelToSearchDictionary(int X,int Y)
    {
        GameObject resultGameObject;
        Vector2Int key = new Vector2Int(X,Y);

        if (childPanelDictionary.TryGetValue(key, out resultGameObject))
        {
            return resultGameObject;
        }

        return null;
    }

    private void UI_MapOnOff() // 다 만들어지면 수정 요함 1101
    {
        bool isOnMap = false;

        if (isOnMap ==  false)
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
}
