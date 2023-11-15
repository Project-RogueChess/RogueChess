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
    public int myGold = 1;
    public int maxPieces = 0;
    public int myPieces = 0;
    public int myLevel = 0;
    public int maxExp = 0;
    public int myExp = 0;
    public int[] wholePercentage = new int[5];
    public bool reroolLock = false;

    private void Awake()
    {
        instance = this;
        DataReset();
    }

    private void Start()
    {
        UIManager.instance.UIRefresh();
    }

    
    //°æÇèÄ¡ È¹µæ ¹öÆ° ´­·¶À» ¶§ ÀÛµ¿ ÇÔ¼ö
    public void GettingExp()
    {
        
            if (myLevel == 10)
            {
                return;
            }
            else
            {
                if (myGold >= 4)
                {
                    myGold -= 4;
                    myExp += 4;
                    if (myExp >= maxExp)
                    {
                        LevelUP();
                        DistributePercentage();
                    }
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
        maxPieces += 1;
    }

    public void DataReset()
    {
        myLevel = 1;
        maxExp = 4;
        myExp = 0;
        maxHp = 15;
        myHp = 15;
        myGold = 1000;
        maxPieces = 3;
        myPieces = 0;
        wholePercentage = new int[]{ 100, 0, 0, 0,0 };
    }

    //±â¹° È®·ü
    public void DistributePercentage()
    {
        switch (myLevel)
        {
            case 1:
                {
                    wholePercentage = new int[] { 100,0,0,0,0 };
                    break;
                }
            case 2:
                {
                    wholePercentage = new int[] { 70, 30, 0, 0,0 };
                    break;
                }
            case 3:
                {
                    wholePercentage = new int[] { 65, 30, 5, 0,0 };
                    break;
                }
            case 4:
                {
                    wholePercentage = new int[] { 55, 30, 15, 0 ,0};
                    break;
                }
                case 5:
                {
                    wholePercentage = new int[] { 45, 30, 15, 0 ,0};
                    break;
                }
                case 6:
                {
                    wholePercentage = new int[] { 25, 40, 30, 5 ,0};
                    break;
                }
                case 7:
                {
                    wholePercentage = new int[] { 19, 30,35, 15 ,1};
                    break;
                }
                case 8:
                {
                    wholePercentage = new int[] { 16, 20, 35, 25 ,4};
                    break;
                }
                case 9:
                {
                    wholePercentage = new int[] { 5, 10, 20, 40 ,25};
                    break;
                }
                case 10:
                {
                    wholePercentage = new int[] { 1, 2, 12,50 ,35};
                    break;
                }
        }
    }
    public void SwitchReRoolLock()
    {
        reroolLock = !reroolLock;
        UIManager.instance.ImageOnOff();
    }
}
