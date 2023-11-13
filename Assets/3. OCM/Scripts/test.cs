using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Transform hpBar;
    GameObject hp;
    Camera camera = null;
    private void Awake()
    {
        hpBar = transform.GetChild(0);
        camera = Camera.main;
    }
    void Update()
    {
        hpBar.transform.position = camera.WorldToScreenPoint(transform.parent.position + new Vector3(0, 1.15f, 0.5f));
    }
}
