using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UIAudio audioMenuPanel;

    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundSlider;

    private void OnEnable()
    {
        UIAudio.OnAudioChange += SetSoundVolume;
        AudioManager.OnVolumeLoaded += SetSliderValues;
    }
    private void OnDisable()
    {
        UIAudio.OnAudioChange -= SetSoundVolume;
        AudioManager.OnVolumeLoaded -= SetSliderValues;
    }

    public void SetSliderValues()
    {
        masterSlider.value = AudioManager.Instance.MasterVolume;
        musicSlider.value = AudioManager.Instance.MusicVolume;
        soundSlider.value = AudioManager.Instance.SoundVolume;
    }

    public void SetSoundVolume()
    {
        AudioManager.Instance.SetSoundVolume(soundSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetMasterVolume(masterSlider.value);
    }

    public void ShowAudioMenuPanel()
    {
        if (audioMenuPanel == null)
            return;

        audioMenuPanel.Open();
    }

    public void HideAudioMenuPanel()
    {
        if (audioMenuPanel == null)
            return;
        audioMenuPanel.Close();
    }
}
