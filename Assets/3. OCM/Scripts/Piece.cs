using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Piece
{
    public Sprite piecesImg;
    public string name;
    public string synergy;
    public int gold;

    public int tier;

    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int attack;
    public int attackSpeed;


    public Piece(string name, string synergy, int gold, int tier, int maxHp, int hp, int maxMp, int mp, int attack, int attackSpeed)
    {
        this.name = name;
        this.synergy = synergy;
        this.gold = gold;
        this.tier = tier;
        this.maxHp = maxHp;
        this.hp = hp;
        this.maxMp = maxMp;
        this.mp = mp;
        this.attack = attack;
        this.attackSpeed = attackSpeed;
    }

    public Piece()
    {

    }
}
