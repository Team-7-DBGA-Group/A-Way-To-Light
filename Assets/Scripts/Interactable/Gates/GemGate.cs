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
    [SerializeField]
    private AudioClip teleportingSound;
    [SerializeField, HideInInspector]
    private bool isOpen = false;

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

    protected override void GateOpenedAction()
    {
        vfx.SetActive(true);
        AudioManager.Instance.PlaySound(portalActivatedSound);
        audioSource.Play();
    }

    private void ChangeSoundVolume()
    {
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }

    public void PlayTeleportSound()
    {
        AudioManager.Instance.PlaySound(teleportingSound);
    }
}
