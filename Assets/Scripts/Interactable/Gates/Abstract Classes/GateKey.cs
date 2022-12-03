using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GateKey : MonoBehaviour, IInteractable
{
    public bool IsKeyActive { get; private set; }
    private Gate _gateReference = null;

    protected abstract void CustomInteraction();

    public void SetGate(Gate gate)
    {
        _gateReference = gate;
    }

    public void Interact()
    {
        CustomInteraction();
        ActiveKey();
    }

    private void ActiveKey()
    {
        if (IsKeyActive)
            return;

        IsKeyActive = true;
        _gateReference.OpenCheck();
    }
}
