using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossUnit : HexaUnit
{
    public BossMeteor meteorPrefab;
    public Transform tileIndicator;

    private bool _skillDirty = false;
    private bool _invincibility = false;
    private BossMeteor[] _bossMeteors;
    private HexaUnit _summon;
    private bool _playingDie = false;
    
    
    public override bool needUpdate => !_moveDirty && !_atkDirty && !_skillDirty;

    private void Update()
    {
        Act();
        if(article.hp <= 0)
        {
            _invincibility = false;
            Die();
        }
    }
    public override void Damaged(int damage)
    {
        if (_playingDie || _invincibility)
            return;

        article.hp -= damage;
        if (article.hp <= 0)
            Die();
    }

    public override void Die()
    {
        _playingDie = true;
        if(_summon != null && _summon.gameObject.activeSelf)
            _summon.Die();
        base.Die();
    }

    public override void Attack()
    {
        //디버그용 스타트 어택
        if (article.mp == article.maxMp)
        {
            article.mp = 0;
            _skillDirty = true;

            //분기 행동(코루틴)
            var randomSkill = Random.Range(0, 3);
            switch (randomSkill)
            {
                case 0:
                    StartCoroutine("Skill_Teleport");
                    break;
                case 1:
                    StartCoroutine("Skill_Meteor");
                    break;
                case 2:
                    StartCoroutine("Skill_Summoning");
                    break;
            }

            
            return;
        }

        article.mp = math.min(article.mp + 10, article.maxMp);
        _lastTargetIndex = target.tileIndex;
        
        _atkDirty = true;
        _startAtk = true;

        //터닝이 필요한 경우
        var lastDirIndex = _savedDirIndex;
        _savedDirIndex = HexaUnitManager.instance.EvenToAxial(target.tileIndex) - HexaUnitManager.instance.EvenToAxial(_tileIndex);
        if (lastDirIndex != _savedDirIndex)
        {
            _hasTurn = true;
            _savedRot = transform.rotation;
        }
        _startPos = TilemapManager.instance.hexa_tilePosList[tileIndex.y, tileIndex.x];
        _endPos = TilemapManager.instance.hexa_tilePosList[target.tileIndex.y, target.tileIndex.x];
    }

    IEnumerator Skill_Teleport()
    {
        _target = null;
        HexaUnitManager.instance.RequestSetColMapIndex(_tileIndex, false);
        _preIndex = new Vector2Int(-1, -1);
        _invincibility = true;

        article.animator.Play("Victory", -1, 0f);
        article.animator.Update(0f);

        var timer = 0f;
        var lastTileIndex = _tileIndex;

        var blackHole = Instantiate((GameObject)Resources.Load("CreepPrefabs/CreepFX/BossBlackhole")).GetComponent<ControlBlackHole>();
        blackHole.transform.position = transform.position;
        blackHole.transform.localScale = Vector3.one * 1.5f;
        blackHole.StartMotion();
        while (timer < blackHole.motion.keys[blackHole.motion.keys.Length - 1].time + 0.05f)
        {
            timer += Time.deltaTime;
            if (timer > 1f && _tileIndex == lastTileIndex)
            {
                _tileIndex = new Vector2Int(-1, -1);
                ((CreepComponent)article).rootTransform.gameObject.SetActive(false);
            }
               
            yield return null;
        }
        timer = 0f;

        var validTileList = new List<Vector2Int>();

        for(int i = 0; i < HexaUnitManager.MAX_MAP_Y; i++)
        {
            for(int j = 0; j < HexaUnitManager.MAX_MAP_X; j++)
            {
                if (!HexaUnitManager.instance.collisionMap[i, j])
                    validTileList.Add(new Vector2Int(j, i));
            }
        }

        var randomIndex = Random.Range(0, validTileList.Count);
        _tileIndex = validTileList[randomIndex];
        HexaUnitManager.instance.RequestSetColMapIndex(_tileIndex, true);
        transform.position = TilemapManager.instance.hexa_tilePosList[_tileIndex.y,_tileIndex.x];

        blackHole.transform.position = transform.position;
        blackHole.StartMotion();
        while (timer < 3f)
        {
            timer += Time.deltaTime;
            if (timer > 1f && _invincibility)
            {
                _invincibility = false;
                ((CreepComponent)article).rootTransform.gameObject.SetActive(true);
            }
            yield return null;
        }

        Destroy(blackHole.gameObject);
        _skillDirty = false;
    }

    IEnumerator Skill_Meteor()
    {
        //최초 생성
        if(_bossMeteors == null)
        {
            var meteorParent = new GameObject();
            meteorParent.transform.parent = transform;
            meteorParent.name = "MeteorPool";

            _bossMeteors = new BossMeteor[3];
            for(int i = 0; i < _bossMeteors.Length; i++)
            {
                _bossMeteors[i] = Instantiate(meteorPrefab).GetComponent<BossMeteor>();
                _bossMeteors[i].transform.parent = meteorParent.transform;
                _bossMeteors[i].transform.localPosition = Vector3.zero;
                _bossMeteors[i].model.gameObject.SetActive(false);
                _bossMeteors[i].trail.enabled = false;
            }
        }
        var timer = 0f;
        _invincibility = true;

        article.animator.Play("Victory", -1, 0f);
        article.animator.Update(0f);

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;

        //위치 선정
        var attackIndex = new Vector2Int[]{ new Vector2Int(-1,-1), new Vector2Int(-1, -1), new Vector2Int(-1, -1)};

        var rangeIndex = HexaUnitManager.instance.RangeOfHexaGridIndex(target.tileIndex, 1);
        var ringRangeIndex1 = HexaUnitManager.instance.RingOfHexaGridIndex(target.tileIndex, 3);
        var ringRangeIndex2 = HexaUnitManager.instance.RingOfHexaGridIndex(target.tileIndex, 5);

        if (rangeIndex.Count > 0)
            attackIndex[0] = rangeIndex[Random.Range(0, rangeIndex.Count)];
        if (ringRangeIndex1.Count > 0)
            attackIndex[1] = ringRangeIndex1[Random.Range(0, ringRangeIndex1.Count)];
        if(ringRangeIndex2.Count > 0)
            attackIndex[2] = ringRangeIndex2[Random.Range(0, ringRangeIndex2.Count)];

        var meteorIndex = 0;
       
        while(meteorIndex < 3)
        {
            timer += Time.deltaTime;
            if(timer > 0.6f)
            {
                timer = 0f;
                if (attackIndex[meteorIndex].x == -1)
                {
                    meteorIndex++;
                    continue;
                }

                _bossMeteors[meteorIndex].gameObject.SetActive(true);
                _bossMeteors[meteorIndex].Initialize(attackIndex[meteorIndex]);
                _bossMeteors[meteorIndex].FallDown();
                meteorIndex++;
            }
            yield return null;
        }


        article.animator.Play("Idle");

        _invincibility = false;

        while (timer < 4f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _skillDirty = false;
    }

    IEnumerator Skill_Summoning()
    {
        var selectIndex = new Vector2Int(-1, -1);
        var ringIndex = HexaUnitManager.instance.RingOfHexaGridIndex(_tileIndex, 1);
        if (ringIndex.Count > 0)
            selectIndex = ringIndex[Random.Range(0, ringIndex.Count)];

        if (selectIndex.x == -1)
        {
            var inCount = Random.Range(0, 2);
            switch (inCount)
            {
                case 0:
                    StartCoroutine("Skill_Meteor");
                    break;
                case 1:
                    StartCoroutine("Skill_Teleport");
                    break;
            }
            yield break;
        }

        article.animator.Play("Victory", -1, 0f);
        article.animator.Update(0f);

        HexaUnitManager.instance.RequestSetColMapIndex(selectIndex, true);
        tileIndicator.position = TilemapManager.instance.hexa_tilePosList[selectIndex.y,selectIndex.x];
        tileIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        tileIndicator.gameObject.SetActive(true);
        var timer = 0f;
        while(timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;


        var blackHole = Instantiate((GameObject)Resources.Load("CreepPrefabs/CreepFX/BossBlackhole")).GetComponent<ControlBlackHole>();
        blackHole.transform.position = TilemapManager.instance.hexa_tilePosList[selectIndex.y,selectIndex.x];
        blackHole.transform.localScale = Vector3.one * 0.2f;
        blackHole.motionSpeed = 2.5f;
        blackHole.StartMotion();

        var checkValidUnit = new KeyValuePair<bool,HexaUnit>(false, null);
        foreach (var u in HexaUnitManager.instance.unitList)
        {
            //버그 조심
            if (selectIndex == u.tileIndex)
            {
                checkValidUnit = new KeyValuePair<bool, HexaUnit>(true, u);
                break;
            }
            else if(selectIndex == u.preIndex)
            {
                checkValidUnit = new KeyValuePair<bool, HexaUnit>(true, null);
                break;
            }
        }


        if ((_summon != null && HexaUnitManager.instance.unitList.Contains(_summon)) || (checkValidUnit.Key && checkValidUnit.Value != null))
        {
            //딜
            if (checkValidUnit.Value != null && checkValidUnit.Value.team == 0)
                checkValidUnit.Value.Damaged(10);

            HexaUnitManager.instance.RequestSetColMapIndex(selectIndex, checkValidUnit.Key);
        }
        else
        {
            _summon = CreepSpawnManager.instance.GetCreep(0).GetComponent<HexaUnit>();
            var creepInfo = _summon.GetComponent<CreepComponent>();

            creepInfo.buffData[0].maxHp = -50;
            creepInfo.buffData[0].attackDamage = -6;
            creepInfo.buffData[0].attackSpeed = -0.4f;

            creepInfo.hp = creepInfo.maxHp;
            creepInfo.mp = 0;

            _summon.ResetSavedValue();
            _summon.SetTileIndex(selectIndex);
            _summon.transform.forward = Vector3.back;
            _summon.transform.position = TilemapManager.instance.hexa_tilePosList[selectIndex.y, selectIndex.x];
            HexaUnitManager.instance.RegisterHexaUnit(_summon);
        }

        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        article.animator.Play("Idle");

        tileIndicator.gameObject.SetActive(false);
        Destroy(blackHole.gameObject);

        _skillDirty = false;
    }
}
