using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private bool looped;

    [Header("Sound")]
    [SerializeField]
    private AudioClip soundToPlay;

    private AudioSource _audioSource;

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
            _audioSource.loop = looped;
        }
    }

    private void Start()
    {
        if(_audioSource != null)
            _audioSource.Play();    
    }

    private void ChangeSoundVolume()
    {
        if (_audioSource != null)
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
