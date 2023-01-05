using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private AudioClip musicOneClip;
    [SerializeField] private AudioClip musicTwoClip;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    private void PlayAmbient()
    {
        _audioSource.PlayOneShot(ambientClip);
    }

    public void PlayMainMenuMusic(float delay)
    {
        _audioSource.clip = musicOneClip;
        PlayAmbient();
        _audioSource.PlayDelayed(delay);
        Invoke(nameof(OnAudioEnd), musicOneClip.length);
        
    }

    public void PlayFightMusic(float delay)
    {
        _audioSource.clip = musicTwoClip;
        _audioSource.PlayDelayed(delay);
        Invoke(nameof(OnAudioEnd), musicTwoClip.length);
    }

    private void OnAudioEnd()
    {
        Debug.Log("audio finished");
        if (_audioSource.clip == musicOneClip)
        {
            PlayMainMenuMusic(0f);
        }
        else
        {
            PlayFightMusic(0f);
        }
    }
}
