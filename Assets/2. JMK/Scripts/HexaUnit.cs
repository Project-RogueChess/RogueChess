using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
    private Vector3 _preDir;
    private Vector2Int _gridIndex;
    private Vector2Int _preIndex = new Vector2Int(-1,-1);
    private HexaUnit _target;


    public bool needUpdate => _needUpdate;
    public Vector2Int gridIndex => _gridIndex;
    public Vector2Int preIndex => _preIndex;
    public HexaUnit target => _target;

    public void SetGridIndex(Vector2Int index, bool isPre = false)
    {
        if (isPre)
            _preIndex = index;
        else
            _gridIndex = index;
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
        var dir = (target.transform.position - transform.position).normalized;
        transform.forward = dir;
    }

    IEnumerator ExcuteMove(Vector2Int next)
    {
        _needUpdate = false;
        //이전의 이동방향 기억하기


        //회전이 필요한경우 회전먼저
        var temp = _gridIndex;
        _gridIndex = next;
        _preIndex = temp;

        var startPos = TilemapManager.instance.hexa_tilePosList[_preIndex.y, _preIndex.x];
        var endPos = TilemapManager.instance.hexa_tilePosList[_gridIndex.y, _gridIndex.x];

        var dir = (endPos - startPos).normalized;
        transform.forward = dir;

        var timer = 0f;
        while (timer < moveRate)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, timer / moveRate);
            yield return null;
        }

        //이전의 이동방향이 지금방향과 같았다면 기다림 무시
        
        yield return new WaitForSeconds(0.1f);

        _preDir = dir;
        _needUpdate = true;
    }
}
