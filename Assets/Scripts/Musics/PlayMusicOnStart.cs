using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private bool looped = false;
    [SerializeField]
    private AudioClip musicToPlay;
    [SerializeField]
    private bool playOnStart = true;

    private void Start()
    {
        if(playOnStart)
            AudioManager.Instance.PlayMusic(musicToPlay, looped);
    }

    public void PlayMusic()
    {
        AudioManager.Instance.PlayMusic(musicToPlay, looped);
    }
}
