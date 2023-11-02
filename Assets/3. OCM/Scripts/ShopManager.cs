using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : PiecesList, IPointerClickHandler
{
    
    private CanvasGroup canvasGroup;

    
    public int num ;
    public Sprite piecesImage;
    public string piecesName;
    public string piecesSynergy;
    public int piecesGold;
    PiecesList piecesList;
    private Image piecesShopImage;

    public TMP_Text nameTxt;
    public TMP_Text synergyTxt;
    public TMP_Text goldTxt;

    public int[] percentages = new int[4];


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
        float rangeNum = Random.Range(0, 100);
        
        if (rangeNum <= percentages[0])
        {
            num = 1;
        }
        else if (rangeNum <= percentages[0] + percentages[1])
        {
            num = 2;
        }
        else if(rangeNum <= percentages[0] + percentages[1] + percentages[2])
        {
            num = 3;
        }
        else if (rangeNum <= percentages[0] + percentages[1] + percentages[2] + percentages[3])
        {
            num = 4;
        }
        else 
        {
            num = 5;
        }

        piecesImage = piecesList.piecesImages[num];
        piecesName = piecesList.piecesNames[num];
        piecesSynergy = piecesList.piecesSynergys[num];
        piecesGold = piecesList.piecesGolds[num];
        piecesShopImage.sprite = piecesImage;
        nameTxt.text = piecesName;
        synergyTxt.text = piecesSynergy;
        goldTxt.text = "$" + piecesGold;

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
        num = Random.Range(0, 5);

        ShowShopPieces();
    }

    public void ShowShopPieces()
    {
        piecesImage = piecesList.piecesImages[num];
        piecesName = piecesList.piecesNames[num];
        piecesSynergy = piecesList.piecesSynergys[num];
        piecesGold = piecesList.piecesGolds[num];
        piecesShopImage.sprite = piecesList.piecesImages[num];
        nameTxt.text = piecesList.piecesNames[num];
        goldTxt.text = "$" + piecesList.piecesGolds[num];
        synergyTxt.text = piecesList.piecesSynergys[num];
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(DataManager.instance.myGold >= piecesList.piecesGolds[num])
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            DataManager.instance.myGold -= piecesGold;
            UIManager.instance.UIRefresh();
        }
    }
}
