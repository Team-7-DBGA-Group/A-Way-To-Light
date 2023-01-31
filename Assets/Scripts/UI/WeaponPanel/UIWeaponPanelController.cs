using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private UIChargesPanel uiWeaponPanel = null;
    [SerializeField]
    private UIWeaponIcon uiWeaponIcon;

    private bool _isInit = false;
    private bool _isInvincible = false;
    private void OnEnable()
    {
        Player.OnWeaponEquip += InitializePanel;
        Player.OnWeaponUnequip += ResetPanel;
        Player.OnPlayerDie += ResetPanel;
        Weapon.OnWeaponDurabilityChanged += DurabilityChange;
    }

    private void OnDisable()
    {
        Player.OnWeaponEquip -= InitializePanel;
        Player.OnWeaponUnequip -= ResetPanel;
        Weapon.OnWeaponDurabilityChanged -= DurabilityChange;
        Player.OnPlayerDie -= ResetPanel;
    }

    private void InitializePanel(Weapon w)
    {
        int dur = w.Durability;
        if(w.Durability > 3)
        {
            dur = 3;
            _isInvincible = true;
        }
            

        uiWeaponIcon.SetIcon(w.Icon);

        if (!_isInit)
        {
            uiWeaponPanel.InitializePanel(dur);
            _isInit = true;
            return;
        }

        for (int i = 0; i < dur; i++)
        {
            uiWeaponPanel.ActiveCharge();
        }
    }

    private void ResetPanel()
    {
        _isInvincible = false;
        for(int i=0; i<3; i++)
        {
            uiWeaponPanel.DisableCharge();
        }
        uiWeaponIcon.ResetIcon();
    }

    private void DurabilityChange(int newDur)
    {
        if (_isInvincible)
            return;
        uiWeaponPanel.DisableCharge();
        if(newDur <=0)
            ResetPanel();
    }
}
