using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RdmEvt_Main : MonoBehaviour
{
    [Header("Declaration")]
    [SerializeField] private GameObject MAIN_TITLE;
    [SerializeField] private GameObject MAIN_IMAGE;
    [SerializeField] private GameObject MAIN_TEXT;
    [SerializeField] private GameObject[] MAIN_CHOICE;
    private Sprite main_title;
    private Sprite main_image;
    private Sprite main_text;
    private Sprite[] main_choice;

    [Header("Event_01")]
    [SerializeField] private Sprite evt01Title;
    [SerializeField] private Sprite evt01Image;
    [SerializeField] private Sprite evt01Text;
    [SerializeField] private List<Sprite> evt01Choices;

    private void Start()
    {
        main_title = MAIN_TITLE.GetComponent<Sprite>();
        main_image = MAIN_IMAGE.GetComponent<Sprite>();
        main_text = MAIN_TEXT.GetComponent<Sprite>();

        for (int i = 0; i < MAIN_CHOICE.Length; i++)
        {
            main_choice[i] = MAIN_CHOICE[i].GetComponent<Sprite>();
        }
    }
    private void SettingEvent(Sprite _title, Sprite _image, Sprite _text, List<Sprite> _choiceList)
    {
        int listLength = _choiceList.Count;

        main_title = _title;
        main_image = _image;
        main_text = _text;

        for (int i = 0; i < listLength; i++)
        {
            MAIN_CHOICE[i].SetActive(true);
            main_choice[i] = _choiceList[i];
        }
    }
}
