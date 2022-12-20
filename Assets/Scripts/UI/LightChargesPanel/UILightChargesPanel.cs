using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILightChargesPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject uiLightChargePrefab = null;

    private bool _isInit = false;

    private Stack<UILightCharge> _activeCharges = new Stack<UILightCharge>();
    private Stack<UILightCharge> _cooldownCharges = new Stack<UILightCharge>();

    public void SetEnable(bool enable)
    {
        if (!_isInit)
            return;

        foreach (UILightCharge charge in _activeCharges)
            charge.SetEnable(enable);
        foreach (UILightCharge charge in _cooldownCharges)
            charge.SetEnable(enable);
    }

    public void InitializePanel(int amount)
    {
        if (_isInit)
            return;

        for(int i= 0; i < amount; i++)
        {
            GameObject lightObj = Instantiate(uiLightChargePrefab, this.transform);
            _activeCharges.Push(lightObj.GetComponent<UILightCharge>());
        }

        _isInit = true;
    }

    public void DisableCharge()
    {
        if (!_isInit)
            return;

        if (_activeCharges.Count == 0)
            return;

        UILightCharge charge = _activeCharges.Pop();
        charge.Off();
        _cooldownCharges.Push(charge);
    }

    public void ActiveCharge()
    {
        if (!_isInit)
            return;

        if (_cooldownCharges.Count == 0)
            return;

        UILightCharge charge = _cooldownCharges.Pop();
        charge.On();
        _activeCharges.Push(charge);
    }
}
