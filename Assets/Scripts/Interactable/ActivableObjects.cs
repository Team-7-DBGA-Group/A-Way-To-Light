using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivableObjects : InteractableObject
{
    [Header("Objects References")]
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    [Header("Audio Reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sound")]
    [SerializeField]
    private AudioClip soundToPlayOnActivation;

    [Header("Sound settings")]
    [SerializeField]
    private bool looped;

    private void Awake()
    {
        if(audioSource != null)
        {
            audioSource.clip = soundToPlayOnActivation;
            audioSource.loop = looped;
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
        if(audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
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
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
