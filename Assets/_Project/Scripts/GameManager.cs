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
        
        [SerializeField] private SceneLoader sceneLoader;
        public LoadingScreen loadingScreen;
        public ModalWindowPanel modalWindow;
        
        private Authentication _authentication;
        private PlayerConfig _playerConfig;

        private void Awake()
        {
            Instance = this;
            
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
            StartCoroutine(sceneLoader.LoadAuthentication());
        }

        public void LoadCustomization()
        {
            StartCoroutine(sceneLoader.LoadCustomization());
        }
        
        public void LoadMainMenu()
        {
            StartCoroutine(sceneLoader.LoadMainMenu());
        }

        public void LoadFight(Fight fight)
        {
            StartCoroutine(sceneLoader.LoadFight(fight));
        }

        public void Logout()
        {
            StartCoroutine(sceneLoader.LoadAuthentication());
            PlayerPrefs.DeleteKey("authentication");
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

        public PlayerConfig GetPlayerConfig()
        {
            return _playerConfig;
        }

        public void SetPlayerConfig(PlayerConfig playerConfig)
        {
            if (playerConfig != null)
            {
                _playerConfig = playerConfig;
            } else {
                Debug.LogError("trying to set playerConf to null");
            }
        }
    }
}