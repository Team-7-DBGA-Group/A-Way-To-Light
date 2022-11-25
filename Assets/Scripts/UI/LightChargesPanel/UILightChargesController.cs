using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightChargesController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private UILightChargesPanel uiLightChargesPanel = null;

    private void OnEnable()
    {
        PlayerLightShooting.OnChargesInitialized += uiLightChargesPanel.InitializePanel;
        PlayerLightShooting.OnShot += uiLightChargesPanel.DisableCharge;
        PlayerLightShooting.OnChargeCooldownFinished += uiLightChargesPanel.ActiveCharge;
        DialogueManager.OnDialogueEnter += HidePanel;
        DialogueManager.OnDialogueExit += ShowPanel;
    }

    private void OnDisable()
    {
        PlayerLightShooting.OnChargesInitialized -= uiLightChargesPanel.InitializePanel;
        PlayerLightShooting.OnShot -= uiLightChargesPanel.DisableCharge;
        PlayerLightShooting.OnChargeCooldownFinished -= uiLightChargesPanel.ActiveCharge;
        DialogueManager.OnDialogueEnter -= HidePanel;
        DialogueManager.OnDialogueExit -= ShowPanel;
    }

    private void ShowPanel() => uiLightChargesPanel.SetEnable(true);
    private void HidePanel() => uiLightChargesPanel.SetEnable(false);
}
