using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject spawnGO;



    public void btnClick()
    {
        Tile testTile = FindTile();

        if (testTile != null)
        {
            SpawnObjects(testTile);
            Debug.Log("기물생성");

        }
        else
        {
            Debug.Log("인벤꽉참");
        }
    }

    Tile FindTile()
    {
        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece == null)
            {
                return InvSpawnManager.instance.invTiles[i];
            }
        }
        return null;
    }

    public void SpawnObjects(Tile tile)
    {
        InvSpawnManager.instance.SpawnUnit(tile);
    }
}
