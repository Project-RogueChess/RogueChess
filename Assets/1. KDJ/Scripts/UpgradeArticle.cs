using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeArticle : MonoBehaviour
{
    public static UpgradeArticle instance;

    public List<Pieces> article_Grade_1 = new List<Pieces>();
    public List<Pieces> article_Grade_2 = new List<Pieces>();

    private void Awake()
    {
        instance = this;
        
    }



    public void TryUpgradeArticle(Piece piece)
    {
       


        for (int i = 0; i < InvSpawnManager.instance.hexaTiles.Count; i++)
        {
            if (InvSpawnManager.instance.hexaTiles[i].piece != null)
            {
                
                Pieces findPiece = InvSpawnManager.instance.hexaTiles[i].piece.GetComponent<Pieces>();
                
                if(findPiece.id == piece.id)
                {
                    if(findPiece.grade == 1)
                    {
                        article_Grade_1.Add(findPiece);
                    }
                    else if(findPiece.grade == 2)
                    {
                        article_Grade_2.Add(findPiece);
                    }
                }

            }
        }

        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {

                Pieces findPiece = InvSpawnManager.instance.invTiles[i].piece.GetComponent<Pieces>();
                Debug.Log(findPiece.id);
                Debug.Log(piece.id);
                if (findPiece.id == piece.id)
                {
                    if (findPiece.grade == 1)
                    {
                        article_Grade_1.Add(findPiece);
                    }
                    else if (findPiece.grade == 2)
                    {
                        article_Grade_2.Add(findPiece);
                    }
                }
            }
        }


        if (article_Grade_1.Count > 2)
        {
            

            article_Grade_1[2].MergePeice();


            Destroy(article_Grade_1[0].gameObject);
            Destroy(article_Grade_1[1].gameObject);

            article_Grade_1.Clear();

            if (article_Grade_2.Count > 2)
            {

                article_Grade_2[2].MergePeice();


                Destroy(article_Grade_2[0].gameObject);
                Destroy(article_Grade_2[1].gameObject);

                article_Grade_2.Clear();

            }


        }

    }

}
