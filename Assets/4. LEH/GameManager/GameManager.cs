using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Phase { Map, Deployment, Combat, Recruitment }
public class GameManager : MonoBehaviour
{
    private float time = 0;
    private int timeDisplay = 0;
    public int _timeDisplay { get; private set; }

    [SerializeField] private float deployTime = 99;
    [SerializeField] private float combatTime = 99;
    [SerializeField] private float recruitTime = 99;

    private Phase currentPhase;

    private void Start()
    {
        StartPhase();
    }


    private void Update()
    {
        if (currentPhase != Phase.Map)
            time = Time.deltaTime;
        ManagePhase(currentPhase);

        Debug.Log($"{ time} : {currentPhase} : {timeDisplay}");
    }
    private void StartPhase() => currentPhase = Phase.Deployment;

    private void ManagePhase(Phase state)
    {
        float setTime = 0f;
        Phase setPhase = Phase.Map;

        if (state == Phase.Map)
            return;

        switch (state)
        {
            case Phase.Deployment:
                setTime = deployTime;
                setPhase = Phase.Combat;
                break;
            case Phase.Combat:
                setTime = combatTime;
                setPhase = Phase.Recruitment;
                break;
            case Phase.Recruitment:
                setTime = recruitTime;
                setPhase = Phase.Map;
                break;
        }

        timeDisplay = (int)(setTime - time);

        if (time > setTime)
        {
            currentPhase = setPhase;
            time = 0;
        }
    }
}
