using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EndScreenButtonManage : MonoBehaviour
{
    public void SceneManageButton(string sceneName)
    {
        StartCoroutine(RestartScene(sceneName));
    }

    IEnumerator RestartScene(string sceneName)
    {
        if (FadeSceneManager.instance != null)
        {
            FadeSceneManager.instance.FadeInOut(1f);
            yield return new WaitForSeconds(1.5f);
        }
        GameManager.instance = null;
        HexaUnitManager.instance = null;
        CreepSpawnManager.instance = null;
        TilemapManager.instance = null;
        SceneManager.LoadScene(sceneName);
    }
}
