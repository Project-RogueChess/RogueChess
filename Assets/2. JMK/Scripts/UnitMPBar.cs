using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMPBar : MonoBehaviour
{
    public Article article;
    public Slider mpBar;

    private void Update()
    {
        mpBar.value = (float)article.mp / article.maxMp;
    }
}
