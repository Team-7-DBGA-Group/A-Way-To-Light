using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillGate : Gate
{
    [Header("References")]
    [SerializeField]
    private Animation animationToPlay;
    [SerializeField]
    private GateElevation Gate;
    protected override void GateOpenedAction()
    {
        animationToPlay.Play();
        Gate.StartFloating();
    }
}
