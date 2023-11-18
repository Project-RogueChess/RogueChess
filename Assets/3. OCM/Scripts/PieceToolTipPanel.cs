using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PieceToolTip : MonoBehaviour
{
    public TMP_Text nameTxt;
    public TMP_Text hpTxt;
    public TMP_Text atkdmgTxt;
    public TMP_Text atkspdTxt;
    public Image img;
    public Image itemImg1;
    public Image itemImg2;
    public Image itemImg3;
    

    public void SetupPieceToolTip(Sprite sprite,Sprite itemSprite1, Sprite itemSprite2, Sprite itemSprite3, string name, int hp, int atkdmg, float atkspd)
    {
        img.sprite = sprite;
        if (itemSprite1 != null)
        {
            itemImg1.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            itemImg1.sprite = itemSprite1;
        }
        else
        {
            itemSprite1 = sprite;
            itemImg1.gameObject.GetComponent<CanvasGroup>().alpha = 0 ;
        }
        if (itemSprite2 != null)
        {
            itemImg2.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            itemImg2.sprite = itemSprite2;
        }
        else
        {
            itemSprite2 = sprite;
            itemImg2.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (itemSprite3 != null)
        {
            itemImg3.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            itemImg3.sprite = itemSprite3;
        }
        else
        {
            itemSprite3 = sprite;
            itemImg3.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
      
      
        nameTxt.text = name;
        hpTxt.text = "Hp: " + hp.ToString();
        atkdmgTxt.text = "AtkDmg: " + atkdmg.ToString();
        atkspdTxt.text = "AtkSpd: " + atkspd.ToString();
    }

    
}
