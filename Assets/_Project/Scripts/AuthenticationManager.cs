using System;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using LGamesDev.Core.Request;
using Unity.Services.Authentication;
using Unity.Services.Core;
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

        private string _code;
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

        public async void SetupAuthentication()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            SetupAuthenticationEvents();
            
            #if UNITY_ANDROID
                //Initialize PlayGamesPlatform
                PlayGamesPlatform.Activate();
            #endif

            SetState(AuthenticationState.Loading);
            
            try
            {
                #if UNITY_ANDROID
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    TrySignInWithGooglePlayGames();
                #endif
                #if UNITY_IOS
                    //Initialize Game Center
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (_gameManager.GetAuthentication() != null)
                    {
                        Refresh();
                    }
                    else
                    {
                        SetState(AuthenticationState.Default);
                    }
                #endif
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
        
#if UNITY_ANDROID
        public void TrySignInWithGooglePlayGames()
        {
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login with Google Play games successful.");

                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, async code =>
                    {
                        _code = code;
                        // This token serves as an example to be used for SignInWithGooglePlayGames

                        if (!AuthenticationService.Instance.SessionTokenExists)
                        {
                            try
                            {
                                await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(_code);
                                SetState(AuthenticationState.PlatformRegister);
                                Debug.Log("SignIn with google is successful.");
                            }
                            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
                            {
                                // Prompt the player with an error message.
                                Debug.LogError("This user is already linked with another account. Log in instead.");
                                SetState(AuthenticationState.PlatformConnect);
                                PlatformConnect();
                            }
                            catch (RequestFailedException ex)
                            {
                                // Compare error code to CommonErrorCodes
                                // Notify the player with the proper error message
                                Debug.LogException(ex);
                            }
                        }
                        else
                        {
                            SetState(AuthenticationState.PlatformConnect);
                            PlatformConnect();
                        }
                    });
                }
                else
                {
                    _error = "Failed to retrieve Google play games authorization code";
                    Debug.Log("Google Login Unsuccessful");

                    if (_gameManager.GetAuthentication() != null)
                    {
                        Refresh();
                    }
                    else
                    {
                        SetState(AuthenticationState.Default);
                    }
                }
            });
        }
#endif
        private void SetupAuthenticationEvents()
        {
            // Setup authentication event handlers if desired
            AuthenticationService.Instance.SignedIn += () => {
                // Shows how to get a playerID
                //Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                //Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += (err) => {
                Debug.Log("Sign in failed : " + err);
                SetState(AuthenticationState.Default);
            };

            AuthenticationService.Instance.SignedOut += () => {
                Debug.Log("Player signed out.");
            };
 
            AuthenticationService.Instance.Expired += () =>
            {
                Debug.Log("Player session could not be refreshed and expired.");
            };
        }

        public void Submit(string username = "", string password = "",
            string email = "", string refreshToken = "")
        {
            switch (_state)
            {
                case AuthenticationState.PlatformRegister:
                    PlatformRegister(username);
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

      
        private void PlatformRegister(string username)
        {
            #if UNITY_ANDROID  
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    _code = code;
                });
            #endif
            
            StartCoroutine(AuthenticationHandler.PlatformConnect(this,
                username,
                _code,
                result =>
                {
                    _gameManager.SetAuthentication(result);
                    _gameManager.LoadMainMenu();
                }
            ));
        }

        private void PlatformConnect()
        {
            #if UNITY_ANDROID
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    _code = code;
                });
            #endif
            
            StartCoroutine(AuthenticationHandler.PlatformConnect(this,
                null,
                _code,
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
    }
    
    public enum AuthenticationState
    {
        Loading,
        Default,
        PlatformRegister,
        PlatformConnect,
        Register,
        Login,
        Refresh
    }
}