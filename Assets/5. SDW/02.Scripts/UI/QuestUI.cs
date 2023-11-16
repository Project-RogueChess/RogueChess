using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    GameObject whatIsOnObject;
    public void SetActiveManager()
    {
        whatIsOnObject.SetActive(true);
        whatIsOnObject = null;
        gameObject.SetActive(false);
    }

    public void SetObject(GameObject UIobject)
    {
        whatIsOnObject = UIobject;
    }
}
