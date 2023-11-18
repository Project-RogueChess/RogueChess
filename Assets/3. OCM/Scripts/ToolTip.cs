using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ToolTip : MonoBehaviour
{
    public Image image;
    public TMP_Text nameTxt;
    public TMP_Text hpTxt;
    public TMP_Text atkdmgTxt;
    public TMP_Text atkspdTxt;


    public void SetupItemToolTip(Sprite sprite, string name, int hp, int atkdmg, float atkspd)
    {
        image.sprite = sprite;
        nameTxt.text =  name;
        hpTxt.text = "hp: " + hp.ToString();
        atkdmgTxt.text = "atkdmg: " + atkdmg.ToString();
        atkspdTxt.text = "atkspd: " + atkspd.ToString();
    }
}
    

   

   
    
