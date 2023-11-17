using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class HexaUnit : MonoBehaviour
{
    //기본 정보
    public int team;
    public int range;
    public HexaUnitProjectile projectilePrefab;
    public GameObject attackFX;
    public Article article;

    //더티 셋
    private bool _moveDirty = false;
    private bool _atkDirty = false;
    private bool _hasTurn = false;

    //타겟 정보
    private HexaUnit _target;
    private Vector2Int _tileIndex;
    private Vector2Int _preIndex = new Vector2Int(-1,-1);
    private Vector2Int _lastTargetIndex;

    //저장 값
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Vector2Int _savedDirIndex;
    private Quaternion _savedRot;
    private bool _firstMove = true;
    private bool _startAtk = false;
    private HexaUnitProjectile _projectile;
    private bool _forceStop = false;

    //내부 타이머
    private float _actTimer = 0f;

    //속성
    public bool needUpdate => !_moveDirty && !_atkDirty;
    public Vector2Int tileIndex => _tileIndex;
    public Vector2Int preIndex => _preIndex;
    public Vector2Int lastTargetIndex => _lastTargetIndex;
    public float moveRate => article.moveSpeed;
    public float atkRate => article.attackSpeed;
    public float turnRate => moveRate * 0.4f;
    public float currentAnimLength => article.animator != null ? article.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length : 1f;
    public bool firstMove => _firstMove;
    public HexaUnit target => _target;
    public HexaUnitProjectile projectile => _projectile;


    void Update()
    {
        Act();
    }

    private void OnEnable()
    {
        _forceStop = false;
    }

    public void ForceStop()
    {
        _forceStop = true;
    }

    public void DontForceStop()
    {
        _forceStop = false;
    }

    public void SetTileIndex(Vector2Int index, bool isPre = false)
    {
        if (isPre)
            _preIndex = index;
        else
            _tileIndex = index;
    }

    public void SetTarget(HexaUnit unit)
    {
        _target = unit;
    }

    public void Move(Vector2Int next)
    {
        if(_moveDirty)
            return;

        if (_firstMove)
            _firstMove = false;

        //더티 셋
        _moveDirty = true;
        var temp = _tileIndex;
        _tileIndex = next;
        _preIndex = temp;

        _startPos = TilemapManager.instance.hexa_tilePosList[_preIndex.y, _preIndex.x];
        _endPos = TilemapManager.instance.hexa_tilePosList[_tileIndex.y, _tileIndex.x];

        //터닝이 필요한 경우
        var lastDirIndex = _savedDirIndex;
        _savedDirIndex = HexaUnitManager.instance.EvenToAxial(_tileIndex) - HexaUnitManager.instance.EvenToAxial(_preIndex);

        if(_savedDirIndex != lastDirIndex || _savedDirIndex == Vector2Int.zero)
        {
            _hasTurn = true;
            _savedRot = transform.rotation;
        }
    }

    public void Attack()
    {
        //디버그용 스타트 어택
        _startAtk = true;

        _atkDirty = true;
        _lastTargetIndex = target.tileIndex;

        //터닝이 필요한 경우
        var lastDirIndex = _savedDirIndex;
        _savedDirIndex = HexaUnitManager.instance.EvenToAxial(target.tileIndex) - HexaUnitManager.instance.EvenToAxial(_tileIndex);
        if(lastDirIndex != _savedDirIndex)
        {
            _hasTurn = true;
            _savedRot = transform.rotation;
        }
        _startPos = TilemapManager.instance.hexa_tilePosList[tileIndex.y,tileIndex.x];
        _endPos = TilemapManager.instance.hexa_tilePosList[target.tileIndex.y,target.tileIndex.x];
    }

    public void StartAttack() => _startAtk = true;

    public void Damaged(int damage)
    {
        article.hp -= damage;
        if (article.hp < 0)
            Die();
    }
    public void Die()
    {
        HexaUnitManager.instance.UnRegisterHexaUnit(this);
        //부모전환 해결방법 찾기

        if(team == 1)
        {
            CreepSpawnManager.instance.ReturnCreep(GetComponent<CreepComponent>());
            return;
        }
        //죽는 애니메이션
        ForceStop();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 행동 수행, 더티셋에 따라 분기행동
    /// 단 회전이 있다면 회전을 제일 먼저 수행
    /// </summary>
    void Act()
    {
        if(needUpdate || _forceStop)
        {
            //Idle 재생
            if (article.animator != null && !_forceStop)
                article.animator.Play("Idle");
            return;
        }
            

        if(_hasTurn)
        {
            _actTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(_savedRot, Quaternion.LookRotation((_endPos - _startPos).normalized), _actTimer / turnRate);
            if(_actTimer > turnRate)
            {
                _actTimer = 0f;
                _hasTurn = false;
            }
            return;
        }
        if(_moveDirty)
        {
            if (article.animator != null)
            {
                if (Mathf.Approximately(_actTimer,0f))
                {
                    article.animator.Play("Move",-1,0f);
                    article.animator.Update(0f);
                }
                    
                article.animator.SetFloat("MoveSpeed", moveRate);
            }
                
            var dist = Vector3.Distance(_startPos,_endPos);
            var distF = 1 / (dist / (moveRate * 2f));
            _actTimer += distF * Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _endPos, _actTimer);

            if (_actTimer > 1)
            {
                _actTimer = 0f;
                _moveDirty = false;
                transform.position = TilemapManager.instance.hexa_tilePosList[_tileIndex.y, _tileIndex.x];
            }
            return;
        }
        if (_atkDirty)
        {
            if (article.animator != null)
            {
                if(Mathf.Approximately(_actTimer, 0f))
                {
                    article.animator.Play("Attack", -1, 0f);
                    article.animator.Update(0f);
                }
                   
                article.animator.SetFloat("AttackSpeed", atkRate);
            }
            
            var atkF = 1 / (currentAnimLength / atkRate);
            _actTimer += atkF * Time.deltaTime;

            if (_startAtk)
            {
                if(projectilePrefab != null)
                {
                    //투사체 생성
                    if (_projectile == null)
                    {
                        var projectileParent = new GameObject();
                        projectileParent.name = "ProjectilePool";
                        projectileParent.transform.parent = transform;

                        _projectile = Instantiate(projectilePrefab);
                        _projectile.owner = this;
                        _projectile.transform.parent = projectileParent.transform;
                    }
                    //이미 생성되어 있다면 초기화 세팅
                    _projectile.gameObject.SetActive(true);
                    _projectile.Initialize();
                }
                else
                {
                    if (_target != null && _target.gameObject.activeSelf)
                        _target.Damaged(article.attackDamage);
                    //공격 이펙트
                    //타겟 attack 이펙트
                }

                _startAtk = false;
            }

            //행동 타이머가 끝났는 지 + 투사체 공격일 시 투사체 공격이 닿았는 지 확인 후 공격행동 종료
            if(_actTimer > 1 && (projectilePrefab != null ? _projectile.endMovement : true))
            {
                _actTimer = 0f;
                _atkDirty = false;
            }
            return;
        }
    }
}
