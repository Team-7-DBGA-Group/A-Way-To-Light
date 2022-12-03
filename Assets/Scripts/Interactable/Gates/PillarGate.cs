using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGate : Gate
{
    [Header("References")]
    [SerializeField]
    private Animator animator = null;

    [Header("Settings")]
    [SerializeField]
    private string stateName = "";

    protected override void GateOpenedAction()
    {
        animator.Play(stateName);
    }
}
