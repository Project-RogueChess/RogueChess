using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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

        var clampIdx = math.max(synergySO.currentTermIdx, 0);

       
        if (synergySO.injectData.attackDamage == 0)
        {
            var synergyString = $"MaxHP :{synergySO.injectData.maxHp * clampIdx}";
            SynergyToolTip.SetupSynergyToolTip(synergySO.icon,whatisme, termsNum, synergyString);
        }
        else if(synergySO.injectData.attackDamage != 0 && synergySO.injectData.maxHp != 0)
        {
            var synergyString = $"MaxHP :{synergySO.injectData.maxHp * clampIdx}";
            synergyString += $"\nAtkDmg :{synergySO.injectData.attackDamage * clampIdx}";
            SynergyToolTip.SetupSynergyToolTip(synergySO.icon, whatisme, termsNum, synergyString);
        }
        else
        {
            var synergyString = $"AtkDmg :{synergySO.injectData.attackDamage * clampIdx}";
            SynergyToolTip.SetupSynergyToolTip(synergySO.icon, whatisme, termsNum, synergyString);
        }
        termsNum = "";


    }


    public void OnPointerExit(PointerEventData eventData)
    {
        SynergyToolTip.gameObject.transform.position = new Vector3(2300, 810, 0);
    }

}

