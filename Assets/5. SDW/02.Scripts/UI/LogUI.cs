using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public static LogUI instance { get; private set; }


    [SerializeField] private GameObject logTextObject;
    [SerializeField] private GameObject logBackgroundObject;
    [SerializeField] private float alphaSpeed;
    private Text logText;
    private Color logTextBasicColor;
    private Image logBackground;
    private Color logBackgroundBasicColor;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        logText = logTextObject.GetComponent<Text>();
        logBackground = logBackgroundObject.GetComponent<Image>();

        logTextBasicColor = logText.color;
        logBackgroundBasicColor = logBackground.color;

        logText.color = new Color(0,0,0,0);
        logBackground.color = new Color(0, 0, 0, 0);
    }
    public void SettingLogText(string _LogText)
    {
        logText.text = _LogText;
        logText.color = logTextBasicColor;
        logBackground.color = logBackgroundBasicColor;
    }

    private void Update()
    {
        Color logColor = logText.color;
        Color backColor = logBackground.color;

        if (logColor.a > 0)
        {
            logColor.a = logColor.a - (alphaSpeed * Time.deltaTime);
            logText.color = logColor;
        }

        if (backColor.a > 0)
        {
            backColor.a = backColor.a - (alphaSpeed * Time.deltaTime);
            logBackground.color = backColor;
        }
    }


}
