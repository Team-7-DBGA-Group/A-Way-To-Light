using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarKey : GateKey
{
    [Header("References")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private string animationName = "";

    [Header("Materials settings")]
    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = offMaterial;
    }

    protected override void CustomInteraction()
    {
        _meshRenderer.material = onMaterial;
        animator.SetTrigger(animationName);
    }
}
