using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>, IDataPersistence 
{
    [Header("Sources")]
    [SerializeField]
    private List<AudioSource> soundSources;
    [SerializeField]
    private List<AudioSource> musicSource;
    [SerializeField]
    private List<AudioSource> effectsSource;

    // Values to save
    private float _soundVolume = 1f;
    private float _musicVolume = 1f;
    private float _effectsVolume = 1f;
    private float _masterVolume = 1f;

    // Sounds controls
    public void PlaySound(AudioClip audioClip)
    {
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
        foreach(AudioSource source in soundSources)
        {
            source.Pause();
        }
    }
    public void StopSounds()
    {
        foreach (AudioSource source in soundSources)
        {
            source.loop = false;
            source.clip = null;
            source.Stop();
        }
    }
    public void MuteSounds(bool muteValue)
    {
        foreach (AudioSource source in soundSources)
        {
            source.mute = muteValue;
        }
    }
    public void SetSoundVolume(float amount)
    {
        foreach (AudioSource source in soundSources)
        {
            source.volume = amount;
            _soundVolume = amount;
        }
    }

    // Music controls
    public void PlayMusic(AudioClip audioClip)
    {
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
        foreach (AudioSource source in musicSource)
        {
            source.Pause();
        }
    }
    public void StopMusic()
    {
        foreach (AudioSource source in musicSource)
        {
            source.loop = false;
            source.clip = null;
            source.Stop();
        }
    }

    public void StopMusic(AudioClip audioClip)
    {
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
        foreach (AudioSource source in musicSource)
        {
            source.mute = muteValue;
        }
    }
    public void SetMusicVolume(float amount) 
    {
        foreach(AudioSource source in musicSource)
        {
            source.volume = amount;
            _musicVolume = amount;
        }
    }

    // Effect controls
    public void PlayEffect(AudioClip audioClip)
    {
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
        foreach (AudioSource source in effectsSource)
        {
            source.Pause();
        }
    }
    public void StopEffects()
    {
        foreach (AudioSource source in effectsSource)
        {
            source.Stop();
        }
    }
    public void MuteEffects(bool muteValue)
    {
        foreach (AudioSource source in effectsSource)
        {
            source.mute = muteValue;
        }
    }
    public void SetEffectsVolume(float amount)
    {
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
        Debug.Log(data.MasterVolume);
        _masterVolume = data.MasterVolume;
        SetMasterVolume(_masterVolume);
        _soundVolume = data.SoundVolume * _masterVolume;
        SetSoundVolume(_soundVolume);
        _effectsVolume = data.EffectsVolume * _masterVolume;
        SetEffectsVolume(_effectsVolume);
        _musicVolume = data.MusicVolume * _masterVolume;
        SetMusicVolume(_musicVolume);
    }

    public void SaveData(GameData data)
    {
        data.MasterVolume = _masterVolume;
        data.SoundVolume = _soundVolume;
        data.EffectsVolume = _effectsVolume;
        data.MusicVolume = _musicVolume;
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
