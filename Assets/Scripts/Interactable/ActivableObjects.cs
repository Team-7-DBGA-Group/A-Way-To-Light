using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SoundEffect
{
    [Header("Audio Reference")]
    [SerializeField]
    public AudioSource audioSource;

    [Header("Sound")]
    [SerializeField]
    public AudioClip soundToPlayOnActivation;

    [Header("Sound settings")]
    [SerializeField]
    public bool looped;
}

public class ActivableObjects : InteractableObject
{
    [Header("Objects References")]
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    [SerializeField]
    private List<SoundEffect> effects = new List<SoundEffect>();

    private void Awake()
    {
        foreach(SoundEffect item in effects)
        {
            if (item.audioSource != null)
            {
                item.audioSource.clip = item.soundToPlayOnActivation;
                item.audioSource.loop = item.looped;
            }
        }
    }

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    public override void Interact()
    {
        base.Interact();
        ActiveObjects();
        PlayEffects();
    }

    private void PlayEffects()
    {
        if (effects.Count <= 0)
            return;
        foreach (SoundEffect item in effects)
            item.audioSource?.Play();
    }

    private void ActiveObjects()
    {
        if(objects.Count <= 0)
            return;

        foreach(GameObject obj in objects)
            obj.SetActive(true);
    }

    private void ChangeSoundVolume()
    {
        foreach (SoundEffect item in effects)
        {
            if (item.audioSource != null)
                item.audioSource.volume = AudioManager.Instance.GetSoundVolume();
        }
    }
}
