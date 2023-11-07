using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HexaUnit : MonoBehaviour
{
    private bool _needUpdate;
    private Vector2Int _gridIndex;
    private Vector2Int _reservedIndex = new Vector2Int(-1,-1);
    private int _team;
    private int _range;
    private float _atkRate;
    private float _moveRate;
    private HexaUnit _target;

    public bool needUpdate => _needUpdate;
    public Vector2Int gridIndex => _gridIndex;
    public Vector2Int reservedIndex => _reservedIndex;
    public int team => _team;
    public int range => _range;
    public float actRate => _moveRate;
    public HexaUnit target => _target;

    public void Move()
    {
        StartCoroutine(ExcuteMove());
    }

    public void Attack()
    {

    }

    IEnumerator ExcuteMove()
    {
        //회전이 필요한경우 회전먼저
        _needUpdate = false;
        var startPos = TilemapManager.instance.hexa_tilePosList[_gridIndex.y, _gridIndex.x];
        var endPos = TilemapManager.instance.hexa_tilePosList[_reservedIndex.y, _reservedIndex.x];

        var timer = 0f;
        while (timer > _moveRate)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, timer / _moveRate);
            yield return null;
        }
    }
}
