using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum Phase { Null, SelectMapNode, Deployment, Combat, Result, Recruitment }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ControlBlackHole blackHole;
    public AnimationCurve inhalationMotion;

    private float _time = 0;
    public float timeDisplay => _time / CurrentTime(currentPhase);

    [SerializeField] private float deployTime = 99;
    [SerializeField] private float combatTime = 99;
    [SerializeField] private float resultTIme = 99;
    [SerializeField] private float recruitTime = 99;

    public UnityEvent OnSelectMapNode;
    public UnityEvent OnDeployment;
    public UnityEvent OnCombat;
    public UnityEvent OnResult;
    public UnityEvent OnRecruitment;
    public UnityEvent OnGameOver;

    public WaitForSeconds waitThreeSec = new WaitForSeconds(3);

    public bool forcePause = false;

    public Phase currentPhase = Phase.Null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != null)
                Destroy(this);
        }
    }

    private void Start()
    {
        //RunPhase();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangePhaseAndInvoke(Phase.SelectMapNode);
        }
        ManagePhase();
        CheckBattleSkip();
    }

    private void OnGUI()
    {
        GUIStyle GUIStyle = new GUIStyle(GUI.skin.label);
        GUIStyle.fontSize = 50;
        GUIStyle.normal.textColor = Color.cyan;

        GUI.Label(new Rect(400, 100, Screen.width * 0.5f, Screen.height * 0.25f), timeDisplay.ToString());
    }


    public void RunPhase()
    {
        Resume();
        ChangePhaseAndInvoke(Phase.Deployment);
    }

    public void Pause() => forcePause = true;

    public void Resume() => forcePause = false;

    public void CheckBattleSkip()
    {
        if(currentPhase == Phase.Combat)
        {
            int winFlag = -1;
            //아군 이김
            if (HexaUnitManager.instance.teamCount[1] == 0)
            {
                winFlag = 0;
            }
            //적군 이김
            else if(HexaUnitManager.instance.teamCount[0] == 0)
            {
                winFlag = 1;
            }

            if(winFlag != -1)
            {
                Pause();

                switch (winFlag)
                {
                    case 0:
                        //아군 승리 이벤트
                        break;
                    case 1:
                        //적군 승리 이벤트
                        break;
                }

                HexaUnitManager.instance.excuteUnitControll = false;
                //코루틴(승리 모션, 인자에 승리팀 넘기기)
                StartCoroutine(Winning(winFlag));
            }
        }
    }

    public void GameOver()
    {
        forcePause = true;
        OnGameOver.Invoke();
        //페이드 인,아웃
    }

    private void ManagePhase()
    {
        if (forcePause || currentPhase == Phase.Null || currentPhase == Phase.SelectMapNode)
            return;

        _time -= Time.deltaTime;

        if(_time < 0)
        {
            switch (currentPhase)
            {
                case Phase.Deployment:
                    ChangePhaseAndInvoke(Phase.Combat);
                    break;
                case Phase.Combat:
                    ChangePhaseAndInvoke(Phase.Result);
                    
                    break;
                case Phase.Result:
                    ChangePhaseAndInvoke(Phase.Recruitment);
                    break;
                case Phase.Recruitment:
                    ChangePhaseAndInvoke(Phase.SelectMapNode);
                    break;
            }
        }
    }

    private float CurrentTime(Phase phase)
    {
        switch (phase)
        {
            default:
                return 1f;
            case Phase.Deployment:
                return deployTime;
            case Phase.Combat:
                return combatTime;
            case Phase.Result:
                return resultTIme;
            case Phase.Recruitment:
                return recruitTime;
        }
    }

    private void ChangePhaseAndInvoke(Phase changePhase)
    {
        if (currentPhase == changePhase)
            return;

        currentPhase = changePhase;

        switch (changePhase)
        {
            case Phase.SelectMapNode:
                _time = 0;
                OnSelectMapNode.Invoke();
                break;
            case Phase.Deployment:
                _time = deployTime;
                OnDeployment.Invoke();
                break;
            case Phase.Combat:
                _time = combatTime;
                OnCombat.Invoke();
                break;
            case Phase.Result:
                _time = resultTIme;
                OnResult.Invoke();
                break;
            case Phase.Recruitment:
                _time = recruitTime;
                OnRecruitment.Invoke();
                break;
        }
    }

    IEnumerator Winning(int team)
    {
        var timer = 0f;
        foreach (var unit in HexaUnitManager.instance.unitList)
        {
            unit.ForceStop();
            unit.article.animator.Play("Victory");
        }
        ChangePhaseAndInvoke(Phase.Result);
        yield return waitThreeSec;
        Resume();

        if(team == 0)
        {

        }
        else
        {
            var unitPositions = new List<Vector3>();

            foreach (var unit in HexaUnitManager.instance.unitList)
            {
                unitPositions.Add(unit.transform.position);
                unit.article.animator.Play("Idle", -1, 0f);
                unit.article.animator.Update(0f);
            }

            blackHole.StartMotion();
            while (timer < inhalationMotion.keys[inhalationMotion.keys.Length - 1].time)
            {
                for (int i = 0; i < HexaUnitManager.instance.unitList.Count; i++)
                {
                    HexaUnitManager.instance.unitList[i].transform.position = Vector3.Lerp(unitPositions[i], blackHole.blackHole.position, inhalationMotion.Evaluate(timer));
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }


        foreach (var unit in HexaUnitManager.instance.unitList)
        {
            if (team == 1)
                CreepSpawnManager.instance.ReturnCreep(unit.GetComponent<CreepComponent>());
            else
                unit.gameObject.SetActive(false);
        }
        HexaUnitManager.instance.UnRegisterAll();
    }
}