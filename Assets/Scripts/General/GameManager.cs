using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    [Header("Settings")]
    [SerializeField]
    private string gameSceneName = "Prototype";

    public void ResetGameScene()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Scene resetted");
        NavigationManager.Instance.ChangeScene(gameSceneName);
    }

    public void QuitGame()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Quit called");
        NavigationManager.Instance.QuitGame();
    }
}
