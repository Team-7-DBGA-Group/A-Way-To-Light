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
    private Actor actor;

    private void OnEnable()
    {
        actor.OnHealthInitialized += healthPanel.InitializePanel;
        actor.OnHealthDamaged += UpdateDamageHealth;
        actor.OnHealthHealed += UpdateHealHealth;
    }

    private void OnDisable()
    {
        actor.OnHealthInitialized -= healthPanel.InitializePanel;
        actor.OnHealthDamaged -= UpdateDamageHealth;
        actor.OnHealthHealed -= UpdateHealHealth;
    }

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
}
