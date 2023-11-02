using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public TMP_Text levelTxt;
    public TMP_Text expTxt;
    public TMP_Text hpTxt;
    public TMP_Text stateTxt;
    public TMP_Text piecesTxt;
    public TMP_Text goldTxt;
    


    public GameObject shopPanel;
    public GameObject RerollButton;
    public bool shopOnOffBool;
    public float translateTime = 2.0f;
    public Vector3 originScale = Vector3.one;
    public Vector3 zeroScale = new Vector3 (0f, 0f, 0f);
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        originScale = transform.localScale;
        UIRefresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            ShopUIDraw();
            shopOnOffBool = !shopOnOffBool;
        }
    }

    public void UIRefresh()
    {
        levelTxt.text = "Level: " + DataManager.instance.myLevel.ToString();
        expTxt.text = DataManager.instance.myExp.ToString() + "/" + DataManager.instance.maxExp.ToString();
        hpTxt.text = DataManager.instance.myHp.ToString() + "/ 15";
        piecesTxt.text = DataManager.instance.myPieces.ToString() + "/" + DataManager.instance.maxPieces.ToString();
        goldTxt.text = "$" + DataManager.instance.myGold.ToString();
    }

    public void ShopUIDraw()
    {
        if (shopOnOffBool == true)
        {
            shopPanel.SetActive(false);
            RerollButton.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(true);
            RerollButton.SetActive(true);
        }
        
    }
}
