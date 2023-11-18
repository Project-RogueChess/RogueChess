using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHPBar : MonoBehaviour
{
    public Article article;
    public Slider hpBar;

    private void Start()
    {
        article = transform.parent.GetComponent<Article>();
        hpBar = transform.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        hpBar.value = (float)article.hp / article.maxHp;
    }
}
