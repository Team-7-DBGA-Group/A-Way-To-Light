using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : Gate
{
    [Header("References")]
    [SerializeField]
    private ParticleSystem barrierParticle;
    [SerializeField]
    private Collider barrierCollider;

    public void DeactivateBarrier()
    {
        barrierParticle.Stop();
        barrierCollider.enabled = false;
    }

    public void ActivateBarrier()
    {
        ResetGate();
        barrierParticle.Play();
        barrierCollider.enabled = true;
    }

    protected override void GateOpenedAction()
    {
        DeactivateBarrier();
    }
}
