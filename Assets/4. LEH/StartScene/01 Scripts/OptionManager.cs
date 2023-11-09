using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum resolutions
{
    HD, // 1280*720
    FHD, // 1920*1080
    QHD, // 2560*1440
    UHD // 3840*2160
}
public class OptionManager : MonoBehaviour
{
    [SerializeField] Dropdown resolutionDD;
    [SerializeField] Toggle fullScreen;
    [SerializeField] Slider volumeSlider;

    resolutions defaultResolution = resolutions.FHD;
    bool defaultScreenSet = true; 
    int defaultMasterVolume = 100;
    
}
