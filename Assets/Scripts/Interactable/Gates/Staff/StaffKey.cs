using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffKey : GateKey
{
    [Header("Materials settings")]
    [SerializeField]
    private Material OffMaterial;
    [SerializeField]
    private Material OnMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = OffMaterial;
    }

    protected override void CustomInteraction()
    {
        _meshRenderer.material = OnMaterial;
    }
}
