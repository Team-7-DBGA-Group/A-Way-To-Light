using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Campfire : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DialogueTrigger dialogueTrigger;

    [Header("General Settings")]
    [SerializeField]
    private int saveChoiceIndex = 0;

    private void OnEnable()
    {
        dialogueTrigger.OnDialogueTriggered += DialogueTriggered;
        DialogueManager.OnDialogueExit += DialogueExit;
    }

    private void OnDisable()
    {
        dialogueTrigger.OnDialogueTriggered -= DialogueTriggered;
        DialogueManager.OnDialogueExit -= DialogueExit;
    }

    private void DialogueTriggered() => DialogueManager.OnChoiceChosen += HandleChoice;
    private void DialogueExit() => DialogueManager.OnChoiceChosen -= HandleChoice;

    private void HandleChoice(int index)
    {
        if (index == saveChoiceIndex)
            SaveGame();
        else
            DialogueManager.Instance.ExitDialogueMode();
    }

    private void SaveGame()
    {
        // Call Save Game from Manager here ...
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "Game Saved! ");
    }
}
