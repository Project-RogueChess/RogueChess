using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep
{
    public GameObject creepPrefab;

    public string name;
    public int id;
    public int maxHp;
    public int hp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;

    public Avatar avatar;
    public Animator animator;
}
