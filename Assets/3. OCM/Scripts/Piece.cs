using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Piece
{
    public Sprite pieceImg;
    public GameObject piecePrefab;

    public string name;
    public int id;
    public int gold;
    public string spieces;
    public string classes;
    public int grade;


    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;


    public Avatar avatar;
    public Animator animator;

    public Piece(string name,int id ,string spieces, string classes, int gold, int grade, int maxHp, int hp, int maxMp, int mp, int attackDamage, float attackSpeed, int attackRange, float moveSpeed)
    {
        this.name = name;
        this.id = id;
        this.spieces = spieces;
        this.classes = classes;
        this.gold = gold;
        this.grade = grade;
        this.maxHp = maxHp;
        this.hp = hp;
        this.maxMp = maxMp;
        this.mp = mp;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.moveSpeed = moveSpeed;
    }

    public Piece()
    {

    }
}
