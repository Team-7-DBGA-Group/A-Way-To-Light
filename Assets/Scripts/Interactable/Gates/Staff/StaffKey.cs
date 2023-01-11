using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StaffKey : GateKey
{
    [Header("References")]
    [SerializeField]
    private GameObject deactivatingObject;

    [Header("Materials settings")]
    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;

    private MeshRenderer _meshRenderer;
    private Material[] _mats;

    protected override void CustomInteraction()
    {
        GameObject destroyObject = deactivatingObject;
        Destroy(destroyObject);
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

}
