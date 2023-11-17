using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGageController : MonoBehaviour
{
    public Slider gageBar;

    public void ShowGage()
    {
        gageBar.gameObject.SetActive(true);
    }

    public void HideGage()
    {
        gageBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        gageBar.value = GameManager.instance.timeDisplay;
    }
}
