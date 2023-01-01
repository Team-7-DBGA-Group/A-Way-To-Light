using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Campfire : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DialogueTrigger dialogueTrigger;
    [SerializeField]
    private GameObject spawnPoint;

    [Header("General Settings")]
    [SerializeField]
    private int saveChoiceIndex = 0;
    [SerializeField]
    private int healAmount = 3;

    private GameObject _playerObj = null;

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

    private void DialogueTriggered(GameObject playerObj)
    {
        _playerObj = playerObj;
        
        HealPlayer();
        SpawnManager.Instance.SetNewSpawnPoint(spawnPoint.transform.position);

        DialogueManager.OnChoiceChosen += HandleChoice;
    }
    private void DialogueExit()
    {
        _playerObj = null;
        DialogueManager.OnChoiceChosen -= HandleChoice;
    }

    private void HandleChoice(int index)
    {
        if (index == saveChoiceIndex)
        {
            SaveGame();
        }
        else
        {
            DialogueManager.Instance.ExitDialogueMode();
        }
            
    }

    private void SaveGame()
    {
        // Call Save Game from Manager here ...
    }

    private void HealPlayer()
    {
        if (_playerObj == null)
            return;

        Player player = _playerObj.GetComponent<Player>();
        player.Heal(healAmount);
    }
}
