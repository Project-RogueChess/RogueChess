using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RdmEvt_Main : MonoBehaviour
{
    [Header("Declaration")]
    [SerializeField] private int eventCount = 3;
    [SerializeField] private GameObject MAIN_TITLE;
    [SerializeField] private GameObject MAIN_IMAGE;
    [SerializeField] private GameObject MAIN_TEXT;
    [SerializeField] private GameObject[] MAIN_CHOICE;
    private Image main_title;
    private Image main_image;
    private Image main_text;
    private Image[] main_choice;

    [Header("ScriptManager")]
    [SerializeField] private GameObject dataManagerObject;
    [SerializeField] private GameObject questButton;

    [Header("Event_00")]
    [SerializeField] private Sprite evt01Title;
    [SerializeField] private Sprite evt01Image;
    [SerializeField] private Sprite evt01Text;
    [SerializeField] private List<Sprite> evt01Choices;

    [Header("Event_01")]
    [SerializeField] private Sprite evt02Title;
    [SerializeField] private Sprite evt02Image;
    [SerializeField] private Sprite evt02Text;
    [SerializeField] private List<Sprite> evt02Choices;

    [Header("Event_02")]
    [SerializeField] private Sprite evt03Title;
    [SerializeField] private Sprite evt03Image;
    [SerializeField] private Sprite evt03Text;
    [SerializeField] private List<Sprite> evt03Choices;

    private int eventStage;
    private DataManager dataManagerScript;

    private void Start()
    {
        dataManagerScript = dataManagerObject.GetComponent<DataManager>();

        main_title = MAIN_TITLE.GetComponent<Image>();
        main_image = MAIN_IMAGE.GetComponent<Image>();
        main_text = MAIN_TEXT.GetComponent<Image>();
        main_choice = new Image[MAIN_CHOICE.Length];


        for (int i = 0; i < MAIN_CHOICE.Length; i++)
        {
            main_choice[i] = MAIN_CHOICE[i].GetComponent<Image>();
        }
        
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, 0);
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            RandomEventStart(); //test
        }
    }


    public void RandomEventStart()
    {
        gameObject.SetActive(true);

        int randomvalue = Random.Range(0, eventCount);
        eventStage = randomvalue;

        EventMachine(randomvalue);
    }

    public void TreasureEventStart()
    {
        gameObject.SetActive(true);
        eventStage = 0;
        EventMachine(0);
    }

    private void EventMachine(int randomValue)
    {
        switch(randomValue)
        {
            case 0:
                SettingEvent(evt01Title, evt01Image, evt01Text, evt01Choices);
                break;
            case 1:
                SettingEvent(evt02Title, evt02Image, evt02Text, evt02Choices);
                break;
            case 2:
                SettingEvent(evt03Title, evt03Image, evt03Text, evt03Choices);
                break;
        }
    }
    private void SettingEvent(Sprite _title, Sprite _image, Sprite _text, List<Sprite> _choiceList)
    {
        int listLength = _choiceList.Count;

        main_title.sprite = _title;
        main_image.sprite = _image;
        main_text.sprite = _text;

        for (int j = 0; j < main_choice.Length; j++)
        {
            MAIN_CHOICE[j].SetActive(false);
        }

        for (int i = 0; i < listLength; i++)
        {
            MAIN_CHOICE[i].SetActive(true);
            main_choice[i].sprite = _choiceList[i];
        }
    }

    private void ExitChoiceMethod()
    {
        // 매니저한테 bool값(이 노드가 끝났다는) 알려주기 + 지도 노드 클릭할때 해당 bool값 검사하기
        gameObject.SetActive(false);
    }
    #region Button Click
    public void ChoiceButtonClick1()
    {
        float randomEventValue = Random.Range(0, 100);

        switch (eventStage)
        {
            case 0:
                if (randomEventValue < 40)
                {
                    // get item
                    UIManager.instance.AddRandomItem();
                    Debug.Log("AddItem");
                    LogUI.instance.SettingLogText("아이템을 획득했습니다");
                }

                else if (randomEventValue < 80)
                {
                    // get piece
                    float randomValuepieces = Random.Range(0, 100);

                    if (randomValuepieces < 5)
                    {
                        InvSpawnManager.instance.AddRandomPiece(5);
                        Debug.Log("AddPieces5");
                        LogUI.instance.SettingLogText("5코스트 기물을 획득했습니다");
                    }

                    else if (randomValuepieces < 15)
                    {
                        InvSpawnManager.instance.AddRandomPiece(4);
                        Debug.Log("AddPieces4");
                        LogUI.instance.SettingLogText("4코스트 기물을 획득했습니다");
                    }

                    else if (randomValuepieces < 35)
                    {
                        InvSpawnManager.instance.AddRandomPiece(3);
                        Debug.Log("AddPieces3");
                        LogUI.instance.SettingLogText("3코스트 기물을 획득했습니다");
                    }

                    else if (randomValuepieces < 60)
                    {
                        InvSpawnManager.instance.AddRandomPiece(2);
                        Debug.Log("AddPieces2");
                        LogUI.instance.SettingLogText("2코스트 기물을 획득했습니다");
                    }

                    else if (randomValuepieces <= 100)
                    {
                        InvSpawnManager.instance.AddRandomPiece(1);
                        Debug.Log("AddPieces1");
                        LogUI.instance.SettingLogText("1코스트 기물을 획득했습니다");
                    }
                    
                }

                else if (randomEventValue <= 100)
                {
                    // lost hp
                    dataManagerScript.LostHp(1);
                    Debug.Log("LostHp1");
                    LogUI.instance.SettingLogText("체력을 1만큼 잃었습니다");
                }
                break;

            case 1:
                if (randomEventValue < 50)
                {
                    // get hp
                    dataManagerScript.GetHp(1);
                    Debug.Log("GetHp1");
                    LogUI.instance.SettingLogText("체력을 1만큼 회복했습니다");
                }

                else if (randomEventValue <= 100)
                {
                    // lost hp
                    dataManagerScript.LostHp(1);
                    Debug.Log("LostHp1");
                    LogUI.instance.SettingLogText("체력을 1만큼 잃었습니다");
                }
                break;

            case 2:
                //lost piece
                InvSpawnManager.instance.DeleteRandomPiece();
                Debug.Log("LostPieces");
                LogUI.instance.SettingLogText("해당 부하가 떠났습니다");
                break;

            default:
                Debug.Log(eventStage + "Button1");
                break;
        }
        ExitChoiceMethod();
    }
    public void ChoiceButtonClick2()
    {
        switch (eventStage)
        {
            case 0:
                // nothing
                LogUI.instance.SettingLogText("그냥 지나갔습니다");
                break;

            case 1:
                //lost hp
                dataManagerScript.LostHp(1);
                Debug.Log("LostHp1");
                LogUI.instance.SettingLogText("배가 고파 체력을 1만큼 잃었습니다");
                break;

            case 2:
                //lost gold
                dataManagerScript.LostGold(4);
                Debug.Log("LostGold4");
                LogUI.instance.SettingLogText("부하에게 4골드를 지불했습니다");
                break;

            default:
                Debug.Log(eventStage + "Button2");
                break;
        }
        ExitChoiceMethod();
    }
    public void ChoiceButtonClick3()
    {
        float randomValue = Random.Range(0, 100);

        switch (eventStage)
        {
            case 2:
                if (randomValue < 5)
                {
                    //lost item + gold
                    UIManager.instance.DeleteRandomItem();
                    dataManagerScript.LostGold(5);
                    Debug.Log("DelItem + LostGold5");
                    LogUI.instance.SettingLogText("부하가 아이템과 골드를 훔쳐달아났습니다");
                }
                else if (randomValue < 15)
                {
                    //lost item
                    UIManager.instance.DeleteRandomItem();
                    Debug.Log("DelItem");
                    LogUI.instance.SettingLogText("부하가 아이템을 훔쳐달아났습니다");
                }
                else if (randomValue < 45)
                {
                    //lost gold
                    dataManagerScript.LostGold(5);
                    Debug.Log("LostGold5");
                    LogUI.instance.SettingLogText("부하가 골드를 훔쳐달아났습니다");
                }
                else if (randomValue < 80)
                {
                    //lost hp
                    dataManagerScript.LostHp(1);
                    Debug.Log("LostHp1");
                    LogUI.instance.SettingLogText("부하를 제압하다가 체력을 1만큼 잃었습니다");
                }
                else if (randomValue <= 100)
                {
                    //nothing
                    LogUI.instance.SettingLogText("부하를 제압했습니다");
                }
                break;

            default:
                Debug.Log(eventStage + "Button3");
                break;
        }
        ExitChoiceMethod();
    }

    public void ExitButtonClick()
    {
        questButton.SetActive(true);
        questButton.GetComponent<QuestUI>().SetObject(gameObject);
        gameObject.SetActive (false);
        //창이 닫힌 뒤 아이콘 클릭하면 다시 열려야 함
    }
    #endregion
}
