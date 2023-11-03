using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IPointerClickHandler
{
    
    private CanvasGroup canvasGroup;
    public int num ;
    public Sprite piecesImage;
    public string piecesName;
    public string piecesSynergy;
    public int piecesGold;
    PiecesList piecesList;
    private Image piecesShopImage;

    //상점에서 보여줄 정보
    public TMP_Text nameTxt;
    public TMP_Text synergyTxt;
    public TMP_Text goldTxt;

    public int[] percentages = new int[5];
    Pieces piecesInfo;

    public float rangeNum;

    private void Awake()
    {
        for (int i = 0; i < DataManager.instance.wholePercentage.Length; i++)
        {
            percentages[i] = DataManager.instance.wholePercentage[i];
            
        }
        
        canvasGroup = GetComponent<CanvasGroup>();
        piecesList = FindObjectOfType<PiecesList>();
        piecesShopImage = GetComponent<Image>();
    }

    private void Start()
    {
        Reroll();

        piecesGold = piecesInfo.gold;
        piecesImage = piecesInfo.piecesImg;
        piecesName = piecesInfo.name;
        piecesSynergy = piecesInfo.synergy;
        //piecesName = piecesList.piecesNames[num];
        //piecesSynergy = piecesList.piecesSynergys[num];
        //piecesGold = piecesList.piecesGolds[num];
        piecesShopImage.sprite = piecesImage;
        nameTxt.text = piecesName;
        synergyTxt.text = piecesSynergy;
        goldTxt.text = "$" + piecesInfo.gold;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && DataManager.instance.myGold>=2)
        {
            //DataManager.instance.myGold -= 2;
            Reroll();
        } 
            
    }


    public void Reroll()
    {

        for (int i = 0; i < DataManager.instance.wholePercentage.Length; i++)
        {
            percentages[i] = DataManager.instance.wholePercentage[i];

        }

        rangeNum = Random.Range(0, 100);
        if (rangeNum <= percentages[0])
        {
            num = Random.Range(0, 5);
            piecesInfo = piecesList.gold1Pieces[num].GetComponent<Pieces>();
        }
        else if (rangeNum <= percentages[0] + percentages[1])
        {
            num = Random.Range(0, 5);
            piecesInfo = piecesList.gold2Pieces[num].GetComponent<Pieces>();
        }
        else if (rangeNum <= percentages[0] + percentages[1] + percentages[2])
        {
            num = Random.Range(0, 5);
            piecesInfo = piecesList.gold3Pieces[num].GetComponent<Pieces>();
        }
        else if (rangeNum <= percentages[0] + percentages[1] + percentages[2] + percentages[3])
        {
            num = Random.Range(0, 5);
            piecesInfo = piecesList.gold4Pieces[num].GetComponent<Pieces>();
        }
        else
        {
            num = Random.Range(0, 5);
            piecesInfo = piecesList.gold5Pieces[num].GetComponent<Pieces>();
        }


        ShowShopPieces();
    }

    public void ShowShopPieces()
    {
        piecesShopImage.sprite = piecesInfo.piecesImg;
        piecesName = piecesInfo.name;
        piecesSynergy = piecesInfo.synergy;
        piecesGold = piecesInfo.gold;

        nameTxt.text = piecesInfo.name;
        goldTxt.text = "$" + piecesInfo.gold;
        synergyTxt.text = piecesInfo.synergy;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(DataManager.instance.myGold >= piecesInfo.gold)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            DataManager.instance.myGold -= piecesInfo.gold;
            UIManager.instance.UIRefresh();
        }
    }
}
