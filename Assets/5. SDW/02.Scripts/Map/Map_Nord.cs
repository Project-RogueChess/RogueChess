using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map_Nord : MonoBehaviour
{
    [SerializeField] private int _locationX;
    [SerializeField] private int _locationY;
    public int locationX
    {
        get
        {
            return _locationX;
        }
        set
        {
            _locationX = value;
        }
    }

    public int locationY
    {
        get
        {
            return _locationY;
        }

        set
        {
            _locationY = value;
        }
    }
}
