using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossBarrier : Gate
{
    public Action OnBarrierDeactivated;
    public Action OnBarrierActivated;

    [Header("References")]
    [SerializeField]
    private ParticleSystem barrierParticle;
    [SerializeField]
    private Collider barrierCollider;

    public void DeactivateBarrier()
    {
        barrierParticle.Stop();
        barrierCollider.enabled = false;
        OnBarrierDeactivated?.Invoke();
    }

    public void ActivateBarrier()
    {
        ResetGate();
        barrierParticle.Play();
        barrierCollider.enabled = true;
        OnBarrierActivated?.Invoke();
    }

    protected override void GateOpenedAction()
    {
        DeactivateBarrier();
    }
}
