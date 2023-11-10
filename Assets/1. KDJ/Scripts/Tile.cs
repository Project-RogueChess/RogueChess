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
                if (tempGO != null)
                {
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
