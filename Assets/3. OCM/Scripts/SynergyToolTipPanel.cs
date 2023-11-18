using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyToolTipPanel : MonoBehaviour
{
    public Image image;
    public TMP_Text nameTxt;
    public TMP_Text desTxt;
    public TMP_Text des2Txt;

    public void SetupSynergyToolTip(Sprite icon,string name,string des,string injectDataStr,int injectDataNum)
    {
        image.sprite = icon;
        nameTxt.text = name;
        desTxt.text = $"Term : {des}";
        des2Txt.text = $"{injectDataStr} : {injectDataNum}" ;
    }
}
