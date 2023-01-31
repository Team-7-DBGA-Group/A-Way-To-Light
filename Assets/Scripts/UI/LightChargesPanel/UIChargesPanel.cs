using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChargesPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject uiChargePrefab = null;

    private bool _isInit = false;

    private Stack<UICharge> _activeCharges = new Stack<UICharge>();
    private Stack<UICharge> _cooldownCharges = new Stack<UICharge>();

    public void SetEnable(bool enable)
    {
        if (!_isInit)
            return;

        foreach (UICharge charge in _activeCharges)
            charge.SetEnable(enable);
        foreach (UICharge charge in _cooldownCharges)
            charge.SetEnable(enable);
    }

    public void InitializePanel(int amount)
    {
        if (_isInit)
            return;

        for(int i= 0; i < amount; i++)
        {
            GameObject chargeObj = Instantiate(uiChargePrefab, this.transform);
            _activeCharges.Push(chargeObj.GetComponent<UICharge>());
        }

        _isInit = true;
    }

    public void DisableCharge()
    {
        if (!_isInit)
            return;

        if (_activeCharges.Count == 0)
            return;

        UICharge charge = _activeCharges.Pop();
        charge.Off();
        _cooldownCharges.Push(charge);
    }

    public void ActiveCharge()
    {
        if (!_isInit)
            return;

        if (_cooldownCharges.Count == 0)
            return;

        UICharge charge = _cooldownCharges.Pop();
        charge.On();
        _activeCharges.Push(charge);
    }
}
