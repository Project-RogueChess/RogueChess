using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance {  get; private set; }


    public int maxHp = 0;
    public int myHp = 0;
    public int myGold = 0;
    public int maxPieces =0;
    public int myPieces = 0;
    public int myLevel = 0;
    public int maxExp = 0;
    public int myExp = 0;


    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DataReset();
    }

    public void GettingExp()
    {
        myExp += 4;
        if(myExp>= maxExp)
        {
            LevelUP();
        }
        UIManager.instance.UIRefresh();
    }

    public void LevelUP()
    {
        myExp -= maxExp;
        myLevel += 1;
        maxExp = 4 * myLevel;
    }

    public void DataReset()
    {
        myLevel = 1;
        maxExp = 4;
        myExp = 0;
        maxHp = 15;
        myHp = 15;
        myGold = 30;
        maxPieces = 3;
        myPieces = 0;
    }

}
