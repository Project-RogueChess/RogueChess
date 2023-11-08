using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesDB : MonoBehaviour
{
    public List<Piece> pieces = new List<Piece>();


    private void Awake()
    {
        ReadCharData("pieceDB", pieces);
    }

    private void ReadCharData(string v, List<Piece> pieces)
    {
        pieces.Clear();
        List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
        dicList.Clear();
        dicList = CSVReader.Read(v);

        for (int i = 0; i < dicList.Count; i++)
        {
            Piece piece = new Piece();
            
            piece.name = dicList[i]["name"].ToString();
            piece.synergy = dicList[i]["synergy"].ToString();
            piece.gold = int.Parse(dicList[i]["gold"].ToString());
            piece.tier = int.Parse(dicList[i]["tier"].ToString());

            piece.maxHp = int.Parse(dicList[i]["maxHp"].ToString());
            piece.hp = int.Parse(dicList[i]["hp"].ToString());
            piece.maxMp = int.Parse(dicList[i]["maxMp"].ToString());
            piece.mp = int.Parse(dicList[i]["mp"].ToString());
            piece.attack = int.Parse(dicList[i]["attack"].ToString());
            piece.attackSpeed = int.Parse(dicList[i]["attackSpeed"].ToString());
            piece.piecesImg = Resources.Load<Sprite>("PiecesImages/" + dicList[i]["gold"].ToString() + "/" + dicList[i]["name"].ToString());


            pieces.Add(piece);
        }

    }
}
