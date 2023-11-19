using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        List<KeyValuePair<Pieces, Tile>> article_Grade_1 = new List<KeyValuePair<Pieces, Tile>>();
        List<KeyValuePair<Pieces, Tile>> article_Grade_2 = new List<KeyValuePair<Pieces, Tile>>();

        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {

                Pieces findPiece = InvSpawnManager.instance.invTiles[i].piece.GetComponent<Pieces>();
                if (findPiece.id == piece.id)
                {
                    if (findPiece.grade == 1)
                    {
                        article_Grade_1.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.invTiles[i]));
                    }
                    else if (findPiece.grade == 2)
                    {
                        Debug.Log(1);
                        article_Grade_2.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.invTiles[i]));
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
                        article_Grade_1.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.hexaTiles[i]));
                    }
                    else if (findPiece.grade == 2)
                    {
                        article_Grade_2.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.hexaTiles[i]));
                    }
                }
            }
        }

        



        if (article_Grade_1.Count > 2)
        {


            article_Grade_2.Add(article_Grade_1[2]);


            k = 0;

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    // 파괴 당할 오브젝트들이 아이템 가졌었는지 확인
                    if (article_Grade_1[j].Key.items[i].itemName != string.Empty)
                    {

                        for (int k = 0; k < 3; k++)
                        {
                            //머지할 오브젝트의 0~2까지 아이템 있는지 확인
                            if (article_Grade_1[2].Key.items[k].itemName == string.Empty)
                            {
                                GameObject itemImg = article_Grade_1[2].Key.gameObject.transform.GetChild(0).gameObject.transform.GetChild(k).gameObject;
                                itemImg.SetActive(true);
                                article_Grade_1[2].Key.items[k] = article_Grade_1[j].Key.items[i];
                                itemImg.GetComponent<Image>().sprite = article_Grade_1[j].Key.items[i].itemSprite;
                                break;
                            }  //머지할 오브젝트의 아이템 있고 마지막 아이템 있으면 인벤토리에 추가
                            else if (article_Grade_1[2].Key.items[2].itemName != string.Empty)
                            {
                                UIManager.instance.AddTheItem(article_Grade_1[j].Key.items[i]);
                                Debug.Log(2);
                                break;
                            }
                        }
                    }
                }
            }

            article_Grade_1[2].Key.MergePeice();

            HexaUnitManager.instance.UnRegisterHexaUnit(article_Grade_1[0].Key.GetComponent<HexaUnit>());
            HexaUnitManager.instance.UnRegisterHexaUnit(article_Grade_1[1].Key.GetComponent<HexaUnit>());

            article_Grade_1[0].Value.piece = null;
            article_Grade_1[1].Value.piece = null;

            DestroyImmediate(article_Grade_1[0].Key.gameObject);
            DestroyImmediate(article_Grade_1[1].Key.gameObject);

            article_Grade_1.Clear();

            if (article_Grade_2.Count > 2)
            {

                article_Grade_1.Clear();
                article_Grade_2.Clear();

                k = 0;


                for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
                {
                    if (InvSpawnManager.instance.invTiles[i].piece != null)
                    {

                        Pieces findPiece = InvSpawnManager.instance.invTiles[i].piece.GetComponent<Pieces>();
                        if (findPiece.id == piece.id)
                        {
                            if (findPiece.grade == 1)
                            {
                                article_Grade_1.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.invTiles[i]));
                            }
                            else if (findPiece.grade == 2)
                            {
                                article_Grade_2.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.invTiles[i]));
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
                                article_Grade_1.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.hexaTiles[i]));
                            }
                            else if (findPiece.grade == 2)
                            {
                                article_Grade_2.Add(new KeyValuePair<Pieces, Tile>(findPiece, InvSpawnManager.instance.hexaTiles[i]));
                            }
                        }
                    }
                }




                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // 파괴 당할 오브젝트들이 아이템 가졌었는지 확인
                        if (article_Grade_2[j].Key.items[i].itemName != string.Empty)
                        {

                            for (int k = 0; k < 3; k++)
                            {
                                //머지할 오브젝트의 0~2까지 아이템 있는지 확인
                                if (article_Grade_2[2].Key.items[k].itemName == string.Empty)
                                {
                                    GameObject itemImg = article_Grade_2[2].Key.gameObject.transform.GetChild(0).gameObject.transform.GetChild(k).gameObject;
                                    itemImg.SetActive(true);
                                    article_Grade_2[2].Key.items[k] = article_Grade_2[j].Key.items[i];
                                    itemImg.GetComponent<Image>().sprite = article_Grade_2[j].Key.items[i].itemSprite;
                                    break;
                                }  //머지할 오브젝트의 아이템 있고 마지막 아이템 있으면 인벤토리에 추가
                                else if (article_Grade_2[2].Key.items[2].itemName != string.Empty)
                                {
                                    UIManager.instance.AddTheItem(article_Grade_2[j].Key.items[i]);
                                    Debug.Log(2);
                                    break;
                                }
                            }
                        }
                    }
                }

                article_Grade_2[2].Key.MergePeice();

                HexaUnitManager.instance.UnRegisterHexaUnit(article_Grade_2[0].Key.GetComponent<HexaUnit>());
                HexaUnitManager.instance.UnRegisterHexaUnit(article_Grade_2[1].Key.GetComponent<HexaUnit>());

                article_Grade_2[0].Value.piece = null;
                article_Grade_2[1].Value.piece = null;

                DestroyImmediate(article_Grade_2[0].Key.gameObject);
                DestroyImmediate(article_Grade_2[1].Key.gameObject);


                article_Grade_2.Clear();

                
            }
        }
    }

}
