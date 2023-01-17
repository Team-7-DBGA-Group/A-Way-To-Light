using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gate : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<GateKey> gateKeys = new List<GateKey>();

    private int _keyNum = 0;
    private int _currentKeyNum = 0;
    protected abstract void GateOpenedAction();

    public void OpenCheck()
    {
        _currentKeyNum++;
        if (_currentKeyNum != _keyNum)
            return;
        GateOpenedAction();
    }

    public void ResetGate()
    {
        foreach (GateKey gateKey in gateKeys)
            gateKey.ResetKey();
        _currentKeyNum = 0;
    }

    private void Start()
    {
        foreach (GateKey key in gateKeys)
        {
            if(key != null)
            {
                key.SetGate(this);
                _keyNum++;
            }
        }
    }
}
