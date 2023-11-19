using System.Collections.Generic;
using UnityEngine;



public class CreepSpawnManager : MonoBehaviour
{
    public static CreepSpawnManager instance;

    [SerializeField] private GameObject shellOfCreep;
    [SerializeField] private List<List<GameObject>> creepPool;
    [SerializeField] private Transform creepPoolParent;

    [SerializeField] private int CreepPoolSize = 10;

    [SerializeField] string CreepDB_CSV_Path = "CreepDB";
    [SerializeField] string CreepStageDB_CSV_Path = "CreepStageDB";

    private Dictionary<int, Creep> creepClassDict = new Dictionary<int, Creep>();// CSV reader로 생성된 creep
    private List<StageData> stageList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                DestroyImmediate(this);
        }

        ReadCreepData(CreepDB_CSV_Path, creepClassDict);
        stageList = ReadStageData(CreepStageDB_CSV_Path);
        creepPool = PoolingCreeps();
    }

    public GameObject GetCreep(int id)
    {
        if (creepPool[id].Count > 0)
        {
            var creep = creepPool[id][0];
            creep.transform.parent = null;
            creep.SetActive(true);
            creepPool[id].RemoveAt(0);
            return creep;
        }
        else
        {
            var newCreep = CreateCreep(id);
            return newCreep;
        }
    }

    public void ReturnCreep(CreepComponent creep)
    {
        creepPool[creep.id].Add(creep.gameObject);
        creep.transform.parent = creepPoolParent.GetChild(creep.id);
        creep.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadCreepToField(3);
        }
    }

    public void LoadCreepToField(int stageID)
    {
        List<StageData> currentStage = SearchStageWithID(stageID, stageList);

        foreach(var stageData in currentStage)
        {
            var creepGO = GetCreep(stageData.creepID);

            var creepInfo = creepGO.GetComponent<CreepComponent>();
            creepInfo.hp = creepInfo.maxHp;
            creepInfo.mp = 0;

            var unit = creepGO.GetComponent<HexaUnit>();
            unit.ResetSavedValue();
            unit.SetTileIndex(new Vector2Int(stageData.x, stageData.y));
            unit.transform.forward = Vector3.back;
            unit.transform.position = TilemapManager.instance.hexa_tilePosList[unit.tileIndex.y, unit.tileIndex.x];
            HexaUnitManager.instance.RegisterHexaUnit(unit);
        }
    }

    public void LoadBossCreepToField()
    {
        //추가시간증정
        //GameManager.instance.time

        var parent = new GameObject();
        parent.name = "Creep_Boss_Pool";
        parent.transform.parent = creepPoolParent;
        var bossPool = new List<GameObject>();
        creepPool.Add(bossPool);

        var boss = Instantiate((GameObject)Resources.Load("CreepPrefabs/CustomCreeps/Creep_Boss"));
        boss.transform.forward = Vector3.back;

        var creep = boss.GetComponent<CreepComponent>();
        var model = Instantiate(creep.modelPrefab, creep.rootTransform);
        creep.animator = model.GetComponent<Animator>();

        var unit = boss.GetComponent<HexaUnit>();
        unit.SetTileIndex(new Vector2Int(4, 7));
        unit.transform.position = TilemapManager.instance.hexa_tilePosList[unit.tileIndex.y, unit.tileIndex.x];
        HexaUnitManager.instance.RegisterHexaUnit(unit);
    }

    public List<StageData> SearchStageWithID(int id, List<StageData> stageSource)
    {
        List<StageData> currentStage = new List<StageData>();

        foreach(var stage in stageSource)
        {
            if (stage.stageID == id)
                currentStage.Add(stage);
        }

        return currentStage;
    }

    public List<StageData> ReadStageData(string filename)
    {
        List<Dictionary<string, object>> dictionaryData = CSVReader.Read(filename);
        List<StageData> stageList = new List<StageData>();
        

        foreach(var data in dictionaryData)
        {
            StageData stage = new StageData();

            stage.stageID = (int)data["stage"];
            stage.creepID = (int)data["creep"];
            stage.x = (int)data["x"];
            stage.y = (int)data["y"];

            stageList.Add(stage);
        }

        return stageList;
    }

    private void ReadCreepData(string filename, Dictionary<int, Creep> creepDict)
    {
        List<Dictionary<string, object>> dictionaryOfCreepData = new List<Dictionary<string, object>>(); // 왜 리스트<사전<>> ?

        dictionaryOfCreepData = CSVReader.Read(filename);

        for (int i = 0; i < dictionaryOfCreepData.Count; i++)
        {
            Creep creepObject = new Creep();

            creepObject.name = dictionaryOfCreepData[i]["name"].ToString();
            creepObject.id = (int)dictionaryOfCreepData[i]["id"];
            creepObject.maxHp = (int)dictionaryOfCreepData[i]["maxHp"];
            creepObject.hp = (int)dictionaryOfCreepData[i]["hp"];
            creepObject.attackDamage = (int)dictionaryOfCreepData[i]["attackDamage"];
            creepObject.attackSpeed = (float)dictionaryOfCreepData[i]["attackSpeed"];
            creepObject.attackRange = (int)dictionaryOfCreepData[i]["attackRange"];
            creepObject.moveSpeed = (float)dictionaryOfCreepData[i]["moveSpeed"];

            creepObject.modelPrefab = (string)dictionaryOfCreepData[i]["modelPrefab"];
            creepObject.projectile = (string)dictionaryOfCreepData[i]["projectile"];

            creepDict.Add(creepObject.id, creepObject);
        }
    }

    public GameObject CreateCreep(int id)
    {
        var shell = Instantiate(shellOfCreep);

        var creep = shell.GetComponent<CreepComponent>();

        creep.id = id;
        creep.name = creepClassDict[id].name + "(Clone)";
        creep.creepName = creepClassDict[id].name;
        creep.originMaxHp = creepClassDict[id].maxHp;
        creep.hp = creepClassDict[id].hp;
        creep.originAttackDamage = creepClassDict[id].attackDamage;
        creep.originAttackRange = creepClassDict[id].attackRange;
        creep.originAttackSpeed = creepClassDict[id].attackSpeed;
        creep.originMoveSpeed = creepClassDict[id].moveSpeed;

        creep.modelPrefab = (GameObject)Resources.Load("CreepPrefabs/" + creepClassDict[id].modelPrefab);
        creep.projectile = creepClassDict[id].projectile != null ? (GameObject)Resources.Load("CreepPrefabs/CreepProjectiles/" + creepClassDict[id].projectile) : null;

        var model = Instantiate(creep.modelPrefab, creep.rootTransform);
        creep.animator = model.GetComponent<Animator>();

        var unit = creep.GetComponent<HexaUnit>();
        unit.article = creep;
        unit.team = 1;
        unit.range = creep.attackRange;
        if (creep.projectile != null)
            unit.projectilePrefab = (creep.projectile).GetComponent<HexaUnitProjectile>();

        return shell;
    }

    private List<List<GameObject>> PoolingCreeps()
    {
        List<List<GameObject>> poolOfCreeps = new List<List<GameObject>>();

        foreach(var id in creepClassDict.Keys)
        {
            var currentCreeps = new List<GameObject>();
            var parent = new GameObject();
            parent.name = creepClassDict[id].name + "_Pool";

            parent.transform.parent = creepPoolParent;

            for(int i = 0; i < CreepPoolSize; i++)
            {
                var currentCreep = CreateCreep(id);
                currentCreep.transform.parent = parent.transform;
                currentCreep.SetActive(false);
                currentCreeps.Add(currentCreep);
            }

            poolOfCreeps.Add(currentCreeps);
        }

        return poolOfCreeps;
    }
}

public class StageData
{
    public int stageID;
    public int creepID;
    public int x, y;
}


