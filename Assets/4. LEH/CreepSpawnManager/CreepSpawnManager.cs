using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CreepSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject shellOfCreep;
    [SerializeField] private Transform creepPool;

    [SerializeField] private int CreepPoolSize = 10;

    [SerializeField] string CreepDB_CSV_Path = "CreepDB";

    private Dictionary<int, Creep> creepClassDict = new Dictionary<int, Creep>();// CSV reader로 생성된 creep

    private void Awake()
    {
        ReadCreepData(CreepDB_CSV_Path, creepClassDict);
        PoolingCreeps();
    }
    private void Start()
    {
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

            creepObject.avatarPath = Resources.Load<Avatar>("CreepsAvatar/" + dictionaryOfCreepData[i]["avatarPath"]);
            creepObject.animatorPath = Resources.Load<Animator>("CreepsAvatar/" + dictionaryOfCreepData[i]["animatorPath"]);

            creepClassDict.Add(creepObject.id, creepObject);
        }
    }

    private void PoolingCreeps()
    {
        List<GameObject> poolOfCreeps = new List<GameObject>();

        for (int i = 0; i < CreepPoolSize; i++)
        {
            GameObject realCreep = Instantiate(shellOfCreep, creepPool);
            realCreep.SetActive(false);
            poolOfCreeps.Add(realCreep);
        }
    }
}


