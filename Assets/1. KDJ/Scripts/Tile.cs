using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Tile : MonoBehaviour
{
    public GameObject piece;

    public GameObject tile;

    public TilemapTriggerInfo triggerInfo;

    private Color blackholeColor = Color.black;


    private void OnMouseDown()
    {
        if (piece != null)
        {
            UIManager.instance.ShowSellText(piece);
            TileManager.Instance.isDrag = true;
            TileManager.Instance.prevPiece = piece;
            TileManager.Instance.prevTile = this;

            HexaUnit unit = piece.GetComponent<HexaUnit>();
            if (HexaUnitManager.instance.unitList.Contains(unit))
                HexaUnitManager.instance.UnRegisterHexaUnit(unit);

            SoundManager.instance.PlaySound("pieceDown");
        }
    }

    private void OnMouseOver()
    {
        
        TileManager.Instance.nextTile = this;
        TileManager.Instance.nextPiece = piece;
        if (TileManager.Instance.isDrag)
        {
            tile.GetComponent<MeshRenderer>().material.color = TilemapManager.instance.hexa_activeColor;
        }
    }

    private void OnMouseExit()
    {
        if (TileManager.Instance.nextTile.tag == "Sell")
        {
            tile.GetComponent<MeshRenderer>().material.color = blackholeColor;
        }
        else
        {
            tile.GetComponent<MeshRenderer>().material.color = TilemapManager.instance.hexa_defColor;
        }
        TileManager.Instance.nextTile = null;
        TileManager.Instance.nextPiece = null;
        
    }

    private void OnMouseUp()
    {
        if (TileManager.Instance.isDrag)
        {
            TileManager.Instance.isDrag = false;
            if (TileManager.Instance.nextTile != null)
            {
                if (TileManager.Instance.nextTile.tag == "Sell")
                {
                    if (TileManager.Instance.prevTile.triggerInfo.type == TileType.Hexa)
                        HexaUnitManager.instance.UnRegisterHexaUnit(TileManager.Instance.prevTile.piece.GetComponent<HexaUnit>());
                    TileManager.Instance.prevTile.piece.GetComponent<Pieces>().SellPiece();
                    TileManager.Instance.prevTile.piece = null;
                    UIManager.instance.CloseSellText();
                    InvSpawnManager.instance.CountArticle();
                    return;
                }
                GameObject tempGO = TileManager.Instance.nextPiece;
                TileManager.Instance.prevPiece.transform.position = TileManager.Instance.nextTile.transform.position;
                TileManager.Instance.nextTile.piece = TileManager.Instance.prevPiece;
                TileManager.Instance.prevTile.piece = tempGO;

                if (tempGO != null)
                {
                    TileManager.Instance.prevTile.piece.transform.position = TileManager.Instance.prevTile.transform.position;
                    if(TileManager.Instance.nextTile.triggerInfo.type == TileType.Hexa)
                        HexaUnitManager.instance.UnRegisterHexaUnit(tempGO.GetComponent<HexaUnit>());

                    //¸¸¾à¿¡ °¡´Â °÷ÀÌ Çí»çÀÎ °æ¿ì¿¡ ´Ù½Ã À¯´Öµî·Ï
                    if(TileManager.Instance.prevTile.triggerInfo.type == TileType.Hexa)
                    {
                        //´Ù½Ã µî·Ï
                        var otherUnit = tempGO.GetComponent<HexaUnit>();
                        otherUnit.SetTileIndex(new Vector2Int(TileManager.Instance.prevTile.triggerInfo.x, TileManager.Instance.prevTile.triggerInfo.y));
                        HexaUnitManager.instance.RegisterHexaUnit(otherUnit);
                    }
                }

                if (TileManager.Instance.nextTile.tag != "Sell"
                    && TileManager.Instance.nextTile.triggerInfo.type == TileType.Hexa)
                {
                    HexaUnit unit = TileManager.Instance.prevPiece.GetComponent<HexaUnit>();
                    unit.SetTileIndex(new Vector2Int(TileManager.Instance.nextTile.triggerInfo.x, TileManager.Instance.nextTile.triggerInfo.y));
                    if (!HexaUnitManager.instance.unitList.Contains(unit))
                        HexaUnitManager.instance.RegisterHexaUnit(unit);
                }

            }
            else
            {
                if(TileManager.Instance.prevTile.triggerInfo.type == TileType.Hexa)
                {
                    HexaUnitManager.instance.RegisterHexaUnit(TileManager.Instance.prevPiece.GetComponent<HexaUnit>());
                }
                TileManager.Instance.prevPiece.transform.position = TileManager.Instance.prevTile.transform.position;
                Debug.Log("ºóÄ­¿¡ ³öµÒ");
            }

            UIManager.instance.CloseSellText();

            InvSpawnManager.instance.CountArticle();

            if (DataManager.instance.WhatMyPieces()> DataManager.instance.WhatMyMAXPieces())
            {
                TileManager.Instance.prevPiece.transform.position = TileManager.Instance.prevTile.transform.position;
                HexaUnit unit = TileManager.Instance.prevPiece.GetComponent<HexaUnit>();
                HexaUnitManager.instance.UnRegisterHexaUnit(unit);
                TileManager.Instance.prevTile.piece = TileManager.Instance.nextTile.piece;
                TileManager.Instance.nextTile.piece = null;
                InvSpawnManager.instance.CountArticle();
            }
            //InvSpawnManager.instance.SearchingIdsArrayBool();
            InvSpawnManager.instance.SearchEveryTileForSynergyData();
            InvSpawnManager.instance.SynergyEnhance(InvSpawnManager.instance.CompareSynergy());

            SoundManager.instance.PlaySound("pieceUp");

        }
    }

    public void TileReset()
    {
        if(piece != null) 
            piece.SetActive(true);

        //½ºÅÝ ¸®¼Â Ãß°¡

        SetWorldPostion();
        SetWorldRotation();
    }

    public void SetWorldRotation()
    {
         piece.transform.rotation = Quaternion.identity;
    }
    public void SetWorldPostion()
    {        
        piece.transform.position = this.transform.position;
    }    
}
