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

    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay, looped);
    }
}
