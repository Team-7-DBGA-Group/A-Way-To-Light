using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGate : Gate
{
    [SerializeField]
    private GameObject vfx;

    private SphereCollider _collider;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
    }

    protected override void GateOpenedAction()
    {
        vfx.SetActive(true);
        _collider.enabled = true;
    }
}
