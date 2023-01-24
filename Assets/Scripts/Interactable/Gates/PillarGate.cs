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

    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds settings")]
    [SerializeField]
    private AudioClip onOpenSound;

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource.clip = onOpenSound;
    }

    private void Start()
    {
        if(pillarLightRenderer != null)
         pillarLightRenderer.material = closedGateMaterial;    
    }

    protected override void GateOpenedAction()
    {
        animator.SetTrigger("Open");
        audioSource.Play();
        if (pillarLightRenderer != null)
            pillarLightRenderer.material = openGateMaterial;
    }

    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
