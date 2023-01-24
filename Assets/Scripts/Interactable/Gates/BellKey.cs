using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellKey : GateKey
{
    [Header("Materials settings")]
    [SerializeField]
    private Material OffMaterial;
    [SerializeField]
    private Material OnMaterial;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip onActivateSound;

    private MeshRenderer _meshRenderer;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = OffMaterial;
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = onActivateSound;
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
        }
    }

    protected override void CustomInteraction()
    {
        _meshRenderer.material = OnMaterial;

        if (_audioSource != null && onActivateSound != null)
            _audioSource.Play();
    }
    private void ChangeSoundVolume()
    {
        if (_audioSource != null)
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
