using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorSound : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField]
    private AudioClip soundToPlay;

    private AudioSource _audioSource;

    public void PlayTrapdoorSound()
    {
        _audioSource.Play();
    }

    public void StopTrapdoorSound()
    {
        _audioSource.Stop();
    }

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
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = soundToPlay;
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
        }
    }

    private void ChangeSoundVolume()
    {
        if (_audioSource != null)
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
