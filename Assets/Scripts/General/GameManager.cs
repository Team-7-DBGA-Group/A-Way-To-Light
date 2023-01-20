using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameManager : Singleton<GameManager>
{
    
    [Header("Settings")]
    [SerializeField]
    private string gameSceneName = "Prototype";

    private Player _player = null;
    public static event Action<bool> OnPause;
    private bool inPause = false;
    public void Update()
    {
        if(InputManager.Instance.GetPausePressed())
        {
            
            if(inPause)
            {
                UnpauseGame();
            }
            
            else
            {
                PauseGame();
                
            }
        }
    }

    public void ResetGameScene()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Scene resetted");

        StartCoroutine(ResetSequence());
        //NavigationManager.Instance.ChangeScene(gameSceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        OnPause?.Invoke(true);
        inPause = true;
    }

    public void UnpauseGame()
    {
        OnPause?.Invoke(false);
        inPause = false;
    }
    public void QuitGame()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Quit called");
        NavigationManager.Instance.QuitGame();
    }

    private void OnEnable()
    {
        SpawnManager.OnPlayerSpawn += ResetPlayer;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= ResetPlayer;
    }

    private void ResetPlayer(GameObject playerObj)
    {
        _player = playerObj.GetComponent<Player>();
        _player.gameObject.GetComponent<PlayerLightShooting>().ResetLightCharges();
    }

    private IEnumerator ResetSequence()
    {
        UISceneTransitionController.Instance.OpenTransition();
        yield return new WaitForSeconds(2.0f);
        UISceneTransitionController.Instance.CloseTransition();
        PickablesManager.Instance.ResetPickables();
        SpawnManager.Instance.SpawnPlayer();
    }
}
