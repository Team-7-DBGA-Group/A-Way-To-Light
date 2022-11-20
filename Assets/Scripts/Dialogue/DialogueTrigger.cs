using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] 
    private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] 
    private TextAsset inkJSON;

    private bool _playerInRange;
    private GameObject _playerObj;

    private void Awake() 
    {
        _playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update() 
    {
        if (EnemyManager.Instance.CurrentEnemiesInCombat > 0)
            return;

        if (_playerInRange && !DialogueManager.Instance.IsDialoguePlaying) 
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F)) 
            {
                DialogueManager.Instance.EnterDialogueMode(inkJSON);
                _playerObj.transform.LookAt(transform, Vector3.up);
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