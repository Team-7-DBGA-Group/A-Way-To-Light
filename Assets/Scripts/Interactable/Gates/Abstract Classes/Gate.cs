using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gate : MonoBehaviour
{
    [SerializeField]
    private List<GateKey> gateKeys = new List<GateKey>();

    protected abstract void GateOpenedAction();

    public void OpenCheck()
    {
        foreach(GateKey gateKey in gateKeys)
        {
            if (gateKey != null && !gateKey.IsKeyActive)
                return;
        }
        GateOpenedAction();
    }

    void Start()
    {
        foreach (GateKey key in gateKeys)
        {
            if(key != null)
                key.SetGate(this);
        }
    }
}
