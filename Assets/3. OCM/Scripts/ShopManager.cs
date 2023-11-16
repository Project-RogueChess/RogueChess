using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IPointerClickHandler
{
    
    private CanvasGroup canvasGroup;
    public int num ;
    public Sprite pieceImage;
    public string pieceName;
    public string pieceSpieces;
    public string pieceClass;
    public int pieceGold;


    PiecesDB piecesDB;
    private Image piecesShopImage;

    //상점에서 보여줄 정보
    public TMP_Text nameTxt;
    public TMP_Text synergysTxt;
    public TMP_Text goldTxt;

    public int[] percentages = new int[5];
    List<Piece> pieceInfos = new List<Piece>();
    public Piece pieceInfo;
    public int rangeNum;

    

    private void Awake()
    {
        for (int i = 0; i < DataManager.instance.wholePercentage.Length; i++)
        {
            percentages[i] = DataManager.instance.wholePercentage[i];
            
        }
        
        canvasGroup = GetComponent<CanvasGroup>();
        piecesDB = FindObjectOfType<PiecesDB>();
        piecesShopImage = GetComponent<Image>();
    }

    private void Start()
    {
        ReRoll(DataManager.instance.reroolLock);

        ShowShopPieces();

    }


    public void ReRoll(bool lockflag)
    {

        if(lockflag == true)
        {
            return;
        }
        else
        {
            for (int i = 0; i < DataManager.instance.wholePercentage.Length; i++)
            {
                percentages[i] = DataManager.instance.wholePercentage[i];

            }

            rangeNum = Random.Range(0, 101);
            if (rangeNum <= 101)
            {
                num = Random.Range(0, 5);
                pieceInfos = piecesDB.gold1list;
                pieceInfo = pieceInfos[num];
            }
            else if (rangeNum <= percentages[0] + percentages[1])
            {
                num = Random.Range(0, 5);
                pieceInfos = piecesDB.gold2list;
                pieceInfo = pieceInfos[num];
            }
            else if (rangeNum <= percentages[0] + percentages[1] + percentages[2])
            {
                num = Random.Range(0, 5);
                pieceInfos = piecesDB.gold3list;
                pieceInfo = pieceInfos[num];
            }
            else if (rangeNum <= percentages[0] + percentages[1] + percentages[2] + percentages[3])
            {
                num = Random.Range(0, 5);
                pieceInfos = piecesDB.gold4list;
                pieceInfo = pieceInfos[num];
            }
            else
            {
                num = Random.Range(0, 5);
                pieceInfos = piecesDB.gold5list;
                pieceInfo = pieceInfos[num];
            }
            ShowShopPieces();
        } 
    }

    public void ShowShopPieces()
    {
        piecesShopImage.sprite = pieceInfo.pieceImg;
        pieceName = pieceInfo.name;
        pieceSpieces = pieceInfo.spieces;
        pieceClass = pieceInfo.classes;
        pieceGold = pieceInfo.gold;


        pieceImage = piecesShopImage.sprite;
        nameTxt.text = pieceName;
        synergysTxt.text = pieceSpieces + ", " + pieceClass;
        goldTxt.text = "$" + pieceGold;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        UIManager.instance.UIRefresh();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece == null)
            {
                if (DataManager.instance.WhatMyGold() >= pieceInfo.gold)
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.blocksRaycasts = false;
                    DataManager.instance.LostGold(pieceInfo.gold);
                    PiecesCountManager.instance.piecesIdCounts[pieceInfo.id]++;
                    CreatePiece();

                    UIManager.instance.UIRefresh();
                    return ;
                }
            }

        }
        
    }

    public void CreatePiece()
    {
        InvSpawnManager.instance.spawnUnit = pieceInfo.piecePrefab;
        int index = ButtonSpawner.instance.btnClick();

        if(index == -1)
        {
            return;     // 위에서 애초에 클릭해도 반응없게 해서 상관없는데 일단 냅둠
        }
        var currentPiece = InvSpawnManager.instance.invTiles[index].piece.GetComponent<Pieces>();

        currentPiece.Parse(pieceInfo);
        //Instantiate(piecesList.gold1Pieces[num],new Vector3(0,0,0), Quaternion.identity);
    }
}
