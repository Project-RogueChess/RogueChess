using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvSpawnManager : MonoBehaviour
{
    public static InvSpawnManager instance;

    public int articleCount;

    public GameObject spawnUnit;

    public List<Tile> hexaTiles = new List<Tile>();
    public List<Tile> invTiles = new List<Tile>();


    private void Awake()
    {
        instance = this;
    }

    public void SpawnUnit(Tile tile)
    {
        if(TileType.Inv == tile.triggerInfo.type)
        tile.piece = Instantiate(spawnUnit, tile.transform.position , Quaternion.identity);
    }

    public void ResetChampions()
    {
        
        for (int i = 0; i < hexaTiles.Count; i++)
        {
            if (hexaTiles[i].piece != null)
            {
                hexaTiles[i].Reset();
            }
        }
    }
    public void CountArticle()
    {
        articleCount = 0;

        for (int i = 0; i < hexaTiles.Count; i++)
        {
            if (hexaTiles[i].piece != null)
            {
                articleCount++;

                Debug.Log(articleCount);
            }
        }
    }


}   
