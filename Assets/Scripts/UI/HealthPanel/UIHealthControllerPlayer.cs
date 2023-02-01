using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthControllerPlayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private UIHealthPanel healthPanel;

    [Header("Model Reference")]
    [SerializeField]
    private Actor actor;

    private void Start()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        if (actor != null)
        {
            actor.OnHealthInitialized += healthPanel.InitializePanel;
            actor.OnHealthDamaged += UpdateDamageHealth;
            actor.OnHealthHealed += UpdateHealHealth;
            DialogueManager.OnDialogueEnter += HidePanel;
            SpawnManager.OnPlayerSpawn += ResetHealth;

            EnemyManager.OnCombatEnter += ShowPanel;
            EnemyManager.OnCombatExit += HidePanel;
            EnemyManager.OnBossCombatEnter += ShowPanel;
            EnemyManager.OnBossCombatExit += HidePanel;

            actor.ResetHealth();
        }
    }

    private void OnDisable()
    {
        SpawnManager.OnPlayerSpawn -= ResetHealth;
        actor.OnHealthInitialized -= healthPanel.InitializePanel;
        actor.OnHealthDamaged -= UpdateDamageHealth;
        actor.OnHealthHealed -= UpdateHealHealth;
        DialogueManager.OnDialogueEnter -= HidePanel;

        EnemyManager.OnCombatEnter -= ShowPanel;
        EnemyManager.OnCombatExit -= HidePanel;
        EnemyManager.OnBossCombatEnter -= ShowPanel;
        EnemyManager.OnBossCombatExit -= HidePanel;
    }

    private void ShowPanel() => healthPanel.SetEnable(true);
    private void HidePanel() => healthPanel.SetEnable(false);

    private void UpdateHealHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
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

    private void ResetHealth(GameObject playerObj)
    {
        actor.ResetHealth();
        HidePanel();
    }
}
