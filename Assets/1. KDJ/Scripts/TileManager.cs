using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

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

    private void Update()
    {
        if (isDrag)
        {
            prevPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }
    }
}
