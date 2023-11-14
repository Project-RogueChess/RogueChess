using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UpgradeArticle : MonoBehaviour
{
    public static UpgradeArticle instance;

    public int k = 0;

    private void Awake()
    {
        instance = this;

    }



    public void TryUpgradeArticle(Piece piece)
    {


        List<Pieces> article_Grade_1 = new List<Pieces>();
        List<Pieces> article_Grade_2 = new List<Pieces>();

        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {

                Pieces findPiece = InvSpawnManager.instance.invTiles[i].piece.GetComponent<Pieces>();
                if (findPiece.id == piece.id)
                {
                    if (findPiece.grade == 1)
                    {
                        article_Grade_1.Add(findPiece);
                    }
                    else if (findPiece.grade == 2)
                    {
                        Debug.Log(2);
                        article_Grade_2.Add(findPiece);
                    }
                }
            }
        }

        for (int i = 0; i < InvSpawnManager.instance.hexaTiles.Count; i++)
        {
            if (InvSpawnManager.instance.hexaTiles[i].piece != null)
            {

                Pieces findPiece = InvSpawnManager.instance.hexaTiles[i].piece.GetComponent<Pieces>();

                if (findPiece.id == piece.id)
                {
                    if (findPiece.grade == 1)
                    {
                        article_Grade_1.Add(findPiece);
                    }
                    else if (findPiece.grade == 2)
                    {
                        Debug.Log(1);
                        article_Grade_2.Add(findPiece);
                    }
                }
            }
        }

        

        if (article_Grade_1.Count > 2)
        {
            article_Grade_1[2].MergePeice();

            article_Grade_2.Add(article_Grade_1[2]);
            k = 0;

            for (int j = 0; j < 2; j++)
            {

                for (int i = 0; i < 3; i++)
                {
                    // 파괴 당할 오브젝트들이 아이템 가졌었는지 확인
                    if (article_Grade_1[j].gameObject.GetComponent<Pieces>().items[i].itemName != string.Empty)
                    {

                        for (int k = 0; k < 3; k++)
                        {
                            //머지할 오브젝트의 0~2까지 아이템 있는지 확인
                            if (article_Grade_1[2].gameObject.GetComponent<Pieces>().items[k].itemName == string.Empty)
                            {
                                GameObject itemImg = article_Grade_1[2].gameObject.transform.GetChild(0).gameObject.transform.GetChild(k).gameObject;
                                itemImg.SetActive(true);
                                article_Grade_1[2].gameObject.GetComponent<Pieces>().items[k] = article_Grade_1[j].gameObject.GetComponent<Pieces>().items[i];
                                itemImg.GetComponent<Image>().sprite = article_Grade_1[j].gameObject.GetComponent<Pieces>().items[i].itemSprite;
                                break;
                            }  //머지할 오브젝트의 아이템 있고 마지막 아이템 있으면 인벤토리에 추가
                            else if (article_Grade_1[2].gameObject.GetComponent<Pieces>().items[k].itemName != string.Empty && article_Grade_1[2].gameObject.GetComponent<Pieces>().items[2].itemName != string.Empty)
                            {
                                UIManager.instance.AddTheItem(article_Grade_1[j].gameObject.GetComponent<Pieces>().items[i]);
                                Debug.Log(2);
                                break;
                            }
                        }
                    }
                }
            }

            Destroy(article_Grade_1[0].gameObject);
            Destroy(article_Grade_1[1].gameObject);

            article_Grade_1.Clear();

            if (article_Grade_2.Count > 2)
            {
                article_Grade_2[0].MergePeice();
                k = 0;

                for (int j = 1; j < 3; j++)
                {

                    for (int i = 0; i < 3; i++)
                    {
                        // 파괴 당할 오브젝트들이 아이템 가졌었는지 확인
                        if (article_Grade_2[j].gameObject.GetComponent<Pieces>().items[i].itemName != string.Empty)
                        {

                            for (int k = 0; k < 3; k++)
                            {
                                //머지할 오브젝트의 0~2까지 아이템 있는지 확인
                                if (article_Grade_2[0].gameObject.GetComponent<Pieces>().items[k].itemName == string.Empty)
                                {
                                    GameObject itemImg = article_Grade_1[0].gameObject.transform.GetChild(0).gameObject.transform.GetChild(k).gameObject;
                                    itemImg.SetActive(true);
                                    article_Grade_2[0].gameObject.GetComponent<Pieces>().items[k] = article_Grade_2[j].gameObject.GetComponent<Pieces>().items[i];
                                    itemImg.GetComponent<Image>().sprite = article_Grade_2[j].gameObject.GetComponent<Pieces>().items[i].itemSprite;
                                    break;
                                }  //머지할 오브젝트의 아이템 있고 마지막 아이템 있으면 인벤토리에 추가
                                else if (article_Grade_2[0].gameObject.GetComponent<Pieces>().items[k].itemName != string.Empty && article_Grade_2[0].gameObject.GetComponent<Pieces>().items[2].itemName != string.Empty)
                                {
                                    UIManager.instance.AddTheItem(article_Grade_2[j].gameObject.GetComponent<Pieces>().items[i]);
                                    Debug.Log(2);
                                    break;
                                }
                            }
                        }
                    }
                }
                    
                Destroy(article_Grade_2[1].gameObject);
                Destroy(article_Grade_2[2].gameObject);

                article_Grade_2.Clear();
            }
        }
    }
}
