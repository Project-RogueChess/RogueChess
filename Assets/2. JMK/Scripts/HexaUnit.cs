using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HexaUnit : MonoBehaviour
{
    private Vector2Int _gridIndex;
    private int _team;
    private int _range;
    private bool _actEnable;
    private float _actRate;
    private HexaUnit _target;

    public Vector2Int gridIndex => _gridIndex;
    public int team => _team;
    public int range => _range;
    public bool actEnable => _actEnable;
    public float actRate => _actRate;
    public HexaUnit target => _target;
}
