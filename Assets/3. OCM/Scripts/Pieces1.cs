using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces1 : PiecesBase
{
    public void Awake()
    {
        maxHp = 1;
        hp = 1;
        maxMp = 1;
        mp = 1;
        atk = 1;
        atkRange = 1;
        atkSped = 1;
        piecesSynergy = PiecesSynergy.goblin;
        piecesType = PiecesType.warrior;


        gold = 1;
        
    }
    
}
