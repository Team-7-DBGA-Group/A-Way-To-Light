using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>, IDataPersistence 
{
    public static event Action OnVolumeLoaded;
    public static event Action OnChangedSoundVolume;

    public float SoundVolume { get => _soundVolume; }
    public float MusicVolume { get => _musicVolume; }
    public float EffectsVolume { get => _effectsVolume; }
    public float MasterVolume { get => _masterVolume; }

    [Header("Sources")]
    [SerializeField]
    private List<AudioSource> soundSources = new List<AudioSource>();
    [SerializeField]
    private List<AudioSource> musicSource = new List<AudioSource>();
    [SerializeField]
    private List<AudioSource> effectsSource = new List<AudioSource>();

    // Values to save
    private float _soundVolume;
    private float _musicVolume;
    private float _effectsVolume;
    private float _masterVolume;

    // Sounds controls
    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip == null)
            return;
        if (soundSources.Count <= 0)
            return;

        foreach(AudioSource source in soundSources)
        {
            if (!source.isPlaying)
            {
                source.clip = audioClip;
                source.PlayOneShot(audioClip);
                return;
            }
        }
    }
    public void PlaySound(AudioClip audioClip, bool isLooped)
    {
        if (soundSources.Count <= 0)
            return;

        foreach (AudioSource source in soundSources)
        {
            if (!source.isPlaying)
            {
                source.loop = isLooped;
                source.clip = audioClip;
                source.Play();
                return;
            }
        }
    }

    public void StopSound(AudioClip audioClip)
    {
        if (soundSources.Count <= 0)
            return;

        foreach (AudioSource source in soundSources)
        {
            if (source.isPlaying && source.clip == audioClip)
            {
                source.clip = null;

                if (source.loop == true)
                    source.loop = false;

                source.Stop();
                return;
            }
        }
    }

    public void PauseSounds()
    {
        if (soundSources.Count <= 0)
            return;

        foreach(AudioSource source in soundSources)
        {
            source.Pause();
        }
    }
    public void StopSounds()
    {
        if (soundSources.Count <= 0)
            return;

        foreach (AudioSource source in soundSources)
        {
            source.loop = false;
            source.clip = null;
            source.Stop();
        }
    }
    public void MuteSounds(bool muteValue)
    {
        if (soundSources.Count <= 0)
            return;

        foreach (AudioSource source in soundSources)
        {
            source.mute = muteValue;
        }
    }
    public void SetSoundVolume(float amount)
    {
        if (soundSources.Count <= 0)
            return;

        foreach (AudioSource source in soundSources)
        {
            source.volume = amount;
            _soundVolume = amount;
        }

        OnChangedSoundVolume?.Invoke();
    }
    public float GetSoundVolume()
    {
        return _soundVolume;
    }

    // Music controls
    public void PlayMusic(AudioClip audioClip)
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            if (!source.isPlaying)
            {
                source.clip = audioClip;
                source.PlayOneShot(audioClip);
                return;
            }
        }
    }
    public void PlayMusic(AudioClip audioClip, bool isLooped)
    {
        if (musicSource.Count <= 0)
        {
            Debug.Log("NOMUSICSOURCE");
            return;
        }

        foreach (AudioSource sources in musicSource)
        {
            if (!sources.isPlaying)
            {
                sources.loop = isLooped;
                sources.clip = audioClip;
                sources.Play();
                return;
            }
        }
    }

    public void PauseMusic()
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            source.Pause();
        }
    }

    public void StopMusic()
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            source.loop = false;
            source.clip = null;
            source.Stop();
        }
    }

    public void StopMusic(AudioClip audioClip)
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            if (source.isPlaying && source.clip == audioClip)
            {
                source.clip = null;

                if (source.loop == true)
                    source.loop = false;

                source.Stop();
                return;
            }
        }
    }

    public void MuteMusic(bool muteValue) 
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            source.mute = muteValue;
        }
    }
    public void SetMusicVolume(float amount)
    {
        if (musicSource.Count <= 0)
            return;

        foreach (AudioSource source in musicSource)
        {
            source.volume = amount;
            _musicVolume = amount;
        }
    }

    // Effect controls
    public void PlayEffect(AudioClip audioClip)
    {
        if (effectsSource.Count <= 0)
            return;

        foreach (AudioSource source in effectsSource)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(audioClip);
                return;
            }
        }
    }

    public void PauseEffects()
    {
        if (effectsSource.Count <= 0)
            return;

        foreach (AudioSource source in effectsSource)
        {
            source.Pause();
        }
    }

    public void StopEffects()
    {
        if (effectsSource.Count <= 0)
            return;

        foreach (AudioSource source in effectsSource)
        {
            source.Stop();
        }
    }
    public void MuteEffects(bool muteValue)
    {
        if (effectsSource.Count <= 0)
            return;

        foreach (AudioSource source in effectsSource)
        {
            source.mute = muteValue;
        }
    }
    public void SetEffectsVolume(float amount)
    {
        if (effectsSource.Count <= 0)
            return;

        foreach (AudioSource sources in effectsSource)
        {
            sources.volume = amount;
            _musicVolume = amount;
        }
    }

    // General 
    public void StopAll()
    {
        StopSounds();
        StopMusic();
        StopEffects();
    }
    public void SetMasterVolume(float amount)
    {
        _masterVolume = amount;

        _soundVolume *= _masterVolume;
        SetSoundVolume(_soundVolume);
        _musicVolume *= _masterVolume;
        SetMusicVolume(_musicVolume);
        _effectsVolume *= _masterVolume;
        SetEffectsVolume(_effectsVolume);
    }

    // Save system
    public void LoadData(GameData data)
    {
        
        //_masterVolume = data.MasterVolume;
        //SetMasterVolume(_masterVolume);
        _soundVolume = data.SoundVolume/* * _masterVolume*/;
        //SetSoundVolume(_soundVolume);
        //_effectsVolume = data.EffectsVolume/* * _masterVolume*/;
        //SetEffectsVolume(_effectsVolume);
        _musicVolume = data.MusicVolume/* * _masterVolume*/;
        //SetMusicVolume(_musicVolume);
        Debug.Log("Sound: " + _soundVolume + " Music: " + _musicVolume);
        OnVolumeLoaded?.Invoke();
    }

    public void SaveData(GameData data)
    {
        //data.MasterVolume = _masterVolume;
        data.SoundVolume = _soundVolume;
        //data.EffectsVolume = _effectsVolume;
        data.MusicVolume = _musicVolume;
        Debug.Log("Sound: " + _soundVolume + " Music: " + _musicVolume);
    }

    // Audio effects
    private IEnumerator COStartFade(AudioSource audioSource, float duration, float targetVolume) 
    { 
        float currentTime = 0; 
        float start = audioSource.volume; 
        while (currentTime < duration) 
        { 
            currentTime += Time.deltaTime; 
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration); 
            yield return null; 
        } 
        yield break; 
    }
}
