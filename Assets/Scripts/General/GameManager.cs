using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static event Action<bool> OnPause;
    public static event Action OnPauseAction;
    public static event Action OnGameReset;

    [Header("Settings")]
    [SerializeField]
    private float pauseCooldown = 3.0f;

    private Player _player = null;

    private bool _inPause = false;
    private bool _canPause = true;

    public void Update()
    {
        if(InputManager.Instance.GetPausePressed() && _canPause)
        {
            StartCoroutine(COWaitPauseCooldown());
            if(_inPause)
                UnpauseGame();
            else
                PauseGame();
        }
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetString("GameScene", SceneManager.GetActiveScene().name);
    }

    public void ResetGameScene()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Scene resetted");

        StartCoroutine(COResetSequence());
        //NavigationManager.Instance.ChangeScene(gameSceneName);
    }

    public void PauseGame()
    {
        OnPause?.Invoke(true);
        OnPauseAction?.Invoke();
        _inPause = true;
    }

    public void UnpauseGame()
    {
        OnPause?.Invoke(false);
        _inPause = false;
    }

    public void QuitGame()
    {
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Quit called");
        NavigationManager.Instance.QuitGame();
    }

    private void OnEnable()
    {
        SpawnManager.OnPlayerSpawn += ResetPlayer;
        Player.OnPlayerDie += DisablePause;
        DialogueManager.OnDialogueEnter += DisablePause;
        DialogueManager.OnDialogueExit += EnablePause;
        
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= ResetPlayer;
        Player.OnPlayerDie -= DisablePause;
        DialogueManager.OnDialogueEnter -= DisablePause;
        DialogueManager.OnDialogueExit -= EnablePause;
    }

    private void ResetPlayer(GameObject playerObj)
    {
        _player = playerObj.GetComponent<Player>();
        _player.gameObject.GetComponent<PlayerLightShooting>().ResetLightCharges();

        EnablePause();
    }

    private IEnumerator COResetSequence()
    {
        UISceneTransitionController.Instance.OpenTransition();
        yield return new WaitForSeconds(2.0f);
        UISceneTransitionController.Instance.CloseTransition();
        PickablesManager.Instance.ResetPickables();
        SpawnManager.Instance.SpawnPlayer();
        OnGameReset?.Invoke();
    }

    private IEnumerator COWaitPauseCooldown() 
    {
        _canPause = false;
        yield return new WaitForSeconds(pauseCooldown);
        _canPause = true;
    }

    private void DisablePause()
    {
        _canPause = false;
        StopAllCoroutines();
    }

    private void EnablePause()
    {
        _canPause = true;
    }
}
