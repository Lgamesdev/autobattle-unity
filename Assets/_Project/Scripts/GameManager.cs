using System;
using System.Collections;
using System.Collections.Generic;
using Core.Player;
using LGamesDev.Core;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Fighting;
using LGamesDev.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LGamesDev
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private SceneLoader sceneLoader;

        public LoadingScreen loadingScreen;
        public ModalWindowPanel modalWindow;
        public AudioManager audioManager;

        private Authentication _authentication;
        private const string AuthenticationKey = "authentication";
        private PlayerOptions _playerOptions;
        private const string OptionsKey = "options";

        private void Awake()
        {
            Instance = this;
            
            //Audio Manager
            audioManager = GetComponent<AudioManager>();
            //Scene Loader
            sceneLoader = GetComponent<SceneLoader>();
            //Loading Screen
            loadingScreen = GameObject.Find("/Canvas/LoadingScreen").GetComponent<LoadingScreen>();
            //Modal Window
            modalWindow = GameObject.Find("/Canvas/Modal Window Panel").GetComponent<ModalWindowPanel>();
            modalWindow.Close();

            //Authentication
            _authentication = JsonConvert.DeserializeObject<Authentication>(PlayerPrefs.GetString(AuthenticationKey));
            
            //Player Options
            _playerOptions = JsonConvert.DeserializeObject<PlayerOptions>(PlayerPrefs.GetString(OptionsKey)) ?? new PlayerOptions();
        }

        private void Start()
        {
            if (_authentication == null)
            {
                StartCoroutine(sceneLoader.LoadAuthentication(true, true));
            }
            else
            {
                StartCoroutine(sceneLoader.LoadAuthentication(true, false));
            }
            
            audioManager.SetMixerVolume(AudioTrack.Music, _playerOptions.MusicVolume);
            audioManager.SetMixerVolume(AudioTrack.Effects, _playerOptions.EffectsVolume);
        }

        public void LoadCustomization()
        {
            StartCoroutine(sceneLoader.LoadCustomization());
        }
        
        public void LoadMainMenu()
        {
            StartCoroutine(sceneLoader.LoadMainMenu());
        }

        public void PlayMainMenuMusic()
        {
            audioManager.PlayMusic(1f);
        }

        public void LoadFight(Fight fight)
        {
            StartCoroutine(sceneLoader.LoadFight(fight));
        }
        
        public void PlayFightMusic()
        {
            audioManager.PlayFightMusic(1f);
        }

        public void Logout()
        {
            PlayerPrefs.DeleteKey("authentication");
            _authentication = null;
            StartCoroutine(sceneLoader.LoadAuthentication(true, true));
        }

        public Authentication GetAuthentication()
        {
            return _authentication;
        }

        public void SetAuthentication(Authentication authentication)
        {
            if (authentication != null)
            {
                _authentication = authentication;
                PlayerPrefs.SetString(AuthenticationKey, JsonUtility.ToJson(authentication));
            }
            else
            {
                Debug.LogError("trying to set authentication to null");
            }
        }

        public PlayerOptions GetPlayerOptions()
        {
            return _playerOptions;
        }
        
        public void SetPlayerOptions(PlayerOptions playerOptions)
        {
            if (playerOptions != null)
            {
                _playerOptions = playerOptions;
                PlayerPrefs.SetString(OptionsKey, JsonUtility.ToJson(_playerOptions));
            }
            else
            {
                Debug.LogError("trying to set options to null");
            }
        }

        public SceneIndexes GetActiveScene()
        {
            return sceneLoader.activeScene;
        }
    }
}