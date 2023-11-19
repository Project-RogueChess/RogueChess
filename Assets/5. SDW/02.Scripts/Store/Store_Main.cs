using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store_Main : MonoBehaviour
{
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject[] images;

    [Header("ItemSprite")]
    [SerializeField] private Sprite[] itemSprites;

    private int[] itemBuyValue = new int[3];


    private void Start()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.SetActive(false);
    }

    public void StoreNodeStart()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            int randomValue = Random.Range(0, itemSprites.Length);
            itemBuyValue[i] = randomValue;
            images[i].GetComponent<Image>().sprite = itemSprites[randomValue];
            images[i].GetComponent<StoreItemToolTip>().itemObjectNum = randomValue;
        }

    }


    #region ClickEvent
    public void ClickedExitButton()
    {
        questUI.SetActive(true);
        questUI.GetComponent<QuestUI>().SetObject(gameObject);

        gameObject.SetActive(false);
        SoundManager.instance.PlaySound("PageFlipOff");
    }

    public void ClickedBuyButton1()
    {
        if (DataManager.instance.TryLostGold(4) == false)
        {
            return;
        }

        UIManager.instance.AddNumItem(itemBuyValue[0]);
        ExitStoreEvent();
    }
    public void ClickedBuyButton2()
    {
        if (DataManager.instance.TryLostGold(4) == false)
        {
            return;
        }

        UIManager.instance.AddNumItem(itemBuyValue[1]);
        ExitStoreEvent();
    }
    public void ClickedBuyButton3()
    {
        if (DataManager.instance.TryLostGold(4) == false)
        {
            return;
        }

        UIManager.instance.AddNumItem(itemBuyValue[2]);
        ExitStoreEvent();
    }

    public void StoreExitButton()
    {
        ExitStoreEvent();
    }
    #endregion

    private void ExitStoreEvent()
    {
        GameManager.instance.isNodeComplete = true;
        gameObject.SetActive(false);
        //노드 종료
    }
}
