using System;
using System.Collections.Generic;
using UnityEngine;
using JMK.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Unity.Collections;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class HexaGridTilemapManager : MonoBehaviour
{
    public static HexaGridTilemapManager instance;

    public Color tileDefaultColor = Color.white;
    public Color ActiveColor = Color.white;
    public Camera mainCam;
    public GameObject[] tilePrefabs;
    public GameObject[,] hexaGrid;
    public Transform gridPivot;
    public Transform tilemapParent;
    public int mapX;
    public int mapY;
    public float spaceX = 1;
    public float spaceY = 1;

    private static int _currentMapX;
    private static int _currentMapY;
    private static float _currentSpaceX;
    private static float _currentSpaceY;

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

    private void Start()
    {
        mainCam = Camera.main;
    }

    [SerializeField] private bool selectStart;
    [SerializeField] private bool selectEnd;
    [SerializeField] private Vector2Int selectStartIndex;
    [SerializeField] private Vector2Int selectEndIndex;

    [SerializeField] GameObject[] loadPath;
    [SerializeField] GameObject loadTiles;
    [SerializeField] int range;

    private void Update()
    {
        if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo,Mathf.Infinity,-1,QueryTriggerInteraction.Ignore))
        {
            var tileIndex = GetTileIndex(hitInfo.transform.gameObject, gridPivot.position, spaceX, spaceY);

            //Debug.Log(tileIndex);

            //시작 지점 지정
            if(Input.GetKeyDown(KeyCode.Z) && !selectStart)
            {
                selectStart = true;
                selectStartIndex = tileIndex;
            }

            //끝 지점 지정
            if (Input.GetKeyDown(KeyCode.X) && !selectEnd)
            {
                selectEnd = true;
                selectEndIndex = tileIndex;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                if (loadPath != null)
                {
                    foreach (var obj in loadPath)
                        Destroy(obj);
                    loadPath = null;
                }

                var currentList = RangeOfHexaGridIndex(tileIndex, range);

                loadPath = new GameObject[currentList.Count];

                for(int i = 0; i < loadPath.Length; i++)
                {
                    loadPath[i] = Instantiate(loadTiles);
                    loadPath[i].transform.position = hexaGrid[currentList[i].y, currentList[i].x].transform.position;
                }
            }
        }

        if(selectStart && selectEnd)
        {
            //트리거 리셋
            selectStart = false;
            selectEnd = false;

            //이전 길 표시는 삭제
            if (loadPath != null)
            {
                foreach (var obj in loadPath)
                    Destroy(obj);
                loadPath = null;
            }

            //길 찾기
            var pathlist = PathFinding(selectStartIndex, selectEndIndex);

            //못 찾았음 리턴
            if (pathlist.Count == 0)
                return;

            //길 표시 오브젝트 생성
            loadPath = new GameObject[pathlist.Count];
            int addCount = 0;

            foreach(var pos in pathlist)
            {
                loadPath[addCount] = Instantiate(loadTiles);
                loadPath[addCount].transform.position = pos;
                addCount++;
            }
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
        tilemapParent.parent = gridPivot;

        _currentMapX = mapX;
        _currentMapY = mapY;
        _currentSpaceX = spaceX;
        _currentSpaceY = spaceY;

        hexaGrid = new GameObject[_currentMapY, _currentMapX];

        for (int i = 0; i < mapY; i++)
        {
            for (int j = 0; j < mapX; j++)
            {
                hexaGrid[i, j] = Instantiate(tilePrefabs[tilePrefabs.Length > 1 ? (j + i % 2) % 2 : 0]);
                hexaGrid[i, j].transform.position = new Vector3(j * spaceX + (i % 2 == 0 ? 0.5f * spaceX : 0), 0, i * spaceY) + pivot;
                hexaGrid[i, j].transform.parent = tilemapParent;
                hexaGrid[i, j].GetComponent<MeshRenderer>().material.color = tileDefaultColor;
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
        int x = Mathf.RoundToInt((correctPos.x - (y % 2 == 0 ? 0.5f * spaceX : 0) ) / spaceX);

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
        for (int i = 0; i < _currentMapY; i++)
        {
            for (int j = 0; j < _currentMapX; j++)
            {
                if(hexaGrid[i, j] != null)
                    Gizmos.DrawSphere(hexaGrid[i,j].transform.position, 0.25f);
            }
        }
    }

    private int CalcCost(Vector2Int a, Vector2Int b)
    {
        var cubeA = AxialToCube(a);
        var cubeB = AxialToCube(b);

        return Mathf.RoundToInt(CubeDistance(cubeA, cubeB));
    }

    public List<Vector3> PathFinding(Vector2Int start, Vector2Int end)
    {
        List<Vector3> paths = new List<Vector3>();

        int[] oddDirX = { -1, -1, -1, 0, 1, 0 };       //홀수
        int[] evenDirX = { 0, -1, 0, 1, 1, 1 };     //짝수
        int[] dirY = { 1, 0, -1, -1, 0, 1 };
        int[] cost = { 7, 10, 7, 7, 10, 7 };

        bool[,] closed = new bool[_currentMapY, _currentMapX];
        int[,] open = new int[_currentMapY, _currentMapX];

        for (int i = 0; i < _currentMapY; i++)
            for (int j = 0; j < _currentMapX; j++)
                open[i, j] = int.MaxValue;

        Vector2Int[,] parent = new Vector2Int[_currentMapX, _currentMapY];
        PriorityQueue<Tile> priorityQueue = new PriorityQueue<Tile>();

        open[start.y, start.x] = CalcCost(start,end);
        priorityQueue.Push(new Tile { G = 0, H = open[start.y, start.x], index = new Vector2Int(start.x,start.y) });
        parent[start.y, start.x] = new Vector2Int(start.x, start.y);


        while (priorityQueue.Count > 0)
        {
            Tile t = priorityQueue.Pop();

            if (closed[t.index.y, t.index.x])
                continue;

            closed[t.index.y, t.index.x] = true;

            if(t.index == end)
            {
                break;
            }

            for (int i = 0; i < cost.Length; i++)
            {
                var next = t.index + new Vector2Int(t.index.y % 2 == 0 ? evenDirX[i] : oddDirX[i], dirY[i]);

                if (next.x < 0 || next.x >= mapX || next.y < 0 || next.y >= mapY)
                    continue;
                if (closed[next.y, next.x])
                    continue;
                int g = t.G + cost[i];
                int h = CalcCost(next, end);

                if (open[next.y, next.x] < g + h)
                    continue;

                open[next.y, next.x] = g + h;
                priorityQueue.Push(new Tile { G = g, H = h, index = next });
                parent[next.y, next.x] = new Vector2Int(t.index.x, t.index.y);
            }
        }

        CalcPathFormParent(parent, end, paths);

        return paths;
    }

    public void CalcPathFormParent(Vector2Int[,] parent, Vector2Int end, List<Vector3> paths)
    {
        Vector2Int current = new Vector2Int(end.x, end.y);

        while (parent[current.y, current.x].y != current.y || parent[current.y, current.x].x != current.x)
        {
            paths.Add(hexaGrid[current.y, current.x].transform.position);
            var newPos = parent[current.y, current.x];
            current.x = newPos.x;
            current.y = newPos.y;
        }
        paths.Add(hexaGrid[current.y, current.x].transform.position);
        paths.Reverse();
    }

    public Vector3Int AxialToCube(Vector2Int index)
    {
        return new Vector3Int(index.x, index.y, -index.x - index.y);
    }

    public float CubeDistance(Vector3Int a, Vector3Int b)
    {
        var vec = a - b;
        return Math.Max(Math.Abs(vec.x), Math.Max(Math.Abs(vec.y) ,Math.Abs(vec.z)));
    }

    public List<Vector2Int> RangeOfHexaGridIndex(Vector2Int center, int radius)
    {
        List<Vector2Int> indexList = new List<Vector2Int>();

        for (int i = -radius; i <= radius; i++)
            for (int j = Math.Max(-radius, -i - radius); j <= Math.Min(radius, -i + radius); j++)
            {
                Debug.Log($"{center.x + i} : {center.y + j}");
                indexList.Add(center + new Vector2Int(i, j));
            }
             

        return indexList;
    }
}

public struct Tile : IComparable<Tile>
{
    public int G;
    public int H;
    public int F => G + H;
    public Vector2Int index;

    public int CompareTo(Tile other)
    {
        if (F == other.F)
            return 0;
        return F > other.F ? 1 : -1;
    }
}
