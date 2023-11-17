using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapTriggerInfo : MonoBehaviour
{
    public TileType type;
    public int x, y = -1;
}

public enum TileType
{
    Hexa = 0,
    Inv = 1
}