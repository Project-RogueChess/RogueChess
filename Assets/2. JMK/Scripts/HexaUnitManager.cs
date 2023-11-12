using JMK.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class HexaUnitManager : MonoBehaviour
{
    public static HexaUnitManager instance;

    public List<HexaUnit> unitList;

    public string cubeString = "Cube";

    public Vector3[,] positionMap => TilemapManager.instance.hexa_tilePosList;
    public bool[,] collisionMap = new bool[MAX_MAP_Y, MAX_MAP_X];
    public Camera mainCam;

    public bool excuteUnitControll = false;

    private const int MAX_MAP_X = 8;
    private const int MAX_MAP_Y = 8;

    [SerializeField] HexaUnit debugUnit01;
    [SerializeField] HexaUnit debugUnit02;
    [SerializeField] private bool selectStart;
    [SerializeField] private bool selectEnd;
    [SerializeField] private Vector2Int selectStartIndex;
    [SerializeField] private Vector2Int selectEndIndex;

    [SerializeField] GameObject[] loadPath;
    [SerializeField] GameObject loadTiles;
    [SerializeField] int range;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
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
        if (!excuteUnitControll)
            return;

        var updateUnitList = new List<HexaUnit>();

        foreach (var u in unitList)
        {
            if (u.needUpdate)
                updateUnitList.Add(u);
        }

        HexaUnitUpdate(updateUnitList);
    }

    private void Update()
    {
        Debug_HexatileTool();
        Debug_UnitControll();
    }

    public void HexaUnitUpdate(List<HexaUnit> units)
    {
        //?¿½?¿½?¿½?¿½?¿½?¿½Æ®?¿½?¿½ ?¿½Ê¿?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½Ï³?¿½?¿½?¿½ ?¿½?¿½?¿½
        foreach (var u in units)
        {
            //?¿½?¿½?¿½?¿½?¿½ ?¿½æµ¹ ?¿½Îµ?¿½?¿½?¿½ ?¿½Ê±?¿½È­
            if (u.preIndex.x != -1)
            {
                collisionMap[u.preIndex.y, u.preIndex.x] = false;
                u.SetTileIndex(new Vector2Int(-1, -1), true);
            }

            if (u.tileIndex.x < 0 || u.tileIndex.x >= MAX_MAP_X || u.tileIndex.y < 0 || u.tileIndex.y >= MAX_MAP_Y)
                Debug.Log($"{u.tileIndex.x} : {u.tileIndex.y}");
            collisionMap[u.tileIndex.y, u.tileIndex.x] = true;

            Dictionary<HexaUnit, int> distDic = new Dictionary<HexaUnit, int>();

            //Å¸?¿½?¿½?¿½?¿½ ?¿½Ö´Â°?¿½?¿½ -> ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½ß¾?¿½?¿½?¿½
            //1. Å¸?¿½?¿½?¿½?¿½ ?¿½×¾?¿½?¿½?¿½ ?¿½?¿½.
            //2. Å¸?¿½?¿½?¿½?¿½ ?¿½Ìµ?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½ß´?¿½?¿½?¿½.

            //?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½Ê´Â´Ù¸?¿½ ?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½Øµ?¿½ ?¿½?¿½. ?¿½?¿½?¿½Ý½?¿½?¿½?¿½
            //?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½ß´Ù¸?¿½ Å¸?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½Ø¾?¿½?¿½?¿½ -> Å¸?¿½?¿½?¿½?¿½ NULL?¿½?¿½ ?¿½Ù²Ù°?¿½ ?¿½Ð½?¿½?¿½?¿½?¿½Îµ?¿½

            if (u.target != null && u.lastTargetIndex == u.target.tileIndex)
            {
                u.Attack();
                continue;
            }
            
            //?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½ ?¿½?¼±?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½Æ® ?¿½?¿½?¿½?¿½
            foreach (var other in unitList)
            {
                if (other == u || other.team == u.team)
                    continue;

                distDic.Add(other, CalcDist(u.tileIndex, other.tileIndex));
            }

            distDic = distDic.OrderBy(item => item.Value).ToDictionary(x => x.Key, x => x.Value);
            var distList = new Queue<HexaUnit>();

            foreach (var key in distDic.Keys)
                distList.Enqueue(key);

            //Ã¹?¿½?¿½Â° Ã¼Å© - ?¿½?¿½?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½Ã£?¿½?¿½
            var firstCheck = false;

            //?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½Ã£?¿½?¿½
            while (distList.Count > 0)
            {
                var currentTarget = distList.Dequeue();

                //?¿½?¿½?¿½?¿½?¿½Å¸?¿½ ?¿½?¿½?¿½
                var rangeTile = RangeOfHexaGridIndex(currentTarget.tileIndex, u.range + 1);
                if (rangeTile.Contains(u.tileIndex))
                {
                    //?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½È¯
                    u.SetTarget(currentTarget);
                    u.Attack();
                    firstCheck = true;
                    break;
                }

                //?¿½?¿½?¿½?¿½?¿½ Ã¼Å©
                var ringIndex = RingOfHexaGridIndex(currentTarget.tileIndex, u.range + 1);
                var inUseIndexCount = 0;

                foreach (var idx in ringIndex)
                    if (collisionMap[idx.y, idx.x])
                        inUseIndexCount++;

                if (inUseIndexCount == ringIndex.Count)
                {
                    //?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½, ?¿½?¿½?¿½?¿½ ?¿½?¿½ È®?¿½?¿½
                    continue;
                }

                //?¿½æµ¹?¿½?¿½ ?¿½?¿½?¿½?¿½
                var tileTemp = collisionMap.Clone();
                collisionMap[currentTarget.tileIndex.y, currentTarget.tileIndex.x] = false;


                //?¿½?¿½?¿½?¿½?¿½Å¸?¿½ ?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½Å¸?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½æµ¹?¿½?¿½ ?¿½?¿½?¿½
                if (u.range > 0)
                {
                    rangeTile = RangeOfHexaGridIndex(currentTarget.tileIndex, u.range);

                    foreach (var t in rangeTile)
                    {
                        collisionMap[t.y, t.x] = false;
                    }
                }

                var pathTile = PathFinding(u.tileIndex, currentTarget.tileIndex);
               
                if (pathTile.Count > 0)
                {
                    //?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½Ìµ?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½È¯
                    pathTile.RemoveAt(0);
                    u.Move(pathTile[0]);
                    firstCheck = true;
                    collisionMap = (bool[,])tileTemp;
                    collisionMap[pathTile[0].y, pathTile[0].x] = true;
                    break;
                }

                collisionMap = (bool[,])tileTemp;
            }

            //?¿½?¿½Ã£?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½Æ®?¿½?¿½ ?¿½Ê¿?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½?¿½?¿½
            if (firstCheck || distDic.Count == 0)
                continue;

            var secondCheckTarget = distDic.FirstOrDefault().Key;
            var secondTileTemp = collisionMap.Clone();

            //?¿½æµ¹?¿½?¿½ ?¿½?¿½?¿½?¿½
            var secondRangeTile = RangeOfHexaGridIndex(secondCheckTarget.tileIndex, u.range + 1);
            collisionMap[secondCheckTarget.tileIndex.y, secondCheckTarget.tileIndex.x] = false;

            foreach (var t in secondRangeTile)
            {
                collisionMap[t.y, t.x] = false;
            }

            var secondPathTile = PathFinding(u.tileIndex, secondCheckTarget.tileIndex);
            if(secondPathTile.Count > 0)
                secondPathTile.RemoveAt(0);
            collisionMap = (bool[,])secondTileTemp;

            if (secondPathTile.Count > 0 && !collisionMap[secondPathTile[0].y, secondPathTile[0].x])
            {
                //?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½Ìµ?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½È¯
                u.Move(secondPathTile[0]);
                collisionMap[secondPathTile[0].y, secondPathTile[0].x] = true;
            }
            else
            {
                //6?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½Å¸?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½?¿½?¿½ ?¿½Ìµ?¿½ (?¿½?¼±?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½Æ® ?¿½Û¼?¿½)
                PriorityQueue<PathNode> checkTiles = new PriorityQueue<PathNode>();
                List<Vector2Int> neighborTile = RingOfHexaGridIndex(u.tileIndex, 1);

                foreach(var tile in neighborTile)
                {
                    PathNode pqNode = new PathNode();
                    pqNode.index = tile;
                    pqNode.G = 0;
                    pqNode.H = CalcDist(tile, secondCheckTarget.tileIndex);
                    checkTiles.Push(pqNode);
                }

                while(checkTiles.Count > 0)
                {
                    var currentTile = checkTiles.Pop();
                    if(!collisionMap[currentTile.index.y, currentTile.index.x])
                    {
                        u.Move(currentTile.index);
                        collisionMap[currentTile.index.y, currentTile.index.x] = true;
                        break;
                    }
                }
            }

            u.SetTarget(null);
        }
    }


    public void RegisterHexaUnit(HexaUnit unit)
    {
        unitList.Add(unit);
    }

    public void UnRegisterHexaUnit(HexaUnit unit)
    {
        if (unit.preIndex.x != -1)
        {
            collisionMap[unit.preIndex.y, unit.preIndex.x] = false;
            unit.SetTileIndex(unit.preIndex);
        }

        collisionMap[unit.tileIndex.y, unit.tileIndex.x] = false;
        unitList.Remove(unit);
    }

    public List<Vector2Int> PathFinding(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> paths = new List<Vector2Int>();

        int[] oddDirX = { -1, -1, -1, 0, 1, 0 };    //È¦?¿½?¿½
        int[] evenDirX = { 0, -1, 0, 1, 1, 1 };     //Â¦?¿½?¿½
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

        var isValidPath = false;

        while (priorityQueue.Count > 0)
        {
            PathNode t = priorityQueue.Pop();

            if (closed[t.index.y, t.index.x])
                continue;

            closed[t.index.y, t.index.x] = true;

            if (t.index == end)
            {
                isValidPath = true;
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

        if (!isValidPath)
            return paths;

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

    public int CalcDist(Vector2Int a, Vector2Int b)
    {
        var axialA = EvenToAxial(a);
        var axialB = EvenToAxial(b);

        return AxialDistance(axialA, axialB);
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
        //?¿½ßº?¿½ ?¿½Îµ?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½
        indexList.Distinct().ToList();
        return indexList;
    }

    public List<Vector2Int> RingOfHexaGridIndex(Vector2Int center, int radius)
    {
        center = EvenToAxial(center);

        List<Vector2Int> indexList = AxialRing(center, radius);

        List<Vector2Int> result = new List<Vector2Int>();

        foreach (var index in indexList)
        {
            var cal = AxialToEven(index);
            if (cal.x >= 0 && cal.x < MAX_MAP_X && cal.y >= 0 && cal.y < MAX_MAP_Y)
                result.Add(cal);
        }

        return result;
    }

    public Vector2Int[] axialDirVec = { new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) };

    public Vector2Int AxialToEven(Vector2Int input)
    {
        var col = input.x + (input.y + (input.y & 1)) / 2;
        var row = input.y;
        return new Vector2Int(col, row);
    }

    public Vector2Int EvenToAxial(Vector2Int input)
    {
        var q = input.x - (input.y + (input.y & 1)) / 2;
        var r = input.y;
        return new Vector2Int(q, r);
    }

    public int AxialDistance(Vector2Int a, Vector2Int b)
    {
        var vec = a - b;
        return (Math.Abs(vec.x) + Math.Abs(vec.x + vec.y) + Math.Abs(vec.y)) / 2;
    }

    public Vector2Int AxialDir(int dir) => axialDirVec[dir];

    public Vector2Int AxialNeighbor(Vector2Int axial, int dir) => axial + AxialDir(dir);

    public List<Vector2Int> AxialRing(Vector2Int center, int radius)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        var hex = center + AxialDir(4) * radius;

        for (int i = 0; i < 6; i++)
            for (int j = 0; j < radius; j++)
            {
                result.Add(hex);
                hex = AxialNeighbor(hex, i);
            }

        return result;
    }

    void Debug_UnitControll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            excuteUnitControll = !excuteUnitControll;
            TilemapManager.instance.hexa_tilemapPivot.gameObject.SetActive(!excuteUnitControll);
            TilemapManager.instance.inv_tilemapPivot.gameObject.SetActive(!excuteUnitControll);
        }
            

        if (Input.GetKeyDown(KeyCode.Y))
            Debug_GenerateUnit();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            var deleteList = new List<HexaUnit>();

            foreach(var u in unitList)
            {
                if (u.team == 1)
                {
                    deleteList.Add(u);
                    break;
                }
            }

            collisionMap[deleteList[0].tileIndex.y, deleteList[0].tileIndex.x] = false;
            if (deleteList[0].preIndex.x != -1)
                collisionMap[deleteList[0].preIndex.y, deleteList[0].preIndex.x] = false;

            unitList.Remove(deleteList[0]);

            Destroy(deleteList[0].gameObject);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            var GOArray = new GameObject[unitList.Count];
            var idx = 0;
            foreach (var r in unitList)
            {
                GOArray[idx++] = r.gameObject;
                collisionMap[r.tileIndex.y, r.tileIndex.x] = false;
                if (r.preIndex.x != -1)
                    collisionMap[r.preIndex.y, r.preIndex.x] = false;
            }


            for (int i = 0; i < GOArray.Length; i++)
                Destroy(GOArray[i]);

            unitList.Clear();
        }

        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore)
            && hitInfo.transform.TryGetComponent(out TilemapTriggerInfo tInfo))
        {
            var tileIndex = new Vector2Int(tInfo.x, tInfo.y);

            var indexList = new List<Vector2Int>();
            foreach (var item in unitList)
                indexList.Add(item.tileIndex);

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (indexList.Contains(tileIndex))
                    return;
                var unitGO = Instantiate(debugUnit01);
                unitGO.SetTileIndex(tileIndex);
                unitGO.transform.position = positionMap[tileIndex.y, tileIndex.x];
                RegisterHexaUnit(unitGO);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (indexList.Contains(tileIndex))
                    return;
                var unitGO = Instantiate(debugUnit02);
                unitGO.SetTileIndex(tileIndex);
                unitGO.transform.position = positionMap[tileIndex.y, tileIndex.x];
                RegisterHexaUnit(unitGO);
            }

            if (Input.GetMouseButton(1))
            {
                var unitGO = new GameObject();
                foreach (var u in unitList)
                {
                    if (u.tileIndex == tileIndex)
                        unitGO = u.gameObject;
                }

                UnRegisterHexaUnit(unitGO.GetComponent<HexaUnit>());
                Destroy(unitGO.gameObject);
            }
        }
    }
    void Debug_GenerateUnit()
    {
        var tileIndex = new Vector2Int(UnityEngine.Random.Range(0,MAX_MAP_X), UnityEngine.Random.Range(4, MAX_MAP_Y));

        var indexList = new List<Vector2Int>();
        foreach (var item in unitList)
            indexList.Add(item.tileIndex);

        if (indexList.Count >= 32)
            return;

        if (indexList.Contains(tileIndex))
        {
            Debug_GenerateUnit();
            return;
        }

        var unitGO = Instantiate(debugUnit02);
        unitGO.SetTileIndex(tileIndex);
        unitGO.transform.position = positionMap[tileIndex.y, tileIndex.x];
        unitGO.transform.forward = Vector3.back;
        collisionMap[tileIndex.y, tileIndex.x] = true;
        RegisterHexaUnit(unitGO);
    }

    void Debug_HexatileTool()
    {
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore)
            && hitInfo.transform.TryGetComponent(out TilemapTriggerInfo tInfo))
        {
            var tileIndex = new Vector2Int(tInfo.x, tInfo.y);

            //?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½
            if (Input.GetKeyDown(KeyCode.Z) && !selectStart)
            {
                selectStart = true;
                selectStartIndex = tileIndex;
            }

            //?¿½?¿½ ?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½
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

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (loadPath != null)
                {
                    foreach (var obj in loadPath)
                        Destroy(obj);
                    loadPath = null;
                }

                var currentList = RingOfHexaGridIndex(tileIndex, range + 1);

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
            //Æ®?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½
            selectStart = false;
            selectEnd = false;

            //?¿½?¿½?¿½?¿½ ?¿½?¿½ Ç¥?¿½Ã´?¿½ ?¿½?¿½?¿½?¿½
            if (loadPath != null)
            {
                foreach (var obj in loadPath)
                    Destroy(obj);
                loadPath = null;
            }

            //?¿½?¿½ Ã£?¿½?¿½
            var pathlist = PathFinding(selectStartIndex, selectEndIndex);

            //?¿½?¿½ Ã£?¿½?¿½?¿½?¿½ ?¿½?¿½?¿½?¿½
            if (pathlist.Count == 0)
                return;

            //?¿½?¿½ Ç¥?¿½?¿½ ?¿½?¿½?¿½?¿½?¿½?¿½Æ® ?¿½?¿½?¿½?¿½
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