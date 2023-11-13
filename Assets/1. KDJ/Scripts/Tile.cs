using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject piece;

    public GameObject tile;

    public TilemapTriggerInfo triggerInfo;

    private void OnMouseDown()
    {
        if (piece != null)
        {
            TileManager.Instance.isDrag = true;
            TileManager.Instance.prevPiece = piece;
            TileManager.Instance.prevTile = this;

            HexaUnit unit = piece.GetComponent<HexaUnit>();
            if (HexaUnitManager.instance.unitList.Contains(unit))
                HexaUnitManager.instance.UnRegisterHexaUnit(unit);
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
        TileManager.Instance.nextTile = null;
        TileManager.Instance.nextPiece = null;
        tile.GetComponent<MeshRenderer>().material.color = TilemapManager.instance.hexa_defColor;
    }

    private void OnMouseUp()
    {
        if (TileManager.Instance.isDrag)
        {
            TileManager.Instance.isDrag = false;
            if (TileManager.Instance.nextTile != null)
            {
                GameObject tempGO = TileManager.Instance.nextPiece;
                TileManager.Instance.prevPiece.transform.position = TileManager.Instance.nextTile.transform.position;
                TileManager.Instance.nextTile.piece = TileManager.Instance.prevPiece;
                TileManager.Instance.prevTile.piece = tempGO;
				HexaUnit unit = TileManager.Instance.prevPiece.GetComponent<HexaUnit>();
                unit.SetTileIndex(new Vector2Int(TileManager.Instance.nextTile.triggerInfo.x, TileManager.Instance.nextTile.triggerInfo.y));
                HexaUnitManager.instance.RegisterHexaUnit(unit);

                if (tempGO != null)
                {
                    HexaUnit otherUnit = TileManager.Instance.nextPiece.GetComponent<HexaUnit>();
                    otherUnit.SetTileIndex(new Vector2Int(TileManager.Instance.prevTile.triggerInfo.x, TileManager.Instance.prevTile.triggerInfo.y));
                    TileManager.Instance.prevTile.piece.transform.position = TileManager.Instance.prevTile.transform.position;
                }
            }
            else
            {
                TileManager.Instance.prevPiece.transform.position = TileManager.Instance.prevTile.transform.position;
                Debug.Log("ºóÄ­¿¡ ³öµÒ");
            }
        }
    }
}
