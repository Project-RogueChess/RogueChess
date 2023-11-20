
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
            Debug.Log("드래그 통과");
            if (nextTile != null)
            {
                Debug.Log("타일 존재");

                if (nextTile.piece == null && DataManager.instance.WhatMyPieces() == DataManager.instance.WhatMyMAXPieces())
                {
                    prevPiece.transform.position = prevTile.transform.position;
                    InvSpawnManager.instance.CountArticle();
                    UIManager.instance.CloseSellText();
                    isDrag = false;
                    return;
                }
                if (nextTile.tag == "Sell")
                {
                    if (prevTile.triggerInfo.type == TileType.Hexa)
                        HexaUnitManager.instance.UnRegisterHexaUnit(prevTile.piece.GetComponent<HexaUnit>());

                    Pieces currentPieces = prevTile.piece.GetComponent<Pieces>();

                    if (currentPieces.grade == 1)
                        PiecesCountManager.instance.piecesIdCounts[currentPieces.id]--;

                    prevTile.piece.GetComponent<Pieces>().SellPiece();
                    prevTile.piece = null;
                    InvSpawnManager.instance.SearchEveryTileForSynergyData();
                    InvSpawnManager.instance.SynergyEnhance(InvSpawnManager.instance.CompareSynergy());
                    UIManager.instance.CloseSellText();
                    InvSpawnManager.instance.CountArticle();
                    return;
                }
                var tempGO = nextTile.piece;
                prevPiece.transform.position = nextTile.transform.position;
                nextTile.piece = prevPiece;
                prevTile.piece = tempGO;

                if (tempGO != null)
                {
                    prevTile.piece.transform.position = prevTile.transform.position;
                    if (nextTile.triggerInfo.type == TileType.Hexa)
                        HexaUnitManager.instance.UnRegisterHexaUnit(tempGO.GetComponent<HexaUnit>());

                    //만약에 가는 곳이 헥사인 경우에 다시 유닛등록
                    if (prevTile.triggerInfo.type == TileType.Hexa)
                    {
                        //다시 등록
                        var otherUnit = tempGO.GetComponent<HexaUnit>();
                        otherUnit.SetTileIndex(new Vector2Int(prevTile.triggerInfo.x, prevTile.triggerInfo.y));
                        HexaUnitManager.instance.RegisterHexaUnit(otherUnit);
                    }
                }

                if (nextTile.triggerInfo.type == TileType.Hexa)
                {
                    HexaUnit unit = nextTile.piece.GetComponent<HexaUnit>();
                    unit.SetTileIndex(new Vector2Int(nextTile.triggerInfo.x, nextTile.triggerInfo.y));
                    if(!HexaUnitManager.instance.unitList.Contains(unit))
                        HexaUnitManager.instance.RegisterHexaUnit(unit);
                }
            }
            else
            {
                if (prevTile.triggerInfo.type == TileType.Hexa)
                {
                    HexaUnitManager.instance.RegisterHexaUnit(prevPiece.GetComponent<HexaUnit>());
                }
                prevPiece.transform.position = prevTile.transform.position;
            }
            InvSpawnManager.instance.CountArticle();
            UIManager.instance.CloseSellText();
            isDrag = false;
        }
    }
}
