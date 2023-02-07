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
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip onActivationSound;

    protected override void Awake()
    {
        base.Awake();
        if(audioSource != null) 
        {
            audioSource.clip = onActivationSound;
        }
    }

    protected override void GateOpenedAction()
    {
        animationToPlay.Play();
        audioSource.Play();
        Gate.StartFloating();
    }
}
