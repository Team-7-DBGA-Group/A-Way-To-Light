using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGate : Gate
{
    [Header("References")]
    [SerializeField]
    private Animator animator = null;

    protected override void GateOpenedAction()
    {
        animator.SetTrigger("Open");
    }
}
