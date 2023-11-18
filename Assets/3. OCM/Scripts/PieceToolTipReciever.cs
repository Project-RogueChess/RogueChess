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

    public bool isMouseOn;
    void Awake()
    {
        parentParent = transform.parent.parent;


        pieceToolTip = FindObjectOfType<PieceToolTip>();

        piece = parentParent.GetComponent<Pieces>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOn = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOn = false;
        pieceToolTip.gameObject.transform.position = new Vector3(2300, 600, 0);
    }

    void Update()
    {
        if (isMouseOn == true)
        {
            pieceToolTip.gameObject.transform.position = new Vector3(1710, 810, 0);
            pieceToolTip.SetupPieceToolTip(piece.pieceImg, piece.items[0].itemSprite, piece.items[1].itemSprite, piece.items[2].itemSprite, piece.name, piece.hp, piece.attackDamage, piece.attackSpeed);
        }
    }

}