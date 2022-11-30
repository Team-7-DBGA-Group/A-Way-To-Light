using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    public event Action OnDialogueTriggered;

    [Header("Dialogue Object")]
    [Tooltip("Object for LookAt")]
    [SerializeField]
    private GameObject dialogueObj;

    [Header("Visual Cue")]
    [SerializeField] 
    private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] 
    private TextAsset inkJSON;

    private bool _playerInRange;
    private GameObject _playerObj;

    private bool _canTriggerDialogue = true;

    public void EnableTriggerDialogue() => _canTriggerDialogue = true;
    public void DisableTriggerDialogue() => _canTriggerDialogue = false;

    private void Awake() 
    {
        _playerInRange = false;
        visualCue.SetActive(false);
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
        if (!_canTriggerDialogue)
            return;

        if (_playerInRange && !DialogueManager.Instance.IsDialoguePlaying) 
        {
            visualCue.SetActive(true);
            if (InputManager.Instance.GetInteractPressed()) 
            {
                OnDialogueTriggered?.Invoke();
                DialogueManager.Instance.EnterDialogueMode(inkJSON);
                _playerObj.transform.LookAt(transform, Vector3.up);

                if(dialogueObj != null)
                    dialogueObj.transform.LookAt(_playerObj.transform, Vector3.up);
                else
                    transform.LookAt(_playerObj.transform, Vector3.up);
            }
        }
        else 
        {
            visualCue.SetActive(false);
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