using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitManager : Singleton<InitManager>
{
    [Header("Settings")]
    [SerializeField]
    private string sceneName;

    private void OnEnable()
    {
        DialogueManager.OnDialogueExit += LoadGame;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueExit -= LoadGame;
    }

    private void LoadGame()
    {
        NavigationManager.Instance.ChangeScene(sceneName);
    }
}
