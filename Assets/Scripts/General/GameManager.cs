using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : Singleton<GameManager>
{
    public void ResetGameScene()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Scene resetted");
        SceneManager.LoadScene("ProgrammersTest");
    }

    public void ExitGame()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Quit called");
        Application.Quit();
    }
}
