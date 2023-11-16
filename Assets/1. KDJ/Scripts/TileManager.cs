using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public Tile tile;
    public Plane m_Plane;

    [SerializeField]
    [Range(0, 1)]
    float dragSpeed;

    public GameObject prevPiece
    {
        get
        {
            return _prevPiece;
        }
        set
        {
            _prevPiece = value;
        }
    }
    public GameObject nextPiece 
    {
        get
        {
            return _nextPiece;
        }
        set
        {
            { _nextPiece = value; }
        }
    }

    public Tile prevTile;
    public Tile nextTile;

    [SerializeField] private GameObject _prevPiece;
    [SerializeField] private GameObject _nextPiece;

    public bool isDrag = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        if (isDrag)
        {
            /*prevPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                 Input.mousePosition.y, -Camera.main.transform.position.z));*/

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 100.0f;
            if (m_Plane.Raycast(ray, out enter))
            {

                Vector3 hitPoint = ray.GetPoint(enter);
                

                Vector3 p = new Vector3(hitPoint.x, 1.0f, hitPoint.z);

                prevPiece.transform.position = Vector3.Lerp(prevPiece.transform.position, p, dragSpeed);


            }
        }
    }

    public void AutoSelectTile()
    {
        if (isDrag)
        {
            if (nextTile != null && nextPiece == null
                && nextTile.tag != "Sell" && nextTile.triggerInfo.type == TileType.Hexa)
            {
                GameObject tempGO = nextPiece;
                prevPiece.transform.position = nextTile.transform.position;
                nextTile.piece = prevPiece;
                prevTile.piece = tempGO;
                HexaUnit unit = prevPiece.GetComponent<HexaUnit>();
                unit.SetTileIndex(new Vector2Int(nextTile.triggerInfo.x, nextTile.triggerInfo.y));
                HexaUnitManager.instance.RegisterHexaUnit(unit);
            }
            else
            {
                RandomSelectTile();
            }
            isDrag = false;
        }
    }

    void RandomSelectTile()
    {
        var tileIndex = new Vector2Int(Random.Range(0, 8), Random.Range(0, 4));

        if (HexaUnitManager.instance.unitList.Count > 32)
            return;

        if (DataManager.instance.WhatMyPieces() > DataManager.instance.WhatMyMAXPieces())
        {
            prevPiece.transform.position = prevTile.transform.position;
            return;
        }
            

        if (HexaUnitManager.instance.collisionMap[tileIndex.y, tileIndex.x])
        {
            RandomSelectTile();
            return;
        }

        foreach(var tile in InvSpawnManager.instance.hexaTiles)
        {
            if(tile.triggerInfo.x == tileIndex.x && tile.triggerInfo.y == tileIndex.y)
            {
                tile.piece = prevPiece;
                prevPiece.transform.position = tile.transform.position;
                HexaUnit unit = tile.piece.GetComponent<HexaUnit>();
                unit.SetTileIndex(new Vector2Int(tileIndex.x, tileIndex.y));
                HexaUnitManager.instance.RegisterHexaUnit(unit);
            }
        }
    }
}
