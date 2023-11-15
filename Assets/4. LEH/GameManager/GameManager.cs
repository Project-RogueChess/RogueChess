using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum Phase { SelectMapNode, Deployment, Combat, Recruitment }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float _time = 0;
    public int timeDisplay => Mathf.RoundToInt(_time);

    [SerializeField] private float deployTime = 99;
    [SerializeField] private float combatTime = 99;
    [SerializeField] private float recruitTime = 99;

    public UnityEvent OnSelectMapNode;
    public UnityEvent OnDeployment;
    public UnityEvent OnCombat;
    public UnityEvent OnRecruitment;

    private Phase currentPhase = Phase.SelectMapNode;

    private void Start()
    {
        RunPhase();
    }


    private void Update()
    {
        ManagePhase();
    }

    public void RunPhase() => currentPhase = Phase.Deployment;
    public void StopPhase() => currentPhase = Phase.SelectMapNode;

    private void ManagePhase()
    {
        
    }

    private void ChangePhase(Phase changePhase)
    {
        if (currentPhase == changePhase)
            return;

        currentPhase = changePhase;

        switch (changePhase)
        {
            case Phase.SelectMapNode:
                OnSelectMapNode.Invoke();
                break;
            case Phase.Deployment:
                OnDeployment.Invoke();
                break;
            case Phase.Combat:
                OnCombat.Invoke();
                break;
            case Phase.Recruitment:
                OnRecruitment.Invoke();
                break; 
        }
    }
}
