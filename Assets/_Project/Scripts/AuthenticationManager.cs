using System;
using System.Collections;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance;

        private GameManager _gameManager;

        private void Awake()
        {
            Instance = this;
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            if (_gameManager == null)
            {
                SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            }
        }

        public void SetupAuthentication()
        {
            if (_gameManager.GetAuthentication() == null)
            {
                Debug.Log("no credentials");
            }
            else
            {
                Debug.Log("credentials exists : " + _gameManager.GetAuthentication().username);
                Submit(AuthenticationState.Refresh);
            }
            
            //yield return new WaitForEndOfFrame();
        }

        public void Submit(AuthenticationState authenticationState, string username = "", string password = "",
            string email = "", string refreshToken = "")
        {
            switch (authenticationState)
            {
                case AuthenticationState.Register:
                    Register(username, password, email);
                    break;
                case AuthenticationState.Login:
                    Login(username, password);
                    break;
                case AuthenticationState.Refresh:
                    Refresh();
                    break;
            }
        }

        private void Register(string username, string password, string email)
        {
            StartCoroutine(AuthenticationHandler.Register(this,
                username, 
                password,
                email,
                result =>
                {
                    _gameManager.SetAuthentication(result);
                    _gameManager.LoadMainMenu();
                }
            ));
        }

        private void Login(string username, string password)
        {
            StartCoroutine(AuthenticationHandler.Login(this,
                username, 
                password,
                result =>
                {
                    _gameManager.SetAuthentication(result);
                    _gameManager.LoadMainMenu();
                }
            ));
        }

        private void Refresh()
        {
            StartCoroutine(AuthenticationHandler.RefreshToken(
                this,
                _gameManager.GetAuthentication().refresh_token,
                result =>
                {
                    _gameManager.SetAuthentication(result);
                    _gameManager.LoadMainMenu();
                }
            ));
        }
    }
}

public enum AuthenticationState
{
    Default,
    Register,
    Login,
    Refresh
}