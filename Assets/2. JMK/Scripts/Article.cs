using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ArticleData
{
    public int maxHp;
    public int maxMp;
    public int attackDamage;
    public float attackSpeed;
    public int attackRange;
    public float moveSpeed;

    public static ArticleData ZeroArticleData => new ArticleData();

    public ArticleData(int maxHp, int maxMp, int attackDamage, float attackSpeed, int attackRange, float moveSpeed)
    {
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.moveSpeed = moveSpeed;
    }

    public static ArticleData operator +(ArticleData a, ArticleData b)
    {
        return new ArticleData(a.maxHp + b.maxHp, a.maxMp + b.maxMp, 
            a.attackDamage + b.attackDamage, a.attackSpeed + b.attackSpeed, a.attackRange + b.attackRange, a.moveSpeed + b.moveSpeed);
    }
    public static ArticleData operator -(ArticleData a, ArticleData b)
    {
        return new ArticleData(a.maxHp - b.maxHp, a.maxMp - b.maxMp,
            a.attackDamage - b.attackDamage, a.attackSpeed - b.attackSpeed, a.attackRange - b.attackRange, a.moveSpeed - b.moveSpeed);
    }
}

public class Article : MonoBehaviour
{
    //0 -> 아이템 , 1 -> 병합, 2 -> 종족 시너지, 3 -> 직업 시너지 
    public ArticleData[] buffData = new ArticleData[4] { ArticleData.ZeroArticleData, ArticleData.ZeroArticleData, ArticleData.ZeroArticleData, ArticleData.ZeroArticleData };

    public int originMaxHp;
    public int originMaxMp;
    public int originAttackDamage;
    public float originAttackSpeed;
    public int originAttackRange;
    public float originMoveSpeed;

    public int hp;
    public int mp;

    public int maxHp => originMaxHp + buffData[0].maxHp + buffData[1].maxHp + buffData[2].maxHp + buffData[3].maxHp;
    public int maxMp => originMaxMp + buffData[0].maxMp + buffData[1].maxMp + buffData[2].maxMp + buffData[3].maxMp;

    public int attackDamage => originAttackDamage + buffData[0].attackDamage + buffData[1].attackDamage + buffData[2].attackDamage + buffData[3].attackDamage;
    public float attackSpeed => originAttackSpeed + buffData[0].attackSpeed + buffData[1].attackSpeed + buffData[2].attackSpeed + buffData[3].attackSpeed;

    public int attackRange => originAttackRange + buffData[0].attackRange + buffData[1].attackRange + buffData[2].attackRange + buffData[3].attackRange;
    public float moveSpeed => originMoveSpeed + buffData[0].moveSpeed + buffData[1].moveSpeed + buffData[2].moveSpeed + buffData[3].moveSpeed;

    public Animator animator;
}
