using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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
    public GameObject UpRerollPanel;
    public ShopManager[] shopmanagers = new ShopManager[5];

    public bool shopOnOffBool;


    public GameObject inventory1;
    public GameObject inventory2;
    public GameObject inventoryLeftBtn;
    public GameObject inventoryRightBtn;
    public bool inventoryOnOffBool;
    public bool inventorySwitchBool;
    private InventoryPanel inventoryPanel1;
    private InventoryPanel inventoryPanel2;
    public TMP_Text inventoryNumTxt;


    public GameObject mapObject;

    public int itemAddNum;
    public TMP_Text itemAddNumTxt;
    public GameObject itemAddNumImg;


    public TMP_Text sellTxt;
    public GameObject[] shopManagerList;


    
    private void Awake()
    {
        instance = this;

        shopmanagers = shopPanel.GetComponentsInChildren<ShopManager>();
        inventoryPanel1 = inventory1.GetComponent<InventoryPanel>();
        inventoryPanel2 = inventory2.GetComponent<InventoryPanel>();
        inventory1.SetActive(true);
        inventory2.SetActive(true);
        inventory1.SetActive(false);
        inventory2.SetActive(false);
        itemAddNumTxt.enabled = false;
        itemAddNumImg.SetActive(false);
        //shopOnOffBool = true;

        shopManagerList = shopmanagers.Select(shopmanagers => shopmanagers.gameObject).ToArray();
        //for(int i =0; i< shopmanagers.Length; i++)
        //{
        //    shopManagerList[i] = shopmanagers[i].gameObject;
        //}


        sellTxt.enabled = false;
    }


    private void Start()
    {
        ShopUIDraw(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            shopOnOffBool = !shopOnOffBool;
            ShopUIDraw(shopOnOffBool);
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DataManager.instance.GettingExp();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReRoolForBtn();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUIDraw();

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddItem();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddRandomItem();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapObject.GetComponent<Main_Map>().UI_MapOnOff();
            //if (GameManager.Instance.currentPhase == Phase.SelectMapNode)
            //{
            //    mapObject.GetComponent<Main_Map>().UI_MapOnOff();
            //}

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            AddNumItem(3);
        }

    }

    public void ShowShop()
    {
        ShopUIDraw(true);
    }

    public void HideShop()
    {
        ShopUIDraw(false);
    }

    //UI 보여주는 것
    public void UIRefresh()
    {
        levelTxt.text = "Level: " + DataManager.instance.WhatMyLevel().ToString();
        expTxt.text = DataManager.instance.WhatMyEXP().ToString() + "/" + DataManager.instance.WhatMyMAXEXP().ToString();
        hpTxt.text = DataManager.instance.WhatMyHp().ToString() + "/ 15";
        piecesTxt.text = DataManager.instance.WhatMyPieces().ToString() + "/" + DataManager.instance.WhatMyMAXPieces().ToString();
        goldTxt.text = "$" + DataManager.instance.WhatMyGold().ToString();
        for (int i = 0; i < DataManager.instance.wholePercentage.Length; i++)
        {
            percentages[i].text = DataManager.instance.wholePercentage[i].ToString() + "%";
        }
        if (DataManager.instance.WhatMyPieces() > DataManager.instance.WhatMyMAXPieces())
        {
            piecesTxt.color = new(1, 0, 0);
        }
        else
        {
            piecesTxt.color = new(1, 1, 1);
        }
    }

    //상점 UI 열고 닫기
    public void ShopUIDraw(bool showing)
    {
        
        shopPanel.SetActive(showing);
        rerollButton.SetActive(showing);
        percentagePanel.SetActive(showing);
        rerollLockButton.SetActive(showing);
        UpRerollPanel.SetActive(showing);
    }

    public void ReRoolForBtn()
    {
        if (DataManager.instance.reroolLock == true)
        {
            return;
        }
        else
        {
            if (DataManager.instance.WhatMyGold() >= 2)
            {
                DataManager.instance.LostGold(2);
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
        if (DataManager.instance.reroolLock == true)
        {
            rerollLockImg.SetActive(true);
        }
        else
        {
            rerollLockImg.SetActive(false);
        }
    }

    public void InventoryUIDraw()
    {
        if (inventoryOnOffBool == true)
        {
            inventory1.SetActive(false);
            inventory2.SetActive(false);
            inventoryLeftBtn.SetActive(false);
            inventoryRightBtn.SetActive(false);
            inventorySwitchBool = false;
            inventoryNumTxt.enabled = false;
        }
        else
        {
            inventory1.SetActive(true);
            inventory2.SetActive(false);
            inventoryLeftBtn.SetActive(true);
            inventoryRightBtn.SetActive(true);
            itemAddNum = 0;
            itemAddNumTxt.enabled = false;
            itemAddNumImg.SetActive(false);
            inventorySwitchBool = true;
            inventoryNumTxt.enabled = true;
            inventoryNumTxt.text = "1/2";
            UIRefresh();
        }
        inventoryOnOffBool = !inventoryOnOffBool;
    }

    public void InventoryUISwitch()
    {
        if (inventorySwitchBool == true)
        {
            inventory2.SetActive(true);
            inventory1.SetActive(false);
            inventoryNumTxt.text = "2/2";
            UIRefresh();
        }
        else
        {
            inventory1.SetActive(true);
            inventory2.SetActive(false);
            inventoryNumTxt.text = "1/2";
            UIRefresh();
        }
        inventorySwitchBool = !inventorySwitchBool;
    }

    public void AddItem()
    {

        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel1.itemsDB.items[j];
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[9 + j];
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }


    }
    public void AddRandomItem()
    {
        int k = Random.Range(0, inventoryPanel1.itemsDB.items.Count);
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel1.itemsDB.items[k];
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[k];
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }
    }
    public void AddTheItem(Item item)
    {
        for (int i = 0; i < inventoryPanel1.itemsDB.items.Count; i++)
        {
            if (item.id == inventoryPanel1.itemsDB.items[i].id)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
                    {
                        inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel1.itemsDB.items[i];
                        inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                        ShowAddItemNum();
                        return;
                    }
                }
                for (int j = 0; j < 9; j++)
                {
                    if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
                    {
                        inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[i];
                        inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                        ShowAddItemNum();
                        return;
                    }
                }
            }
        }
    }

    public void DeleteRandomItem()
    {
        List<Item> items = new List<Item>();

        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName != string.Empty &&
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName != null)
            {
                items.Add(inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item);
            }
        }
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName != string.Empty &&
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName != null)
            {
                items.Add(inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item);
            }
        }
        int k = Random.Range(0, items.Count);
        Debug.Log(items.Count);
        Debug.Log(k);

        if (items.Count >= 1)
        {
            for (int j = 0; j < 9; j++)
            {
                if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item == items[k])
                {
                    inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = new Item();
                    inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = true;
                    inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemSprite = null;

                    return;
                }
            }
            for (int j = 0; j < 9; j++)
            {
                if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item == items[k])
                {
                    inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = new Item();
                    inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = true;
                    inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemSprite = null;

                    return;
                }
            }
        }
    }

    public void ShowAddItemNum()
    {
        if (inventoryOnOffBool == false)
        {
            itemAddNumTxt.enabled = true;
            itemAddNumImg.SetActive(true);
            itemAddNum++;
            itemAddNumTxt.text = itemAddNum.ToString();
        }
    }

    public void ShowSellText(GameObject pieces)
    {
        for (int i = 0; i < shopManagerList.Length; i++)
        {
            shopManagerList[i].SetActive(false);
        }
        int gold = pieces.GetComponent<Pieces>().gold;
        sellTxt.enabled = true;
        sellTxt.text = "Sell for " + gold.ToString() + "$";
    }

    public void CloseSellText()
    {
        for (int i = 0; i < shopManagerList.Length; i++)
        {
            shopManagerList[i].SetActive(true);
        }
        sellTxt.enabled = false;
    }



    public void AddNumItem(int itemnum)
    {
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel1.itemsDB.items[itemnum];
                inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }
        for (int j = 0; j < 9; j++)
        {
            if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
            {
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[itemnum];
                inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                ShowAddItemNum();
                return;
            }
        }
    }



}
