using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound : MonoBehaviour
{
    [Header("AudioSource reference")]
    [SerializeField]
    private AudioSource audioSource;

    [Header("Sounds settings")]
    [SerializeField]
    private AudioClip breakingWaterBarrierSound;
    [SerializeField]
    private AudioClip waterFlowSound;


    public void PlayBreakingSound()
    {
        audioSource.PlayOneShot(breakingWaterBarrierSound);
    }

    public void PlayWaterSound()
    {
        audioSource.PlayOneShot(waterFlowSound);
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
        if (audioSource != null)
            audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
