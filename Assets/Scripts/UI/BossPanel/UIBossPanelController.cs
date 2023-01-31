using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossPanelController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIChargesPanel uiBossCharges;
    [SerializeField]
    private GameObject bossContainer;

    private bool _isInit = false;

    private void Start()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        EnemyManager.OnBossCombatEnter += InitPanel;
        EnemyManager.OnBossCombatExit += ResetPanel;
        EnemyManager.OnBossCombatExit += HidePanel;
        Player.OnPlayerDie += ResetPanel;
        Player.OnPlayerDie += HidePanel;
        Boss.OnPhaseChanged += NextPhase;
    }

    private void OnDisable()
    {
        EnemyManager.OnBossCombatEnter -= InitPanel;
        EnemyManager.OnBossCombatExit -= ResetPanel;
        EnemyManager.OnBossCombatExit -= HidePanel;
        Boss.OnPhaseChanged -= NextPhase;
        Player.OnPlayerDie -= ResetPanel;
        Player.OnPlayerDie -= HidePanel;
    }

    private void InitPanel()
    {
        ShowPanel();
        if (!_isInit)
        {
            uiBossCharges.InitializePanel(3);
            _isInit = true;
            return;
        }
        ResetPanel();
    }

    private void NextPhase()
    {
        uiBossCharges.DisableCharge();
    }

    private void ResetPanel()
    {
        for(int i = 0; i<3; ++i)
        {
            uiBossCharges.ActiveCharge();
        }
    }

    private void HidePanel() => bossContainer.SetActive(false);
    private void ShowPanel() => bossContainer.SetActive(true);
}
