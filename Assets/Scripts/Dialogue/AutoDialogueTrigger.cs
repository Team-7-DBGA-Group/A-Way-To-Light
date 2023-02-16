using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AutoDialogueTrigger : MonoBehaviour, IDataPersistence
{
    public event Action<GameObject> OnDialogueTriggered;
    // Mattia changes
    [Header("Dialogue Object")]
    [Tooltip("Object for LookAt")]
    [SerializeField]
    private GameObject dialogueObj;
    [SerializeField]
    private bool shouldPlayerLookDialogueObj = false;
    // End Mattia changes

    [Header("Save System")]
    [SerializeField]
    protected string ID;
    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private bool _playerInRange;
    private GameObject _playerObj;

    private bool _canTriggerDialogue = true;
    private bool _playOnce = false;
    private bool _canEnableDialogue = true;

    public void LoadData(GameData data)
    {
        bool canPlay = false;
        data.AutoDialoguesActivated.TryGetValue(ID, out canPlay);
        _playOnce = canPlay;
    }

    public void SaveData(GameData data)
    {
        if (data.AutoDialoguesActivated.ContainsKey(ID))
            data.AutoDialoguesActivated.Remove(ID);

        data.AutoDialoguesActivated.Add(ID, _playOnce);
    }

    public void EnableTriggerDialogue()
    {
        if (!_canEnableDialogue)
            return;

        _canTriggerDialogue = true;
    }

    public void DisableTriggerDialogue() => _canTriggerDialogue = false;

    public void SetCanEnableDialogue(bool active) => _canEnableDialogue = active;

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

            // Mattia Changes
            if (shouldPlayerLookDialogueObj)
                _playerObj.transform.LookAt(transform, Vector3.up);

            if (dialogueObj != null)
                dialogueObj.transform.LookAt(_playerObj.transform, Vector3.up);
            // End Mattia Changes
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
