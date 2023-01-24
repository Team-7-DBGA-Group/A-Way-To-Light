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

    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource barrierAudioSource;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip barrierUpSound;
    [SerializeField]
    private AudioClip barrierDownSound;
    [SerializeField]
    private AudioClip magicCircleSound;

    public void DeactivateBarrier()
    {
        barrierParticle.Stop();
        barrierCollider.enabled = false;
        //barrierAudioSource.Stop();
        //barrierAudioSource.PlayOneShot(barrierDownSound);
        OnBarrierDeactivated?.Invoke();
    }

    public void ActivateBarrier()
    {
        ResetGate();
        barrierParticle.Play();
        barrierCollider.enabled = true;
        //barrierAudioSource.PlayOneShot(barrierUpSound);
        //barrierAudioSource.Play();
        OnBarrierActivated?.Invoke();
    }

    protected override void GateOpenedAction()
    {

        DeactivateBarrier();
    }

    protected override void Awake()
    {
        base.Awake();
        barrierAudioSource.clip = magicCircleSound;
    }

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    private void ChangeSoundVolume()
    {
        if (barrierAudioSource != null)
            barrierAudioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
