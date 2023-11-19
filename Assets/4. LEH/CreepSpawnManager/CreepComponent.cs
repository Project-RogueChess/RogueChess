using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepComponent : Article
{
    public Transform rootTransform;
    public GameObject modelPrefab;
    public GameObject projectile;
    public string creepName;
    public int id;

    private void OnDisable()
    {
        buffData = new ArticleData[4] { ArticleData.ZeroArticleData, ArticleData.ZeroArticleData, ArticleData.ZeroArticleData, ArticleData.ZeroArticleData };
    }
}
