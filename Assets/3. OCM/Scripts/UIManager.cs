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


    public GameObject inventory1;
    public GameObject inventory2;
    public GameObject inventoryLeftBtn;
    public GameObject inventoryRightBtn;
    public bool inventoryOnOffBool;
    public bool inventorySwitchBool;
    private InventoryPanel inventoryPanel1;
    private InventoryPanel inventoryPanel2;


    public GameObject mapObject;

    public int itemAddNum;
    public TMP_Text itemAddNumTxt;
    public GameObject itemAddNumImg;
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
        shopOnOffBool = true;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUIDraw();
            inventoryOnOffBool = !inventoryOnOffBool;
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

    public void InventoryUIDraw()
    {
        if (inventoryOnOffBool == true)
        {
            inventory1.SetActive(false);
            inventory2.SetActive(false);
            inventoryLeftBtn.SetActive(false);
            inventoryRightBtn.SetActive(false);
            inventorySwitchBool = false;
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
        }
    }

    public void InventoryUISwitch()
    {
        if (inventorySwitchBool == true)
        {
            inventory2.SetActive(true);
            inventory1.SetActive(false);
        }
        else
        {
            inventory1.SetActive(true);
            inventory2.SetActive(false);
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
                    inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[9+j];
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
                if (inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty)
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
                    if (inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == string.Empty ||
                       inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item.itemName == null)
                    {
                        inventoryPanel1.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel1.itemsDB.items[i];
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
                        inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().itemGO.GetComponent<ItemObject>()._item = inventoryPanel2.itemsDB.items[i];
                        inventoryPanel2.inventorySlots[j].GetComponent<InventorySlot>().isSlotEmpty = false;
                        ShowAddItemNum();
                        return;
                    }
                }
            }
        }
    }

    public void ShowAddItemNum()
    {
        if(inventoryOnOffBool == false)
        {
            itemAddNumTxt.enabled = true;
            itemAddNumImg.SetActive(true);
            itemAddNum++;
            itemAddNumTxt.text = itemAddNum.ToString();
        }
    }
}
