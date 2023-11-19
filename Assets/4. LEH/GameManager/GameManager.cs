using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum Phase { None, SelectMapNode, Deployment, Combat, Result, Recruitment }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Map_Node.currentNodeTypeEnum currentNode;
    public ControlBlackHole blackHole;
    public AnimationCurve inhalationMotion;
    public AnimationCurve returnMotion;

    public bool isNodeComplete = true;

    private float _time = 0;
    public float timeDisplay => _time / CurrentTime(currentPhase);


    [SerializeField] private bool _playBattleEvent = false;
    [SerializeField] private float _deployTime = 99;
    [SerializeField] private float _combatTime = 99;
    [SerializeField] private float _resultTIme = 99;
    [SerializeField] private float _recruitTime = 99;

    public UnityEvent OnSelectMapNode;
    public UnityEvent OnDeployment;
    public UnityEvent OnCombat;
    public UnityEvent OnResult;
    public UnityEvent OnRecruitment;

    public static Phase[] phaseChain = { Phase.SelectMapNode, Phase.Deployment, Phase.Combat, Phase.Result, Phase.Recruitment };

    public WaitForSeconds waitThreeSec = new WaitForSeconds(3);

    public bool forcePause = false;

    public Phase currentPhase = Phase.None;

    public GameObject gameOverUI;
    public GameObject gameClearUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
                Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(StartScene());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangePhaseAndInvoke(Phase.SelectMapNode);
        }
        ManagePhase();

        if (currentPhase == Phase.Combat)
            CheckSkipBattle();
    }

    /*private void OnGUI()
    {
        GUIStyle GUIStyle = new GUIStyle(GUI.skin.label);
        GUIStyle.fontSize = 50;
        GUIStyle.normal.textColor = Color.cyan;

        GUI.Label(new Rect(400, 100, Screen.width * 0.5f, Screen.height * 0.25f), timeDisplay.ToString());
    }*/


    public void RunPhase()
    {
        Resume();
        ForceChangePhaseAndInvoke(Phase.Deployment);
    }

    public void SKipToNextPhase()
    {
        Debug.Log(phaseChain.Length);
        var nextPhase = phaseChain[(int)currentPhase % phaseChain.Length];
        ForceChangePhaseAndInvoke(nextPhase);
    }

    public void Pause() => forcePause = true;

    public void Resume() => forcePause = false;

    public void CheckSkipBattle()
    {
        int winFlag = -1;
        //아군 이김
        if (HexaUnitManager.instance.teamCount[1] == 0)
        {
            winFlag = 0;
        }
        //적군 이김
        else if (HexaUnitManager.instance.teamCount[0] == 0)
        {
            winFlag = 1;
        }

        if (winFlag != -1)
        {
            CheckBattleResult(winFlag);
        }
    }

    public void ForceEndBattle()
    {
        if (_playBattleEvent)
            return;
        var copyUnitList = HexaUnitManager.instance.unitList.ToList();

        foreach (var u in copyUnitList)
        {
            if (u.team == 0)
            {
                u.Die();
            }
        }
        CheckBattleResult(1);
    }

    public void CheckBattleResult(int winFlag)
    {
        Pause();
        HexaUnitManager.instance.excuteUnitControll = false;
        //코루틴(승리 모션, 인자에 승리팀 넘기기)
        StartCoroutine(Winning(winFlag));
    }

    private void ManagePhase()
    {
        if (forcePause || currentPhase == Phase.None || currentPhase == Phase.SelectMapNode)
            return;

        _time -= Time.deltaTime;

        if (_time < 0)
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
                return _deployTime;
            case Phase.Combat:
                return _combatTime;
            case Phase.Result:
                return _resultTIme;
            case Phase.Recruitment:
                return _recruitTime;
        }
    }

    public void ForceChangePhaseAndInvoke(Phase changePhase)
    {
        currentPhase = changePhase;

        switch (changePhase)
        {
            case Phase.SelectMapNode:
                _time = 0;
                isNodeComplete = true;
                OnSelectMapNode.Invoke();
                break;
            case Phase.Deployment:
                _time = _deployTime;
                OnDeployment.Invoke();
                break;
            case Phase.Combat:
                _time = _combatTime;
                OnCombat.Invoke();
                break;
            case Phase.Result:
                _time = _resultTIme;
                OnResult.Invoke();
                break;
            case Phase.Recruitment:
                _time = _recruitTime;
                OnRecruitment.Invoke();
                break;
        }
    }

    public void ChangePhaseAndInvoke(Phase changePhase)
    {
        if (currentPhase == changePhase)
            return;

        currentPhase = changePhase;

        switch (changePhase)
        {
            case Phase.SelectMapNode:
                isNodeComplete = true;
                _time = 0;
                OnSelectMapNode.Invoke();
                break;
            case Phase.Deployment:
                _time = _deployTime;
                OnDeployment.Invoke();
                break;
            case Phase.Combat:
                _time = _combatTime;
                OnCombat.Invoke();
                break;
            case Phase.Result:
                _time = _resultTIme;
                OnResult.Invoke();
                break;
            case Phase.Recruitment:
                _time = _recruitTime;
                OnRecruitment.Invoke();
                break;
        }
    }

    IEnumerator Winning(int team)
    {
        DataManager.instance.GetGold(team == 0 ? 3 : 1);

        
        if (team == 1)
        {
            if (currentNode == Map_Node.currentNodeTypeEnum.End)
            {
                DataManager.instance.PlayerHP -= DataManager.instance.PlayerHP;
            }
            else
            {
                DataManager.instance.PlayerHP -= HexaUnitManager.instance.teamCount[1];
            }
        }
            

        _playBattleEvent = true;
        var timer = 0f;
        foreach (var unit in HexaUnitManager.instance.unitList)
        {
            if (unit.article.animator != null)
            {
                unit.ForceStop();
                unit.article.animator.Play("Victory");
            }
        }
        ChangePhaseAndInvoke(Phase.Result);
        yield return waitThreeSec;
        Resume();

        if (team == 1)
        {
            var unitPositions = new List<Vector3>();

            foreach (var unit in HexaUnitManager.instance.unitList)
            {
                unitPositions.Add(unit.transform.position);
                if (unit.article.animator != null)
                {
                    unit.article.animator.Play("Idle", -1, 0f);
                    unit.article.animator.Update(0f);
                }
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

            foreach (var unit in HexaUnitManager.instance.unitList)
            {
                CreepSpawnManager.instance.ReturnCreep(unit.GetComponent<CreepComponent>());
            }

            yield return waitThreeSec;
        }

        //엔딩 (게임 클 / 게임 오버)
        if(DataManager.instance.PlayerHP <= 0)
            gameOverUI.SetActive(true);
        if (currentNode == Map_Node.currentNodeTypeEnum.End && team == 0)
            gameClearUI.SetActive(true);

        var destinationUnitPos = new Dictionary<GameObject, Vector3>();
        foreach (var tile in InvSpawnManager.instance.hexaTiles)
        {
            if (tile.piece != null)
            {
                tile.piece.SetActive(true);
                destinationUnitPos.Add(tile.piece, tile.transform.position);
                var unit = tile.piece.GetComponent<HexaUnit>();
                if (unit.article.animator != null)
                {
                    unit.article.animator.Play("Idle", -1, 0f);
                    unit.article.animator.Update(0f);
                }
            }
        }

        var currentUnitPos = new Dictionary<GameObject, Vector3>();
        var currentUnitRot = new Dictionary<GameObject, Quaternion>();

        foreach (var u in destinationUnitPos.Keys)
        {
            currentUnitPos.Add(u, u.transform.position + (team == 1 ? Vector3.down * 2f : Vector3.zero));
            currentUnitRot.Add(u, u.transform.rotation);
        }

        timer = 0f;

        while (timer < returnMotion.keys[inhalationMotion.keys.Length - 1].time)
        {
            foreach (var unit in destinationUnitPos.Keys)
            {
                unit.transform.position = Vector3.Lerp(currentUnitPos[unit], destinationUnitPos[unit], returnMotion.Evaluate(timer));
                unit.transform.rotation = Quaternion.Slerp(currentUnitRot[unit], Quaternion.LookRotation(Vector3.forward), returnMotion.Evaluate(timer));
            }
            timer += Time.deltaTime;
            yield return null;
        }

        HexaUnitManager.instance.UnRegisterAll();
        foreach (var tile in InvSpawnManager.instance.hexaTiles)
        {
            if (tile.piece != null)
            {
                var unit = tile.piece.GetComponent<HexaUnit>();
                unit.ResetSavedValue();
                unit.SetTileIndex(new Vector2Int(tile.triggerInfo.x, tile.triggerInfo.y));

                HexaUnitManager.instance.RegisterHexaUnit(unit);

                var piece = tile.piece.GetComponent<Pieces>();

                piece.hp = piece.maxHp;
                piece.mp = piece.maxMp;
            }
        }

        _time = _time > 0.05f ? 0.05f : _time;
        _playBattleEvent = false;
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(1f);
        ForceChangePhaseAndInvoke(Phase.SelectMapNode);
    }
}