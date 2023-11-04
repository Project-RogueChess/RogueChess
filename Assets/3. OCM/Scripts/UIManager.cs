using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    //상단 panel에서 보여주는 Text
    public TMP_Text levelTxt;
    public TMP_Text expTxt;
    public TMP_Text hpTxt;
    public TMP_Text stateTxt;
    public TMP_Text piecesTxt;
    public TMP_Text goldTxt;
    //기물 확률 panel
    public TMP_Text[] percentages = new TMP_Text[5]; 
    

    public GameObject shopPanel;
    public GameObject rerollButton;
    public GameObject percentagePanel;
    public GameObject rerollLockButton;
    public GameObject rerollLockImg;
    public ShopManager[] shopmanagers = new ShopManager[5];

    public bool shopOnOffBool;

    

    private void Awake()
    {
        instance = this;
        
        shopmanagers = shopPanel.GetComponentsInChildren<ShopManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            ShopUIDraw();
            shopOnOffBool = !shopOnOffBool;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DataManager.instance.GettingExp();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReRoolForBtn();
        }
    }

    //UI 보여주는 것
    public void UIRefresh()
    {
        levelTxt.text = "Level: " + DataManager.instance.myLevel.ToString();
        expTxt.text = DataManager.instance.myExp.ToString() + "/" + DataManager.instance.maxExp.ToString();
        hpTxt.text = DataManager.instance.myHp.ToString() + "/ 15";
        piecesTxt.text = DataManager.instance.myPieces.ToString() + "/" + DataManager.instance.maxPieces.ToString();
        goldTxt.text = "$" + DataManager.instance.myGold.ToString();
        for (int i = 0; i< DataManager.instance.wholePercentage.Length;i++)
        {
            percentages[i].text = DataManager.instance.wholePercentage[i].ToString();
        }
    }

    //상점 UI 열고 닫기
    public void ShopUIDraw()
    {
        if (shopOnOffBool == true)
        {
            shopPanel.SetActive(false);
            rerollButton.SetActive(false);
            percentagePanel.SetActive(false);
            rerollLockButton.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(true);
            rerollButton.SetActive(true);
            percentagePanel.SetActive(true);
            rerollLockButton.SetActive(true);
        }
        
    }

    public void ReRoolForBtn()
    {
        if (DataManager.instance.reroolLock == true)
        {
            return;
        }
        else
        {
            if (DataManager.instance.myGold >= 2)
            {
                DataManager.instance.myGold -= 2;
                foreach (ShopManager shopManager in shopmanagers)
                {
                    shopManager.ReRoll(DataManager.instance.reroolLock);
                }
                UIRefresh();
            }
        }
    }

    public void ImageOnOff()
    {
        if(DataManager.instance.reroolLock == true)
        {
            rerollLockImg.SetActive(true);
        }
        else
        {
            rerollLockImg.SetActive(false);
        }
    }
}
