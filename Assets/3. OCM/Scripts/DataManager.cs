using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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


    public int[] wholePercentage= new int[4];

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DataReset();
        UIManager.instance.UIRefresh();
    }

    public void GettingExp()
    {
        if(myLevel == 10)
        {
            return;
        }
        if(myGold >= 4)
        {
            myExp += 4;
            if (myExp >= maxExp)
            {
                LevelUP();
                DistributePercentage();
                
            }
        }
        UIManager.instance.UIRefresh();
    }

    public void LevelUP()
    {
        if(myLevel >= 10)
        {
            myExp = 0;
            maxExp = 0;
            return;
        }
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
        wholePercentage = new int[]{ 100, 0, 0, 0 };
    }

    public void DistributePercentage()
    {
        switch (myLevel)
        {
            case 1:
                {
                    wholePercentage = new int[] { 100,0,0,0 };
                    break;
                }
            case 2:
                {
                    wholePercentage = new int[] { 70, 30, 0, 0 };
                    break;
                }
            case 3:
                {
                    wholePercentage = new int[] { 60, 35, 5, 0 };
                    break;
                }
            case 4:
                {
                    wholePercentage = new int[] { 50, 35, 15, 0 };
                    break;
                }
                case 5:
                {
                    wholePercentage = new int[] { 40, 35, 23, 2 };
                    break;
                }
                case 6:
                {
                    wholePercentage = new int[] { 33, 30, 30, 7 };
                    break;
                }
                case 7:
                {
                    wholePercentage = new int[] { 30, 30, 30, 10 };
                    break;
                }
                case 8:
                {
                    wholePercentage = new int[] { 23, 30, 30, 15 };
                    break;
                }
                case 9:
                {
                    wholePercentage = new int[] { 21, 30, 25, 20 };
                    break;
                }
                case 10:
                {
                    wholePercentage = new int[] { 19, 25, 25,25 };
                    break;
                }
        }
    }
}
