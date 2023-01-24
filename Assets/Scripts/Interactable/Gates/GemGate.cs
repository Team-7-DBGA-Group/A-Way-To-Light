using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGate : Gate
{
    [SerializeField]
    private GameObject vfx;

    private SphereCollider _collider;

    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds settings")]
    [SerializeField]
    private AudioClip portalActivatedSound;
    [SerializeField]
    private AudioClip portalMantainingSound;

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
        audioSource.clip = portalMantainingSound;
    }

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
    }

    protected override void GateOpenedAction()
    {
        vfx.SetActive(true);
        _collider.enabled = true;
        AudioManager.Instance.PlaySound(portalActivatedSound);
        audioSource.Play();
    }

    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
