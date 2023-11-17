using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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

    public SynergyArraySO synergyArraySo;
    public string[] synergysArray;
    public int[] synergysNum;



    //public GameObject redPieces;
    //public GameObject greePieces;
    //public GameObject bluePeices;
    //public TMP_Text redPiecesTxt;
    //public TMP_Text greenPiecesTxt;
    //public TMP_Text bluePiecesTxt;



    private void Awake()
    {
        instance = this;

        for (int i = 0; i < synergySpieces.Length; i++)
        {
            synergySpieces[i] = synergyPiecesDB.pieces[i].spieces;

        }
        for (int i = 0; i < synergyClasses.Length; i++)
        {
            synergyClasses[i] = synergyPiecesDB.pieces[i].classes;
        }



        synergySpiecesNum = new int[synergySpieces.Length];
        synergyClassesNum = new int[synergyClasses.Length];


        idsArray = new bool[synergyPiecesDB.pieces.Count];
        synergyArraySo = (SynergyArraySO)Resources.Load("SynergyScriptableObj/SynergysArray");


        synergysArray = new string[synergyArraySo.synergyArray.Length];
        synergysNum = new int[synergyArraySo.synergyArray.Length];
        for (int i = 0; i < synergyArraySo.synergyArray.Length; i++)
        {
            synergysArray[i] = synergyArraySo.synergyArray[i];
        }
        for (int i = 0; i < synergyArraySo.synergyArray.Length; i++)
        {
            synergysNum[i] = 0;
        }

        //redPieces.SetActive(false);
        //greePieces.SetActive(false);
        //bluePeices.SetActive(false);

    }

    public void SpawnUnit(Tile tile)
    {
        if (TileType.Inv == tile.triggerInfo.type)
            tile.piece = Instantiate(spawnUnit, tile.transform.position, Quaternion.identity);


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

        DataManager.instance.FixWhatMyPieces(articleCount);
        UIManager.instance.UIRefresh();
    }

    public void SearchEveryTileForSynergyData()
    {
        synergyData = new SynergyData(synergysArray);
        idList = new bool[30];
        synergysNum = new int[synergysArray.Length];

        foreach (var hex in hexaTiles)
        {

            if (hex.piece != null)
            {
                var currentPiece = hex.piece.GetComponent<Pieces>();

                if (idList[currentPiece.id])
                    continue;

                //switch (currentPiece.spieces)
                //{
                //    default:
                //        break;
                //    case "red":
                //        synergyData.red++;
                //        break;
                //    case "blue":
                //        synergyData.blue++;
                //        break;
                //    case "green":
                //        synergyData.green++;
                //        break;
                //}
                for (int i = 0; i < synergysArray.Length; i++)
                {
                    if (currentPiece.spieces == synergyData.synergysArray[i])
                    {
                        synergysNum[i]++;


                        //showSynergy(currentPiece, synergysArray);
                        
                        //여기서 1개라도 있으면 시너지 UI보여줘야함
                    }
                }
                idList[currentPiece.id] = true;
            }
        }




        //인벤토리에 있는 애들 억지로 시너지 제거 좋은 방법은 아닌데 시간이 없어서 이렇게 함
        List<Pieces> invpieces = new List<Pieces>();
        foreach (var inv in invTiles)
        {
            if (inv.piece != null)
            {
                var currentPiece = inv.piece.GetComponent<Pieces>();
                invpieces.Add(currentPiece);
            }
        }
        for (int i = 0; i < synergysArray.Length; i++)
        {
            
                var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/" + synergysArray[i]);
                currentSO.Execute(invpieces, -1);
            
        }


    }

    //private void showSynergy(Pieces piece, string[] synergyArray )
    //{
    //    for (int i =0;i<synergysArray.Length;i++)
    //    {
    //        if (piece.spieces == synergysArray[i])
    //        {

    //        }
    //    }
    //}

    public void SynergyEnhance(int[] termsData)
    {
        var pieces = new List<Pieces>();

        foreach (var hex in hexaTiles)
        {
            if (hex.piece != null && hex.piece.TryGetComponent(out Pieces current))
            {
                pieces.Add(current);
                
            }
        }


        //if(termsData.red != -1)
        //{
        //    var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Red");
        //    currentSO.Execute(pieces, termsData.red);
        //}
        //if (termsData.green != -1)
        //{
        //    var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Green");
        //    currentSO.Execute(pieces, termsData.green);
        //}
        //if (termsData.blue != -1)
        //{
        //    var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/SE_Blue");
        //    currentSO.Execute(pieces, termsData.blue);
        //}




        //-1일 때 실행을 안하는 것이 아니라 실행을 다시해서 시너지를 돌려야함
        //for (int i = 0; i < synergysArray.Length; i++)
        //{
        //    if (termsData[i] != -1)
        //    {
        //        var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/" + synergysArray[i]);
        //        currentSO.Execute(pieces, termsData[i]);
        //    }
        //}
        for (int i = 0; i < synergysArray.Length; i++)
        {
            
            var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/" + synergysArray[i]);
            currentSO.Execute(pieces, termsData[i]);
            
        }
    }

    public int[] CompareSynergy()
    {
        //간이 딕셔너리
        Dictionary<string, int[]> SynergyDB = new Dictionary<string, int[]>();





        ////사전완성
        //SynergyDB.Add("red", new int[] { 2,4,6});
        //SynergyDB.Add("green", new int[] { 1,3,5});
        //SynergyDB.Add("blue", new int[] { 1,2,3});
        for (int i = 0; i < synergysArray.Length; i++)
        {
            var currentSO = (SynergySO)Resources.Load("SynergyScriptableObj/" + synergysArray[i]);
            SynergyDB.Add(synergysArray[i], currentSO.terms);
        }


        //SynergyData termsData = new SynergyData(-1,-1,-1);
        int[] termsData = new int[] { -1, -1, -1};



        //foreach (var db in SynergyDB.Keys)
        //{
        //    int[] currentTerms = SynergyDB[db];

        //    int saveData = synergyData.SearchToString(db);

        //    for(int i = currentTerms.Length - 1; i >= 0; i--)
        //    {
        //        if (currentTerms[i] <= saveData)
        //        {
        //            termsData.InjectValue(db, i);
        //            break;
        //        }
        //    }
        //}

        int j = -1;
        foreach (var db in SynergyDB.Keys)
        {
            j++;

            Debug.Log(j);
            for (int i = SynergyDB[db].Length - 1; i >= 0; i--)
            {
                Debug.Log(SynergyDB[db][i]);
                Debug.Log(synergysNum[j]);
                if (SynergyDB[db][i] <= synergysNum[j])
                {
                    termsData[j] = i;
                    break;
                }
            }

        }

        for (int i= 0;i< synergysArray.Length;i++)
        {
            if (termsData[i] != -1)
            {
                //해당 시너지 UI,숫자 보여줘야함
            }
        }
        
        

        Debug.Log(termsData[0]);
        Debug.Log(termsData[1]);
        Debug.Log(termsData[2]);
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


    public void DeleteRandomPiece()
    {
        List<GameObject> pieces = new List<GameObject>();

        for (int i = 0; i < invTiles.Count; i++)
        {
            if (invTiles[i].piece != null)
            {
                pieces.Add(invTiles[i].piece);
            }
        }

        int j = Random.Range(0, pieces.Count);
        if (pieces.Count >= 1)
        {
            for (int i = 0; i < invTiles.Count; i++)
            {
                if (invTiles[i].piece == pieces[j])
                {
                    Destroy(invTiles[i].piece);
                }
            }
        }
    }

    public void AddRandomPiece(int pieceCost)
    {

        Piece piece = new Piece();


        switch (pieceCost)
        {
            case 1:
                {
                    int i = Random.Range(0, synergyPiecesDB.gold1list.Count);
                    piece = synergyPiecesDB.gold1list[i];
                    break;
                }
            case 2:
                {
                    int i = Random.Range(0, synergyPiecesDB.gold2list.Count);
                    piece = synergyPiecesDB.gold2list[i];
                    break;
                }
            case 3:
                {
                    int i = Random.Range(0, synergyPiecesDB.gold3list.Count);
                    piece = synergyPiecesDB.gold3list[i];
                    break;
                }
            case 4:
                {
                    int i = Random.Range(0, synergyPiecesDB.gold4list.Count);
                    piece = synergyPiecesDB.gold4list[i];
                    break;
                }
            case 5:
                {
                    int i = Random.Range(0, synergyPiecesDB.gold5list.Count);
                    piece = synergyPiecesDB.gold5list[i];
                    break;
                }

        }

        for (int i = 0; i < invTiles.Count; i++)
        {
            if (invTiles[i].piece == null)
            {
                PiecesCountManager.instance.piecesIdCounts[piece.id]++;

                InvSpawnManager.instance.spawnUnit = piece.piecePrefab;
                int index = ButtonSpawner.instance.btnClick();

                if (index == -1)
                {
                    return;     // 위에서 애초에 클릭해도 반응없게 해서 상관없는데 일단 냅둠
                }
                var currentPiece = InvSpawnManager.instance.invTiles[index].piece.GetComponent<Pieces>();

                currentPiece.Parse(piece);

                if (PiecesCountManager.instance.piecesIdCounts[piece.id] > 2)
                {
                    UpgradeArticle.instance.TryUpgradeArticle(piece);
                    PiecesCountManager.instance.resetCounts(piece);
                }
                UIManager.instance.UIRefresh();
                InvSpawnManager.instance.CountArticle();
                break;
            }
        }
    }
}


public struct SynergyData
{
    //public int red;
    //public int green;
    //public int blue;
    public string[] synergysArray;



    //public SynergyData(int red, int green, int blue)
    //{
    //    this.red = red;
    //    this.green = green;
    //    this.blue = blue;
    //}
    public SynergyData(string[] synergysArray)
    {
        this.synergysArray = synergysArray;
    }



    //public int SearchToString(string text)
    //{
    //    switch (text)
    //    {
    //        default:
    //            return 0;
    //        case "red":
    //            return red;
    //        case "green":
    //            return green;
    //        case "blue":
    //            return blue;
    //    }
    //}

    //public void InjectValue(string text, int value)
    //{
    //    switch (text)
    //    {
    //        default:
    //            break;
    //        case "red":
    //            red = value;
    //            break;
    //        case "green":
    //            green = value;
    //            break;
    //        case "blue":
    //            blue = value;
    //            break;
    //    }
    //}
}

