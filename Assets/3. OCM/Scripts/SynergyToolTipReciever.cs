using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SynergyToolTipReciever : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InvSpawnManager InvSpawnManager;


    public SynergyToolTipPanel SynergyToolTip;

    public string whatisme;


    public SynergyArraySO synergyArraySO;
    public int[] terms;
    public SynergySO synergySO;
    public string termsNum;
    void Awake()
    {
        InvSpawnManager = FindObjectOfType<InvSpawnManager>().GetComponent<InvSpawnManager>();

        SynergyToolTip = FindObjectOfType<SynergyToolTipPanel>();
        whatisme = transform.GetChild(2).name;


        
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SynergyToolTip.gameObject.transform.position = new Vector3(1710, 810, 0);


        for (int i = 0; i < InvSpawnManager.synergysArray.Length; i++)
        {
            if (whatisme == InvSpawnManager.synergysArray[i])
            {

                synergySO = (SynergySO)Resources.Load("SynergyScriptableObj/" + whatisme);
                terms = synergySO.terms;
            }
        }

        for (int i = 0;i<terms.Length;i++)
        {
            if (i==0)
            {
                termsNum += terms[i].ToString();
            }
            else
            {
                termsNum += "/" + terms[i].ToString();
            }
        }
        if (synergySO.injectData.maxHp !=0)
        {
            SynergyToolTip.SetupSynergyToolTip(synergySO.icon,whatisme, termsNum,"maxHp",synergySO.injectData.maxHp);
        }
        else
        {
            SynergyToolTip.SetupSynergyToolTip(synergySO.icon, whatisme, termsNum, "atkDmg", synergySO.injectData.attackDamage);
        }
        termsNum = "";


    }


    public void OnPointerExit(PointerEventData eventData)
    {
        SynergyToolTip.gameObject.transform.position = new Vector3(2300, 810, 0);
    }

}

