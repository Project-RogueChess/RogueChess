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
}
