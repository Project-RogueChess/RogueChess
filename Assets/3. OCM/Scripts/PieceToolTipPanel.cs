using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceToolTip : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text hpTxt;
    public TMP_Text atkdmgTxt;
    public TMP_Text atkspdTxt;
    public Image img;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetupPieceToolTip(Sprite sprite, string name, int hp, int atkdmg, float atkspd)
    {
        img.sprite = sprite;
        nameTxt.text = "name: " + name;
        hpTxt.text = "hp: " + hp.ToString();
        atkdmgTxt.text = "atkdmg: " + atkdmg.ToString();
        atkspdTxt.text = "atkspd: " + atkspd.ToString();
    }
}
