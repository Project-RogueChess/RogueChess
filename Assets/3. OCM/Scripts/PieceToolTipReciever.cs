using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceToolTipReciever : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentParent;
    public Pieces piece;


    public PieceToolTip pieceToolTip;
    void Awake()
    {
        parentParent = transform.parent.parent;
        piece = parentParent.gameObject.GetComponent<Pieces>();


        pieceToolTip = FindObjectOfType<PieceToolTip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pieceToolTip.gameObject.transform.position = new Vector3(1710, 810, 0);
        pieceToolTip.SetupPieceToolTip(piece.pieceImg, piece.items[0].itemSprite, piece.items[1].itemSprite, piece.items[2].itemSprite, piece.name,piece.hp,piece.attackDamage,piece.attackSpeed);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        pieceToolTip.gameObject.transform.position = new Vector3(2300, 810, 0);
    }

}
