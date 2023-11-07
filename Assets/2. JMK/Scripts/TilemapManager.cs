using System;
using System.Collections.Generic;
using UnityEngine;
using JMK.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Unity.Collections;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;

    [Header("공통 설정")]
    public float triggerSize = 3f;

    [Header("육각타일맵")]
    [HideInInspector] public Vector3[,] hexa_tilePosList;
    public Color hexa_defColor = Color.white;
    public Color hexa_activeColor = Color.white;
    public GameObject hexa_tilePrefab;
    public Transform hexa_tilemapPivot;
    public int hexa_tilemapSizeX;
    public int hexa_tilemapSizeY;
    public float hexa_spaceX = 1;
    public float hexa_spaceY = 1;

    [Header("인벤토리 타일맵")]
    [HideInInspector] public Vector3[] inv_tilePosList;
    public Color inv_defColor = Color.white;
    public Color inv_activeColor = Color.white;
    public GameObject inv_tilePrefab;
    public Transform inv_tilemapPivot;
    public int inv_size;
    public float inv_space = 1;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void initTilePosList()
    {
        hexa_tilePosList = new Vector3[hexa_tilemapSizeY, hexa_tilemapSizeX]; 
        inv_tilePosList = new Vector3[inv_size];

        for (int i = 0; i < hexa_tilemapSizeY; i++)
            for (int j = 0; j < hexa_tilemapSizeX; j++)
            {
                hexa_tilePosList[i,j] = new Vector3(j * hexa_spaceX + (i % 2 == 0 ? 0.5f * hexa_spaceX : 0), 0, i * hexa_spaceY) + hexa_tilemapPivot.position;
            }

        for (int i = 0; i < inv_size; i++)
        {
            inv_tilePosList[i] = new Vector3(i * inv_space, 0, 0) + inv_tilemapPivot.position;
        }
    }

    public void CreateTilemaps()
    {
        initTilePosList();

        //육각타일맵 생성
        GameObject hexaTilemapContainer = new GameObject();
        hexaTilemapContainer.name = "Tiles";
        hexaTilemapContainer.transform.parent = hexa_tilemapPivot;

        GameObject hexaTileTriggerContainer = new GameObject();
        hexaTileTriggerContainer.name = "Triggers";
        hexaTileTriggerContainer.transform.parent = hexa_tilemapPivot;

        for (int i = 0; i < hexa_tilemapSizeY; i++)
            for(int j = 0; j < hexa_tilemapSizeX; j++)
            {
                GameObject tileGO = Instantiate(hexa_tilePrefab);
               
                tileGO.transform.position = hexa_tilePosList[i, j];
                tileGO.transform.parent = hexaTilemapContainer.transform;
                tileGO.name = $"[{tileGO.transform.parent.childCount}]Tile";
                if (Application.isPlaying && tileGO.TryGetComponent(out MeshRenderer renderer))
                    renderer.material.color = hexa_defColor;

                //트리거 생성
                GameObject triggerGO = CreateTrigger(TileType.Inv, triggerSize, j,i);
                triggerGO.transform.position = hexa_tilePosList[i, j];
                triggerGO.transform.parent = hexaTileTriggerContainer.transform;
                triggerGO.name = $"[{triggerGO.transform.parent.childCount}]Trigger";
            }

        //인벤토리 타일맵 생성
        GameObject invTilemapContainer = new GameObject();
        invTilemapContainer.name = "Tiles";
        invTilemapContainer.transform.parent = inv_tilemapPivot;

        GameObject invTileTriggerContainer = new GameObject();
        invTileTriggerContainer.name = "Triggers";
        invTileTriggerContainer.transform.parent = inv_tilemapPivot;


        for (int i = 0; i < inv_size; i++)
        {
            GameObject tileGO = Instantiate(inv_tilePrefab);

            tileGO.transform.position = inv_tilePosList[i];
            tileGO.transform.parent = invTilemapContainer.transform;
            tileGO.name = $"[{tileGO.transform.parent.childCount}]Tile";
            if (Application.isPlaying && tileGO.TryGetComponent(out MeshRenderer renderer))
                renderer.material.color = inv_defColor;

            GameObject triggerGO = CreateTrigger(TileType.Inv, triggerSize, i);
            triggerGO.transform.position = inv_tilePosList[i];
            triggerGO.transform.parent = invTileTriggerContainer.transform;
            triggerGO.name = $"[{triggerGO.transform.parent.childCount}]Trigger";
        }
    }

    private GameObject CreateTrigger(TileType type, float radius, int x, int y = -1)
    {
        var currentTrigger = new GameObject();

        var collider = currentTrigger.AddComponent<SphereCollider>();
        collider.radius = radius;

        var info = currentTrigger.AddComponent<TilemapTriggerInfo>();

        info.type = type;
        info.x = x;
        info.y = y;

        return currentTrigger;
    }

    public void DestroyAllTilemaps(bool immediate = false)
    {
        if (hexa_tilemapPivot == null || hexa_tilemapPivot.childCount < 1)
            return;

        var childs = new Transform[hexa_tilemapPivot.childCount];

        for(int i = 0; i < childs.Length; i++)
        {
            childs[i] = hexa_tilemapPivot.GetChild(i);
        }

        foreach (Transform g in childs)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (g != hexa_tilemapPivot.transform)
            {
                if (immediate)
                    DestroyImmediate(g.gameObject);
                else
                    Destroy(g.gameObject);
            }
        }

        if (inv_tilemapPivot == null || inv_tilemapPivot.childCount < 1)
            return;

        childs = new Transform[inv_tilemapPivot.childCount];

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = inv_tilemapPivot.GetChild(i);
        }

        foreach (Transform g in childs)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (g != hexa_tilemapPivot.transform)
            {
                if (immediate)
                    DestroyImmediate(g.gameObject);
                else
                    Destroy(g.gameObject);
            }
        }
    }
}


