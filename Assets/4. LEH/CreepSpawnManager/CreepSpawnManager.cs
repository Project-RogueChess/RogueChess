using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor.SceneManagement;
using UnityEngine;

스테이지, 크립 직업, x, y // 크립 능력치
크립 모든 종류 읽어서 리스트에 추가
0 전사 1 원딜
껍데기에 크립 데이터 넣었고
풀링해서 리스트에 넣은 다음에 스폰 만들기.

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


