using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Stage", fileName = "Stage_")]
public class Stage : ScriptableObject
{
    [SerializeField]
    public List<스테이지정보> infos;
}

public class 스테이지정보
{
    public Gimul type;
    public Vector2Int index;
}
