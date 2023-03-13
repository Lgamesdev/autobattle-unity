using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance;
        private GameManager _gameManager;
        
        private AuthenticationState _state;
        public Action<AuthenticationState> OnStateUpdate;

        private string _token;
        private string _error;

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
                #if UNITY_ANDROID
                    //Initialize PlayGamesPlatform
                    PlayGamesPlatform.Activate();
                    LoginGooglePlayGames();
                #endif
                
                #if UNITY_IOS
                    //Initialize Game Center
                #endif
            }
            else
            {
                Debug.Log("credentials exists : " + _gameManager.GetAuthentication().username);
                SetState(AuthenticationState.Refresh);
                Submit();
            }
        }

        public void Submit(string username = "", string password = "",
            string email = "", string refreshToken = "")
        {
            switch (_state)
            {
                case AuthenticationState.PlatformRegister:
                    Register(username, password, email);
                    break;
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
        
        private void PlatformRegister(string token)
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

        public void SetState(AuthenticationState state)
        {
            _state = state;
            OnStateUpdate?.Invoke(_state);
        }
        
#if UNITY_ANDROID
        private void LoginGooglePlayGames()
        {
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login with Google Play games successful.");

                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        Debug.Log("Authorization code: " + code);
                        _token = code;
                        // This token serves as an example to be used for SignInWithGooglePlayGames
                        SetState(AuthenticationState.PlatformRegister);

                    });
                }
                else
                {
                    _error = "Failed to retrieve Google play games authorization code";
                    Debug.Log("Login Unsuccessful");
                }
            });
        }
#endif
    }
    
    public enum AuthenticationState
    {
        Default,
        PlatformRegister,
        Register,
        Login,
        Refresh
    }
}