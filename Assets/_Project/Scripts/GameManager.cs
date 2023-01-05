using System;
using System.Collections;
using System.Collections.Generic;
using Core.Player;
using LGamesDev.Core;
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
        
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private SceneLoader sceneLoader;

        public LoadingScreen loadingScreen;
        public ModalWindowPanel modalWindow;
        
        private Authentication _authentication;
        private PlayerConfig _playerConfig;

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
            _authentication = JsonConvert.DeserializeObject<Authentication>(PlayerPrefs.GetString("authentication"));
        }

        private void Start()
        {
            if (_authentication == null)
            {
                Debug.Log("authentication is null");
                StartCoroutine(sceneLoader.LoadAuthentication(true, true));
            }
            else
            {  
                Debug.Log("credentials find");
                StartCoroutine(sceneLoader.LoadAuthentication(true, false));
            }
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
            audioManager.PlayMainMenuMusic(1.5f);
        }

        public void LoadFight(Fight fight)
        {
            StartCoroutine(sceneLoader.LoadFight(fight));
        }
        
        public void PlayFightMusic()
        {
            audioManager.PlayFightMusic(1.5f);
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
                PlayerPrefs.SetString("authentication", JsonUtility.ToJson(authentication));
            }
            else
            {
                Debug.LogError("trying to set authentication to null");
            }
        }
    }
}