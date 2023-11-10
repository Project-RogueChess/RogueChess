using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject spawnGO;

    public static ButtonSpawner instance;

    private void Start()
    {
        instance = this;
    }

    public int btnClick()
    {
        int index = -1;
        Tile testTile = FindTile(out index);

        if (testTile != null)
        {
            SpawnObjects(testTile);
            Debug.Log("기물생성");

        }
        else
        {
            Debug.Log("인벤꽉참");
        }

        return index;
    }

    Tile FindTile(out int index)
    {
        index = -1;

        for (int i = 0; i < InvSpawnManager.instance.invTiles.Count; i++)
        {
            if (InvSpawnManager.instance.invTiles[i].piece == null)
            {
                index = i;
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
