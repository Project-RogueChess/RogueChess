using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DataManager : MonoBehaviour
{
    public static DataManager instance {  get; private set; }


    [SerializeField] private int maxHp = 15;
    [SerializeField] private int myHp = 15;
    [SerializeField] private int myGold = 1000;
    [SerializeField] private int maxPieces = 3;
    [SerializeField] private int myPieces = 0;
    [SerializeField] private int myLevel = 1;
    [SerializeField] private int maxExp = 4;
    [SerializeField] private int myExp = 0;

    public int[] wholePercentage = new int[] { 100, 0, 0, 0, 0 };
    public bool reroolLock = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.UIRefresh();
    }

    #region External Call == Get,Lost
    public void LostHp(int damage)
    {
        myHp = myHp - damage;

        if (myHp < 0)
        {
            //ÇÃ·¹ÀÌ¾î »ç¸Á
        }

        UIManager.instance.UIRefresh();
    }

    public void GetHp(int recoveryHp)
    {
        myHp = myHp + recoveryHp;

        if (myHp > maxHp)
        {
            myHp = maxHp;
        }

        UIManager.instance.UIRefresh();
    }

    public bool LostGold(int lostGold)
    {
        int falseMyGold = myGold - lostGold;

        if (falseMyGold < 0)
        {
            return false;
        }

        myGold = falseMyGold;

        UIManager.instance.UIRefresh();

        return true;
    }

    public void GetGold(int gold)
    {
        myGold = myGold + gold;
        UIManager.instance.UIRefresh();
    }
    #endregion

    public void FixWhatMyPieces(int pieceNum)
    {
        myPieces = pieceNum;
    }

    #region External Call == WhatIs
    public int WhatMyLevel()
    {
        return myLevel;
    }

    public int WhatMyHp()
    {
        return myHp;
    }

    public int WhatMyGold()
    {
        return myGold;
    }

    public int WhatMyEXP()
    {
        return myExp;
    }

    public int WhatMyPieces()
    {
        return myPieces;
    }

    public int WhatMyMAXPieces()
    {
        return maxPieces;
    }

    public int WhatMyMAXEXP()
    {
        return maxExp;
    }
    #endregion


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
