using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;



public class CreepSpawnManager : MonoBehaviour
{
    public static CreepSpawnManager instance;

    [SerializeField] private GameObject shellOfCreep;
    [SerializeField] private List<GameObject> creepPool;
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
            DontDestroyOnLoad(this);
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
    private void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadCreep(3);
        }
    }

    public void LoadCreep(int stageID)
    {
        List<StageData> currentStage = SearchStageWithID(stageID, stageList);

        foreach(var stageData in currentStage)
        {
            if(creepPool.Count > 0)
            {
                var creep = InjectCreepData(creepPool[0], stageData.creepID).GetComponent<CreepComponent>();
                var unit = creep.GetComponent<HexaUnit>();
                unit.team = 1;
                unit.atkRate = (int)creep.attackSpeed;
                unit.moveRate = (int)creep.moveSpeed;
                unit.range = creep.attackRange;
                if(unit.range > 0)
                    unit.projectilePrefab = ((GameObject)Resources.Load("UnitPrefab/ProjectileSample")).GetComponent<HexaUnitProjectile>();
                unit.SetTileIndex(new Vector2Int(stageData.x, stageData.y));
                unit.transform.position = TilemapManager.instance.hexa_tilePosList[unit.tileIndex.y, unit.tileIndex.x];
                creep.gameObject.SetActive(true);
                HexaUnitManager.instance.RegisterHexaUnit(unit);

                creepPool.RemoveAt(0);
            }
        }
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

            creepObject.avatarPath = "CreepsAvatar/" + dictionaryOfCreepData[i]["avatarPath"];
            creepObject.animatorPath = "CreepsAvatar/" + dictionaryOfCreepData[i]["animatorPath"];

            creepDict.Add(creepObject.id, creepObject);
        }
    }

    public GameObject InjectCreepData(GameObject go, int creepId)
    {
        var creep = go.GetComponent<CreepComponent>();

        creep.id = creepId;
        creep.creepName = creepClassDict[creepId].name;
        creep.maxHp = creepClassDict[creepId].maxHp;
        creep.hp = creepClassDict[creepId].hp;
        creep.attackDamage = creepClassDict[creepId].attackDamage;
        creep.attackSpeed = creepClassDict[creepId].attackSpeed;
        creep.attackRange = creepClassDict[creepId].attackRange;
        creep.moveSpeed = creepClassDict[creepId].moveSpeed;

        creep.avatar = (Avatar)Resources.Load(creepClassDict[creepId].avatarPath);
        creep.animator = (Animator)Resources.Load(creepClassDict[creepId].animatorPath);

        return go;
    }

    private List<GameObject> PoolingCreeps()
    {
        List<GameObject> poolOfCreeps = new List<GameObject>();

        for (int i = 0; i < CreepPoolSize; i++)
        {
            GameObject realCreep = Instantiate(shellOfCreep, creepPoolParent);
            realCreep.SetActive(false);
            poolOfCreeps.Add(realCreep);
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


