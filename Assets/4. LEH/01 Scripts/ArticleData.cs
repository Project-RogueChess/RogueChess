using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "article Data", menuName = "Scriptable Object/1.article Data", order = 1)]
public class ArticleData : ScriptableObject
{
    [SerializeField]
    private string articleName; // 이름
    public string _articleName { get { return articleName; } }

    [SerializeField]
    private int articleValue; // 가치
    public int _articleValue { get { return articleValue; } }

    [SerializeField]
    private string articleSpecies; // 종족
    public string _articleSpecies { get { return articleSpecies; } }

    [SerializeField]
    private string articleClasses; // 등급
    public string _articleClasses { get { return articleClasses; } }

    [SerializeField]
    private int articleGrade; // 등급
    public int _articleGrade { get { return articleGrade; } }

    [SerializeField]
    private int hp;
    public int _hp { get { return hp; } }

    [SerializeField]
    private int attackPoint;
    public int _attackPoint { get { return attackPoint; } }

    [SerializeField]
    private float attackSpeed;
    public float _attackSpeed { get { return attackSpeed; } }

    [SerializeField]
    private int attackRange;
    public int _attackRange { get { return attackRange; } }

    [SerializeField]
    private float moveSpeed;
    public float _moveSpeed { get { return moveSpeed; } }
}
