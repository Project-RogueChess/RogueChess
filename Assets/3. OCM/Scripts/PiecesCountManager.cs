using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PiecesCountManager : MonoBehaviour
{
    public PiecesDB piecesDB;
 
    public int[] piecesIdCounts;

    public static PiecesCountManager instance { get; private set; }
    public void Awake()
    {
        instance = this;
        piecesDB = FindObjectOfType<PiecesDB>();
        piecesIdCounts = new int[piecesDB.pieces.Count];
    }


}
