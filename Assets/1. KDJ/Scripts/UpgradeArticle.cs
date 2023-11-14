using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeArticle : MonoBehaviour
{
    public static UpgradeArticle instance;

    private void Awake()
    {
        instance = this;
    }



    public void TryUpgradeArticle(Pieces piece)
    {
        List<Pieces> article_Grade_1 = new List<Pieces>();
        List<Pieces> article_Grade_2 = new List<Pieces>();


        for (int i = 0; i < InvSpawnManager.instance.hexaTiles.Count; i++)
        {
            if (InvSpawnManager.instance.hexaTiles[i].piece != null)
            {
                
                Piece piece1 = InvSpawnManager.instance.hexaTiles[i].GetComponent<Piece>();
                
                /*if(piece.id == )
                {

                }*/

            }
        }

        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece != null)
            {




            }
        }


    }

}
