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
        AudioManager.OnVolumeLoaded += SetSliderValues;
        UIAudio.OnSoundChange += SetSoundVolume;
        UIAudio.OnMusicChange += SetMusicVolume;
        
    }
    private void OnDisable()
    {
        AudioManager.OnVolumeLoaded -= SetSliderValues;
        UIAudio.OnSoundChange -= SetSoundVolume;
        UIAudio.OnMusicChange -= SetMusicVolume;
        
    }

    public void SetSliderValues()
    {
        musicSlider.value = AudioManager.Instance.MusicVolume;
        soundSlider.value = AudioManager.Instance.SoundVolume;
    }


    public void SetSoundVolume() => AudioManager.Instance.SetSoundVolume(soundSlider.value);
    
        
    public void SetMusicVolume() => AudioManager.Instance.SetMusicVolume(musicSlider.value);
        //AudioManager.Instance.SetMasterVolume(masterSlider.value);
    

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
