using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergyToolTipReciever : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InvSpawnManager InvSpawnManager;


    public SynergyToolTipPanel SynergyToolTip;
    void Awake()
    {
        InvSpawnManager = GetComponent<InvSpawnManager>();

        SynergyToolTip = FindObjectOfType<SynergyToolTipPanel>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SynergyToolTip.gameObject.transform.position = new Vector3(1710, 810, 0);
        //SynergyToolTip.SetupSynergyToolTip(piece.pieceImg, piece.items[0].itemSprite, piece.items[1].itemSprite, piece.items[2].itemSprite, piece.name, piece.hp, piece.attackDamage, piece.attackSpeed);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        SynergyToolTip.gameObject.transform.position = new Vector3(2300, 810, 0);
    }

}

