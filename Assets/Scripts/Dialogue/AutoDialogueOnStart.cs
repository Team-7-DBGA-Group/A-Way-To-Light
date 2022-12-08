using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDialogueOnStart : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    [SerializeField]
    private float delay = 1.0f;

    private void Start()
    {
        StartCoroutine(COTriggerDialogueWithDelay(delay));
    }

    private IEnumerator COTriggerDialogueWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!DialogueManager.Instance.IsDialoguePlaying)
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
    }
}
