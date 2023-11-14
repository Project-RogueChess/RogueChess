using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.Progress;

public class PiecesDB : MonoBehaviour
{
    public List<Piece> pieces = new List<Piece>();
    public List<Piece> gold1list = new List<Piece>();
    public List<Piece> gold2list = new List<Piece>();
    public List<Piece> gold3list = new List<Piece>();
    public List<Piece> gold4list = new List<Piece>();
    public List<Piece> gold5list = new List<Piece>();

    private void Awake()
    {
        ReadCharData("a_pieceDB", pieces);
        ListingASGold();
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
            piece.gold = int.Parse(dicList[i]["gold"].ToString());
            piece.id = int.Parse(dicList[i]["id"].ToString());
            piece.spieces = dicList[i]["spieces"].ToString();
            piece.classes = dicList[i]["classes"].ToString();
            piece.grade = int.Parse(dicList[i]["grade"].ToString());

            piece.maxHp = int.Parse(dicList[i]["maxHp"].ToString());
            piece.hp = int.Parse(dicList[i]["hp"].ToString());
            piece.maxMp = int.Parse(dicList[i]["maxMp"].ToString());
            piece.mp = int.Parse(dicList[i]["mp"].ToString());
            piece.attackDamage = int.Parse(dicList[i]["attackDamage"].ToString());
            piece.attackSpeed = float.Parse(dicList[i]["attackSpeed"].ToString());
            piece.attackRange = int.Parse(dicList[i]["attackRange"].ToString());
            piece.moveSpeed = float.Parse(dicList[i]["moveSpeed"].ToString());
            piece.pieceImg = Resources.Load<Sprite>("PiecesImages/" + dicList[i]["gold"].ToString() + "/" + dicList[i]["name"].ToString());
            piece.piecePrefab = Resources.Load<GameObject>("PiecesPrefabs/" + dicList[i]["gold"].ToString() + "/" + dicList[i]["name"].ToString());

            piece.avatar = Resources.Load<Avatar>("PiecesAvatar/" + dicList[i]["gold"].ToString() + "/" + dicList[i]["name"].ToString());
            piece.animator = Resources.Load<Animator>("PiecesAnimation/" + dicList[i]["gold"].ToString() + "/" + dicList[i]["name"].ToString());


            pieces.Add(piece);
        }

    }

    private void ListingASGold()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].gold == 1)
            {
                gold1list.Add(pieces[i]);
            }
            if (pieces[i].gold == 2)
            {
                gold2list.Add(pieces[i]);
            }
            if (pieces[i].gold == 3)
            {
                gold3list.Add(pieces[i]);
            }
            if (pieces[i].gold == 4)
            {
                gold4list.Add(pieces[i]);
            }
            if (pieces[i].gold == 5)
            {
                gold5list.Add(pieces[i]);
            }
        }
    }
}
