using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Background : MonoBehaviour
{
    [SerializeField] private GameObject mapUI;
    public void ClickedExitButton()
    {
        mapUI.SetActive(false);
        SoundManager.instance.PlaySound("PageFlipOff");
    }
}
