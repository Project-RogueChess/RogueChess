using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagedImage : MonoBehaviour
{
    public float recoverSpeed = 2f;
    public Image damagedImage;
    private float _hurtGage = 0.0f;

    // Update is called once per frame
    void Update()
    {
        _hurtGage = Mathf.Max(_hurtGage - Time.deltaTime * recoverSpeed, 0.0f);
        damagedImage.color = new Color(1f, 1f, 1f, _hurtGage / 4);
    }

    public void SetHurtGage(int hurtAmount)
    {
        if (hurtAmount < 0)
            _hurtGage = Mathf.Clamp(math.abs(hurtAmount), 0, 4);
    }
}
