using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvSpawnManager : MonoBehaviour
{
    public GameObject spawnUnit;

    public void SpawnUnit(Tile tile)
    {
        tile.piece = Instantiate(spawnUnit, tile.transform.position , Quaternion.identity);
    }

}   
