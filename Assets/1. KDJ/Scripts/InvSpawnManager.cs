using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class InvSpawnManager : MonoBehaviour
{
    public static InvSpawnManager instance;

    public int articleCount;

    public GameObject spawnUnit;

    public List<Tile> hexaTiles = new List<Tile>();
    public List<Tile> invTiles = new List<Tile>();


    public string[] synergySpieces = new string[2];
    public string[] synergyClasses = new string[2];

    public PiecesDB synergyPiecesDB = new PiecesDB();


    public int[] synergySpiecesNum;
    public int[] synergyClassesNum;

    public List<int> ids;

    public bool[] idList;


    public bool[] idsArray;
    public SynergyData synergyData;



    private void Awake()
    {
        instance = this;

        for (int i = 0; i < synergySpieces.Length; i++)
        {
            synergySpieces[i] = synergyPiecesDB.pieces[i].spieces;
            
        }
        for(int i =0;i < synergyClasses.Length; i++)
        {
            synergyClasses[i] = synergyPiecesDB.pieces[i].classes;
        }



        synergySpiecesNum = new int[synergySpieces.Length];
        synergyClassesNum = new int[synergyClasses.Length];


        idsArray = new bool[synergyPiecesDB.pieces.Count];

    }

    public void SpawnUnit(Tile tile)
    {
        if(TileType.Inv == tile.triggerInfo.type)
        tile.piece = Instantiate(spawnUnit, tile.transform.position , Quaternion.identity);

        
    }

    public void ResetChampions()
    {
        
        for (int i = 0; i < hexaTiles.Count; i++)
        {
            if (hexaTiles[i].piece != null)
            {
                hexaTiles[i].Reset();
            }
        }
    }
    public void CountArticle()
    {
        articleCount = 0;

        for (int i = 0; i < hexaTiles.Count; i++)
        {
            if (hexaTiles[i].piece != null)
            {
                articleCount++;
            }
        }

        DataManager.instance.myPieces = articleCount;
        UIManager.instance.UIRefresh();
    }

    public void SearchEveryTileForSynergyData()
    {
        synergyData = new SynergyData();
        idList = new bool[30];
        foreach (var hex in hexaTiles)
        {
            
            if (hex.piece != null) 
            {
                var currentPiece = hex.piece.GetComponent<Pieces>();

                if (idList[currentPiece.id])
                    continue;

                switch (currentPiece.spieces)
                {
                    default:
                        break;
                    case "red":
                        synergyData.red++;
                        break;
                    case "blue":
                        synergyData.blue++;
                        break;
                    case "green":
                        synergyData.green++;
                        break;
                }

                idList[currentPiece.id] = true;
            }
                
        }
    }

    public void SynergyEnhance(SynergyData termsData)
    {
        var pieces = new List<Pieces>();

        foreach (var hex in hexaTiles)
        {
            if(hex.piece != null && hex.piece.TryGetComponent(out Pieces current))
            {
                pieces.Add(current);
            }
        }

        if(termsData.red != -1)
        {
            var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Red");
            currentSO.Execute(pieces, termsData.red);
        }
        if (termsData.green != -1)
        {
            var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Green");
            currentSO.Execute(pieces, termsData.green);
        }
        if (termsData.blue != -1)
        {
            var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Blue");
            currentSO.Execute(pieces, termsData.blue);
        }


    }

    public SynergyData CompareSynergy()
    {
        //∞£¿Ã µÒº≈≥ ∏Æ
        Dictionary<string, int[]> SynergyDB = new Dictionary<string, int[]>();

        //ªÁ¿¸øœº∫
        SynergyDB.Add("red", new int[] { 2,4,6});
        SynergyDB.Add("green", new int[] { 1,3,5});
        SynergyDB.Add("blue", new int[] { 1,2,3});

        SynergyData termsData = new SynergyData(-1,-1,-1);

        foreach(var db in SynergyDB.Keys)
        {
            int[] currentTerms = SynergyDB[db];

            int saveData = synergyData.SearchToString(db);

            for(int i = currentTerms.Length - 1; i >= 0; i--)
            {
                if (currentTerms[i] <= saveData)
                {
                    termsData.InjectValue(db, i);
                    break;
                }
            }
        }

        return termsData;
    }

    //public void SearchingIdsArrayBool()
    //{
    //    for(int i = 0; i < idsArray.Length; i++)
    //    {
    //        idsArray[i] = false;
    //    }
    //    ResetSynergysArray();
    //    for (int i = 0;i<hexaTiles.Count;i++)
    //    {
    //        if (hexaTiles[i].piece != null)
    //        {
    //            if (idsArray[hexaTiles[i].piece.GetComponent<Pieces>().id] == false)
    //            {
    //                for (int j = 0; j < synergyClasses.Length; j++)
    //                {
    //                    if (hexaTiles[i].piece.GetComponent<Pieces>().classes == synergyClasses[j])
    //                    {
    //                        synergyClassesNum[j]++;
    //                    }
    //                }
    //                for(int j =0; j<synergySpieces.Length; j++)
    //                {
    //                    if (hexaTiles[i].piece.GetComponent<Pieces>().spieces == synergySpieces[j])
    //                    {
    //                        synergySpiecesNum[j]++;
    //                    }
    //                }
    //            }
    //            idsArray[hexaTiles[i].piece.GetComponent<Pieces>().id] = true;
    //        }
    //    }
    //}
    public void ResetSynergysArray()
    {
        for (int i = 0; i < synergySpiecesNum.Length; i++)
        {
            synergySpiecesNum[i] = 0;
        }
        for (int i = 0; i < synergyClassesNum.Length; i++)
        {
            synergyClassesNum[i] = 0;
        }
    }

    public void GivingSynergyPower()
    {
        //human, elf, orc,(246)(369)(123)
        //warrior marksman ()()
    }
}   

public struct SynergyData
{
    public int red;
    public int green;
    public int blue;

    public SynergyData(int red, int green, int blue)
    {
        this.red = red;
        this.green = green;
        this.blue = blue;
    }

    public int SearchToString(string text)
    {
        switch (text)
        {
            default:
                return 0;
            case "red":
                return red;
            case "green":
                return green;
            case "blue":
                return blue;
        }
    }
    
    public void InjectValue(string text, int value)
    {
        switch (text)
        {
            default:
                break;
            case "red":
                red = value;
                break;
            case "green":
                green = value;
                break;
            case "blue":
                blue = value;
                break;
        }
    }
}
