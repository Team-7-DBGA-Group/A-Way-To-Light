using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private UIHealthPanel healthPanel;

    [Header("Model Reference")]
    [SerializeField]
    private Actor actor; // if null is the player actor

    private void OnEnable()
    {
        if(actor != null)
        {
            actor.OnHealthInitialized += healthPanel.InitializePanel;
            actor.OnHealthDamaged += UpdateDamageHealth;
            actor.OnHealthHealed += UpdateHealHealth;
            DialogueManager.OnDialogueEnter += HidePanel;
            DialogueManager.OnDialogueExit += ShowPanel;
        }
        else
            SpawnManager.OnPlayerSpawn += InitializeActorsEvents;
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= InitializeActorsEvents;
        actor.OnHealthInitialized -= healthPanel.InitializePanel;
        actor.OnHealthDamaged -= UpdateDamageHealth;
        actor.OnHealthHealed -= UpdateHealHealth;
        DialogueManager.OnDialogueEnter -= HidePanel;
        DialogueManager.OnDialogueExit -= ShowPanel;
    }

    private void ShowPanel() => healthPanel.SetEnable(true);
    private void HidePanel() => healthPanel.SetEnable(false);

    private void UpdateHealHealth(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            healthPanel.FillHeart();
        }
    }

    private void UpdateDamageHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            healthPanel.EmptyHeart();
        }
    }

    private void InitializeActorsEvents(GameObject playerObj)
    {
        if (actor != null)
            return;
        actor = playerObj.GetComponent<Actor>();

        actor.OnHealthInitialized += healthPanel.InitializePanel;
        actor.OnHealthDamaged += UpdateDamageHealth;
        actor.OnHealthHealed += UpdateHealHealth;
        DialogueManager.OnDialogueEnter += HidePanel;
        DialogueManager.OnDialogueExit += ShowPanel;

        actor.ResetHealth();
    }
}
