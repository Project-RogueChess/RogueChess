using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HexaGridTileManager : MonoBehaviour
{
    public static HexaGridTileManager instance;

    public Camera mainCam;
    public GameObject[] tilePrefabs;
    public GameObject[,] hexaGrid;
    public Transform gridPivot;
    public Transform tileMapParent;
    public int mapX;
    public int mapY;
    public float spaceX = 1;
    public float spaceY = 1;

    private int _currentMapX;
    private int _currentMapY;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
                Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo,Mathf.Infinity,-1,QueryTriggerInteraction.Ignore))
        {
            var tileIndex = GetTileIndex(hitInfo.transform.gameObject, gridPivot.position, spaceX, spaceY);
            Debug.Log(tileIndex);
        }
    }

    private void OnValidate()
    {
        if(tilePrefabs.Length > 2)
        {
            Array.Resize(ref tilePrefabs, 2);
            Debug.LogAssertion("타일 프리팹은 최대 2개까지 가능");
        }
    }

    public void GenerateTile(int mapX, int mapY, float spaceX, float spaceY, Vector3 pivot)
    {
        _currentMapX = mapX;
        _currentMapY = mapY;

        hexaGrid = new GameObject[_currentMapY, _currentMapX];

        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                hexaGrid[i, j] = Instantiate(tilePrefabs[tilePrefabs.Length > 1 ? (j + i % 2) % 2 : 0]);
                hexaGrid[i, j].transform.position = new Vector3(j * spaceX + (i % 2 != 0 ? 0.5f * spaceX : 0), 0, i * spaceY) + pivot;
                hexaGrid[i, j].transform.parent = tileMapParent;
            }
        }
    }

    public void DestroyAllTiles(bool immediate = false)
    {
        if (hexaGrid == null)
            return;

        for (int i = 0; i < _currentMapY; i++)
        {
            for (int j = 0; j < _currentMapX; j++)
            {
                if (immediate)
                {
                    DestroyImmediate(hexaGrid[i, j].gameObject);
                }
                else
                {
                    Destroy(hexaGrid[i, j].gameObject);
                }
            }
        }

        hexaGrid = null;
    }

    public Vector2Int GetTileIndex(GameObject obj) 
    {
        return GetTileIndex(obj.transform.position, gridPivot.position, spaceX, spaceY);
    }

    public Vector2Int GetTileIndex(Vector3 pos, Vector3 pivot, float spaceX, float spaceY)
    {
        var correctPos = pos - pivot;
   
        int y = Mathf.RoundToInt(correctPos.z / spaceY);
        int x = Mathf.RoundToInt((correctPos.x - (y % 2 != 0 ? 0.5f * spaceX : 0) ) / spaceX);

        return new Vector2Int(x, y);
    }

    public Vector2Int GetTileIndex(GameObject obj, Vector3 pivot, float spaceX, float spaceY)
    {
        return GetTileIndex(obj.transform.position, pivot, spaceX, spaceY);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || hexaGrid == null)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(gridPivot.position, 0.4f);

        Gizmos.color = Color.white;
        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                if(hexaGrid[i, j] != null)
                    Gizmos.DrawSphere(hexaGrid[i,j].transform.position, 0.25f);
            }
        }
    }
}
