using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    [SerializeField]
    private string gameSceneName = "Prototype";

    public void ResetGameScene()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Scene resetted");
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Quit called");
        Application.Quit();
    }
}
