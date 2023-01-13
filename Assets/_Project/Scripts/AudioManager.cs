using System;
using LGamesDev;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private AudioClip[] musicClips;

    private int _musicClipIndex;
    
    [SerializeField] private AudioClip fightMusicClip;
    [SerializeField] private AudioClip winFightMusicClip;
    [SerializeField] private AudioClip loseFightMusicClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        
        _musicClipIndex = Random.Range(0, musicClips.Length - 1);
    }

    public void PlayAmbient()
    {
        //_audioSource.PlayOneShot(ambientClip);
    }

    public void PlayMusic(float delay)
    {
        PlayAmbient();
        _audioSource.clip = musicClips[_musicClipIndex];
        _audioSource.PlayDelayed(delay);

        if (_musicClipIndex < musicClips.Length - 1)
        {
            _musicClipIndex++;
        }
        else
        {
            _musicClipIndex = 0;
        }
    }

    public void PlayFightMusic(float delay)
    {
        PlayAmbient();
        _audioSource.clip = fightMusicClip;
        _audioSource.PlayDelayed(delay);
    }
    
    public void PlayWinMusic()
    {
        _audioSource.clip = winFightMusicClip;
        _audioSource.Play();
    }
    
    public void PlayLoseMusic()
    {
        _audioSource.clip = loseFightMusicClip;
        _audioSource.Play();
    }
}
