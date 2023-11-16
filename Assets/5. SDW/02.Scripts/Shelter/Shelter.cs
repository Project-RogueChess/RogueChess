using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour
{
    //[SerializeField] private GameObject centerPos;
    private void Start()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.SetActive(false);
    }
    public void ClickedExitButton()
    {
        gameObject.SetActive(false);
    }

    public void ClickedEnterShelter()
    {
        if (DataManager.instance.LostGold(2) == false)
        {
            return;
        }

        DataManager.instance.GetHp(2);
        ExitShelterNode();
    }

    public void ClickedLeaveShelter()
    {
        ExitShelterNode();
    }

    private void ExitShelterNode()
    {
        gameObject.SetActive(false);
        //게임 매니저한테 컴플리트
        //지도창 켜기
    }
}
