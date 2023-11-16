using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiecesList : MonoBehaviour
{
    // 지금 당장 생각 나는 것은 우리가 기물을 샀을 때 놓는 타일들을 배열로 받아서 위에 레이캐스트로 쏘고 있는지 확인해서 생성하는걸 생각함
    public Transform[] createpieceLocation;


    // 코스트 별 기물들의 리스트
    public GameObject[] gold1Pieces = new GameObject[5];
    //public GameObject[] gold2Pieces = new GameObject[5];
    //public GameObject[] gold3Pieces = new GameObject[5];
    //public GameObject[] gold4Pieces = new GameObject[5];
    //public GameObject[] gold5Pieces = new GameObject[5];
}
