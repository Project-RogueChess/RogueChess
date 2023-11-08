using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "creep Data", menuName = "Scriptable Object/2.creep Data", order = 2)]
public class CreepData : ScriptableObject
{
    [SerializeField]
    private string creepName;
    public string _creepName { get { return creepName; } }

    [SerializeField]
    private int hp;
    public int _hp { get { return hp; } }

    [SerializeField]
    private int attackPoint;
    public int _attackPoint { get { return attackPoint; } }

    [SerializeField]
    private float attackSpeed;
    public float _attackSpeed { get {  return attackSpeed; } }

    [SerializeField]
    private int attackRange;
    public int _attackRange { get { return attackRange; } }

    [SerializeField]
    private float moveSpeed;
    public float _moveSpeed { get { return moveSpeed; } }
}

