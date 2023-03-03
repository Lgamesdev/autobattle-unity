using System;
using LGamesDev;
using LGamesDev.UI;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace LGamesDev
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        //[SerializeField] private AudioClip ambientClip;
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

        /*public void PlayAmbient()
        {
            _audioSource.PlayOneShot(ambientClip);
        }*/

        public void PlayMusic(float delay)
        {
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

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        public void SetMixerVolume(AudioTrack track, float value)
        {
            if (mixer != null)
            {
                mixer.SetFloat(track.ToString(), Mathf.Log(value) * 20f);
            }
            else
            {
                Debug.Log("mixer is not set");
            }
        }
    }
    
    public enum AudioTrack
    {
        Music,
        Effects
    }
}
