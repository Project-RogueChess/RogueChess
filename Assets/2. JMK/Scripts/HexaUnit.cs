using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public enum HexaUnitActType
{
    Common,
    Jumper
}

public class HexaUnit : MonoBehaviour
{
    public int team;
    public int range;
    public HexaUnitActType actType;
    public float atkRate = 0.5f;
    public float moveRate = 0.5f;
    public Transform ccPivot;
    public AnimationCurve ccMotion;

    private bool _moveDirty = false;
    private bool _atkDirty = false;
    private bool _turnDirty = false;
    private Vector2Int _tileIndex;
    private Vector2Int _preIndex = new Vector2Int(-1,-1);
    private Vector2Int _lastTargetIndex;
    private HexaUnit _target;

    private Vector3 _startPos;
    private Vector3 _endPos;

    private Vector2Int _savedDirIndex;
    private Quaternion _savedRot;
    private float _actTimer = 0f;

    public bool needUpdate => !_moveDirty && !_atkDirty && !_turnDirty;
    public Vector2Int tileIndex => _tileIndex;
    public Vector2Int preIndex => _preIndex;
    public Vector2Int lastTargetIndex => _lastTargetIndex;
    public float turnRate => moveRate * 0.5f;

    public HexaUnit target => _target;


    void Update()
    {
        Act();
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
            _turnDirty = true;
            _savedRot = transform.rotation;
        }
    }

    public void Attack()
    {
        
        var lastDirIndex = _savedDirIndex;
        _savedDirIndex = HexaUnitManager.instance.EvenToAxial(target.tileIndex) - HexaUnitManager.instance.EvenToAxial(_tileIndex);
        if(lastDirIndex != _savedDirIndex)
        {
            _turnDirty = true;
            _savedRot = transform.rotation;
        }
        _startPos = transform.position;
        _endPos = target.transform.position;
        _lastTargetIndex = target.tileIndex;
        _atkDirty = true;
    }

    private void Act()
    {
        if(needUpdate)
            return;

        _actTimer += Time.deltaTime;
        if(_turnDirty)
        {
            transform.rotation = Quaternion.Slerp(_savedRot, Quaternion.LookRotation((_endPos - _startPos).normalized), _actTimer / turnRate);
            if(_actTimer > turnRate)
            {
                _actTimer = 0f;
                _turnDirty = false;
            }
            return;
        }
        if(_moveDirty)
        {
            transform.position = Vector3.Lerp(_startPos, _endPos, _actTimer / moveRate);

            if (_actTimer > moveRate)
            {
                _actTimer = 0f;
                _moveDirty = false;
                transform.position = TilemapManager.instance.hexa_tilePosList[_tileIndex.y, _tileIndex.x];
            }
            return;
        }
        if (_atkDirty)
        {
        }
    }
}
