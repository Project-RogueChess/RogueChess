using JMK.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HexaUnitManager : MonoBehaviour
{
    public static HexaUnitManager instance;

    public List<HexaUnit> unitList;

    public Vector3[,] positionMap => TilemapManager.instance.hexa_tilePosList;
    public bool[,] collisionMap = new bool[MAX_MAP_Y,MAX_MAP_X];
    public Camera mainCam;

    private const int MAX_MAP_X = 8;
    private const int MAX_MAP_Y = 8;

    [SerializeField] HexaUnit debugPrefab;
    [SerializeField] private bool selectStart;
    [SerializeField] private bool selectEnd;
    [SerializeField] private Vector2Int selectStartIndex;
    [SerializeField] private Vector2Int selectEndIndex;

    [SerializeField] GameObject[] loadPath;
    [SerializeField] GameObject loadTiles;
    [SerializeField] int range;

    void Awake()
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

    private void FixedUpdate()
    {
        var updateUnitList = new List<HexaUnit>();

        foreach (var u in unitList)
        {
            if (u.needUpdate)
                updateUnitList.Add(u);
        }

        HexaUnitUpdate(updateUnitList);
    }

    public void HexaUnitUpdate(List<HexaUnit> units)
    {
        //유닛리스트
       
        foreach (var u in units)
        {
            Dictionary<HexaUnit, int> distList = new Dictionary<HexaUnit, int>();

            int closestDist = int.MaxValue;
            HexaUnit closestUnit = new HexaUnit();

            foreach(var other in unitList)
            {
                if (other == u)
                    continue;

                var currnetDist = CalcDist(u.gridIndex, other.gridIndex);
                if (currnetDist < closestDist)
                {
                    closestDist = currnetDist;
                    closestUnit = other;
                }
            }
        }
    }

    private void Update()
    {
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore) 
            && hitInfo.transform.TryGetComponent(out TilemapTriggerInfo tInfo))
        {
            var tileIndex = new Vector2Int(tInfo.x, tInfo.y);

            //시작 지점 지정
            if (Input.GetKeyDown(KeyCode.Z) && !selectStart)
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

                for (int i = 0; i < loadPath.Length; i++)
                {
                    loadPath[i] = Instantiate(loadTiles);
                    loadPath[i].transform.position = positionMap[currentList[i].y, currentList[i].x];
                }
            }
        }

        if (selectStart && selectEnd)
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

            foreach (var path in pathlist)
            {
                loadPath[addCount] = Instantiate(loadTiles);
                loadPath[addCount].transform.position = positionMap[path.y, path.x];
                addCount++;
            }
        }
    }

    private int CalcDist(Vector2Int a, Vector2Int b)
    {
        var axialA = EvenToAxial(a);
        var axialB = EvenToAxial(b);

        return AxialDistance(axialA, axialB);
    }

    public List<Vector2Int> PathFinding(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> paths = new List<Vector2Int>();

        int[] oddDirX = { -1, -1, -1, 0, 1, 0 };    //홀수
        int[] evenDirX = { 0, -1, 0, 1, 1, 1 };     //짝수
        int[] dirY = { 1, 0, -1, -1, 0, 1 };
        int[] cost = { 7, 10, 7, 7, 10, 7 };

        bool[,] closed = new bool[MAX_MAP_Y, MAX_MAP_X];
        int[,] open = new int[MAX_MAP_Y, MAX_MAP_X];

        for (int i = 0; i < MAX_MAP_Y; i++)
            for (int j = 0; j < MAX_MAP_X; j++)
                open[i, j] = int.MaxValue;

        Vector2Int[,] parent = new Vector2Int[MAX_MAP_Y, MAX_MAP_X];
        PriorityQueue<PathNode> priorityQueue = new PriorityQueue<PathNode>();

        open[start.y, start.x] = CalcDist(start, end);
        priorityQueue.Push(new PathNode { G = 0, H = open[start.y, start.x], index = new Vector2Int(start.x, start.y) });
        parent[start.y, start.x] = new Vector2Int(start.x, start.y);


        while (priorityQueue.Count > 0)
        {
            PathNode t = priorityQueue.Pop();

            if (closed[t.index.y, t.index.x])
                continue;

            closed[t.index.y, t.index.x] = true;

            if (t.index == end)
            {
                break;
            }

            for (int i = 0; i < cost.Length; i++)
            {
                var next = t.index + new Vector2Int(t.index.y % 2 == 0 ? evenDirX[i] : oddDirX[i], dirY[i]);

                if (next.x < 0 || next.x >= MAX_MAP_X || next.y < 0 || next.y >= MAX_MAP_Y)
                    continue;
                if (closed[next.y, next.x])
                    continue;
                if (collisionMap[next.y, next.x])
                    continue;
                int g = t.G + cost[i];
                int h = CalcDist(next, end);

                if (open[next.y, next.x] < g + h)
                    continue;

                open[next.y, next.x] = g + h;
                priorityQueue.Push(new PathNode { G = g, H = h, index = next });
                parent[next.y, next.x] = new Vector2Int(t.index.x, t.index.y);
            }
        }

        Vector2Int current = new Vector2Int(end.x, end.y);

        while (parent[current.y, current.x].y != current.y || parent[current.y, current.x].x != current.x)
        {
            paths.Add(new Vector2Int(current.x, current.y));
            var newPos = parent[current.y, current.x];
            current.x = newPos.x;
            current.y = newPos.y;
        }
        paths.Add(new Vector2Int(current.x, current.y));
        paths.Reverse();

        return paths;
    }

    public void RegisterHexaUnit(HexaUnit unit)
    {
        unitList.Add(unit);
    }

    public void UnRegisterHexaUnit(HexaUnit unit)
    {
        unitList.Remove(unit);
    }

    public int AxialDistance(Vector2Int a, Vector2Int b)
    {
        var vec = a - b;
        return (Math.Abs(vec.x) + Math.Abs(vec.x + vec.y) + Math.Abs(vec.y)) / 2;
    }

    public List<Vector2Int> RangeOfHexaGridIndex(Vector2Int center, int radius)
    {
        center = EvenToAxial(center);

        List<Vector2Int> indexList = new List<Vector2Int>();

        for (int i = -radius; i <= radius; i++)
            for (int j = Math.Max(-radius, -i - radius); j <= Math.Min(radius, -i + radius); j++)
            {
                var cal = AxialToEven(center + new Vector2Int(i, j));
                if (cal.x >= 0 && cal.x < MAX_MAP_X && cal.y >= 0 && cal.y < MAX_MAP_Y)
                    indexList.Add(cal);
            }
        //중복 인덱스 제거
        indexList.Distinct().ToList();
        return indexList;
    }

    Vector2Int AxialToEven(Vector2Int input)
    {
        var col = input.x + (input.y + (input.y & 1)) / 2;
        var row = input.y;
        return new Vector2Int(col, row);
    }

    Vector2Int EvenToAxial(Vector2Int input)
    {
        var q = input.x - (input.y + (input.y & 1)) / 2;
        var r = input.y;
        return new Vector2Int(q, r);
    }
}


public struct PathNode : IComparable<PathNode>
{
    public int G;
    public int H;
    public int F => G + H;
    public Vector2Int index;

    public int CompareTo(PathNode other)
    {
        if (F == other.F)
            return 0;
        return F > other.F ? 1 : -1;
    }
}