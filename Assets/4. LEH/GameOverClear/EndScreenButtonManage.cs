using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButtonManage : MonoBehaviour
{
    public void SceneManageButton(string sceneName) => SceneManager.LoadScene(sceneName);
}
