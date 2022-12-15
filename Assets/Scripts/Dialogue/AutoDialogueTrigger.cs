using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoDialogueTrigger : MonoBehaviour
{
    public event Action<GameObject> OnDialogueTriggered;

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private bool _playerInRange;
    private GameObject _playerObj;

    private bool _canTriggerDialogue = true;
    private bool _playOnce = false;

    public void EnableTriggerDialogue() => _canTriggerDialogue = true;
    public void DisableTriggerDialogue() => _canTriggerDialogue = false;


    private void Awake()
    {
        _playerInRange = false;
    }

    private void OnEnable()
    {
        EnemyManager.OnCombatEnter += DisableTriggerDialogue;
        EnemyManager.OnCombatExit += EnableTriggerDialogue;
    }

    private void OnDisable()
    {
        EnemyManager.OnCombatEnter -= DisableTriggerDialogue;
        EnemyManager.OnCombatExit -= EnableTriggerDialogue;
    }

    private void Update()
    {
        if (_playOnce)
            return;
        if (!_canTriggerDialogue)
            return;

        if (_playerInRange && !DialogueManager.Instance.IsDialoguePlaying)
        {
            _playOnce = true;
            OnDialogueTriggered?.Invoke(_playerObj);
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (EnemyManager.Instance.CurrentEnemiesInCombat > 0)
            return;

        if (collider.gameObject.tag == "Player")
        {
            _playerInRange = true;
            _playerObj = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (EnemyManager.Instance.CurrentEnemiesInCombat > 0)
            return;

        if (collider.gameObject.tag == "Player")
        {
            _playerInRange = false;
        }
    }
}
