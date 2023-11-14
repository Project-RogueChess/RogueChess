using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class HexaUnitProjectile : MonoBehaviour
{
    public float moveSpeed = 2f;

    private HexaUnit _owner;
    private float _maxDist;
    private bool _endMovement = false;
    private Vector3 _lastTargetPos;

    public bool endMovement => _endMovement;

    public HexaUnit owner
    {
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;
        }
    }

    public Vector3 currentTarget => _owner.target != null ? _owner.target.transform.position : _lastTargetPos;

    public void Initialize()
    {
        if (_owner.target == null)
            return;

        _endMovement = false;
        _lastTargetPos = _owner.target.transform.position;
        transform.position = owner.transform.position;
    }

    private void Update()
    {
        if (!_endMovement)
        {
            _maxDist = Vector3.Distance(currentTarget, owner.transform.position);
            transform.rotation = Quaternion.LookRotation((currentTarget - _owner.transform.position).normalized);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
            if (Vector3.Distance(owner.transform.position, transform.position) > _maxDist
                || _owner.target == null)
            {
                //충돌 이펙트 재생
                _endMovement = true;
                gameObject.SetActive(false);
            }

            if (_owner.target != null)
                _lastTargetPos = _owner.target.transform.position;
        }
    }
}
