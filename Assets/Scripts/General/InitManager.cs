using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitManager : Singleton<InitManager>
{
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
        SceneManager.LoadScene("Prototype");
    }
}
