using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum HexaUnitAttackType
{
    Melee,
    Marksman
}

public class HexaUnit : MonoBehaviour
{
    public int team;
    public int range;
    public HexaUnitAttackType attackType;
    public float atkRate = 0.5f;
    public float moveRate = 0.5f;

    private bool _needUpdate = true;
    private Vector2Int _tileIndex;
    private Vector2Int _preIndex = new Vector2Int(-1,-1);
    private Vector2Int _lastTargetIndex;
    public HexaUnit _target;

    public bool needUpdate => _needUpdate;
    public Vector2Int tileIndex => _tileIndex;
    public Vector2Int preIndex => _preIndex;
    public Vector2Int lastTargetIndex => _lastTargetIndex;
    public HexaUnit target => _target;

    void OnDisable()
    {

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
        StartCoroutine(ExcuteMove(next));
    }

    public void Attack()
    {
        StartCoroutine(ExcuteAttack());
    }

    IEnumerator ExcuteMove(Vector2Int next)
    {
        _needUpdate = false;

        var temp = _tileIndex;
        _tileIndex = next;
        _preIndex = temp;

        var timer = 0f;

        var startPos = TilemapManager.instance.hexa_tilePosList[_preIndex.y, _preIndex.x];
        var endPos = TilemapManager.instance.hexa_tilePosList[_tileIndex.y, _tileIndex.x];

        var preDir = transform.forward;
        var dir = (endPos - startPos).normalized;
        var turnRate = moveRate * 0.5f;
        if (preDir != dir)
        {
            while(timer < turnRate)
            {
                timer += Time.deltaTime;
                transform.forward = Vector3.Lerp(preDir, dir, timer / turnRate);
                yield return null;
            }
        }

        timer = 0f;

        while (timer < moveRate)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, timer / moveRate);
            yield return null;
        }

        //이전의 이동방향이 지금방향과 같았다면 기다림 무시

        _needUpdate = true;
    }

    IEnumerator ExcuteAttack()
    {
        _lastTargetIndex = target.tileIndex;

        var dir = (target.transform.position - transform.position).normalized;
        var timer = 0f;
        var preDir = transform.forward;

        if (preDir != dir)
        {
            while (timer < moveRate)
            {
                timer += Time.deltaTime;
                transform.forward = Vector3.Lerp(preDir, dir, timer / moveRate);
                yield return null;
            }
        }
    }
}
