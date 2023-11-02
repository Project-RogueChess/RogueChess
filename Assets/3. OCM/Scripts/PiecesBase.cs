using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecesBase : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int atk;
    public float atkSped;
    public float atkRange;
    public int gold;

    public PiecesSynergy piecesSynergy;
    public PiecesType piecesType;


    public Image piecesImage;
}

public enum PiecesSynergy 
{
    goblin,
    orc,
    human,
    feather
}

public enum PiecesType
{
    warrior,
    range
}
