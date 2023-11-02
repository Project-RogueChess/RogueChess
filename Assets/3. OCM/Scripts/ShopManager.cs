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


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        piecesList = FindObjectOfType<PiecesList>();
        piecesShopImage = GetComponent<Image>();

    }

    private void Start()
    {
        int num = Random.Range(0, 5);

        piecesImage = piecesList.piecesImages[num];
        piecesName = piecesList.piecesNames[num];
        piecesSynergy = piecesList.piecesSynergys[num];
        piecesGold = piecesList.piecesGolds[num];
        piecesShopImage.sprite = piecesList.piecesImages[num];

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

        ShopPieces();
    }

    public void ShopPieces()
    {
        piecesImage = piecesList.piecesImages[num];
        piecesName = piecesList.piecesNames[num];
        piecesSynergy = piecesList.piecesSynergys[num];
        piecesGold = piecesList.piecesGolds[num];
        piecesShopImage.sprite = piecesList.piecesImages[num];
        nameTxt.text = piecesName;
        goldTxt.text = "$" + piecesGold;
        synergyTxt.text = piecesSynergy;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        DataManager.instance.myGold--;
        UIManager.instance.UIRefresh();
    }
}
