using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGate : Gate
{
    [Header("References")]
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private MeshRenderer pillarLightRenderer;

    [Header("Settings")]
    [SerializeField]
    private Material closedGateMaterial; 
    [SerializeField]
    private Material openGateMaterial;

    private void Start()
    {
        if(pillarLightRenderer != null)
         pillarLightRenderer.material = closedGateMaterial;    
    }

    protected override void GateOpenedAction()
    {
        animator.SetTrigger("Open");
        if (pillarLightRenderer != null)
            pillarLightRenderer.material = openGateMaterial;
    }
}
