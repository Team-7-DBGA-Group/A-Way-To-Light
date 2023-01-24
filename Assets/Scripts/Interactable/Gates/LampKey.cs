using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampKey : GateKey
{
    [Header("Sound")]
    [SerializeField]
    private AudioClip onActivationSound;

    [Header("References")]
    [SerializeField]
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource.clip = onActivationSound;
    }

    protected override void CustomInteraction()
    {
        if (onActivationSound != null && !_audioSource.isPlaying)
            _audioSource.Play();
    }
}
