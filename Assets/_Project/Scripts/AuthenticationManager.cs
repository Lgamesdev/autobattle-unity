using System;
using System.Threading.Tasks;
using Core.Player;
using LGamesDev.Core.Request;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace LGamesDev
{
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance;
        private StartManager _startManager;
        
        private AuthenticationState _state;
        public Action<AuthenticationState> OnStateUpdate;

        private string _code;
        private string _error;

        private void Awake()
        {
            Instance = this;
            
            InitializeUnityAuthentication();
        }
        
        private async void InitializeUnityAuthentication() {
            try
            {
                // initialize Unity's authentication and core services, however check for internet connection
                // in order to fail gracefully without throwing exception if connection does not exist
                if (Utilities.CheckForInternetConnection())
                {
                    if (UnityServices.State != ServicesInitializationState.Initialized) {
                        //InitializationOptions initializationOptions = new InitializationOptions();
                        //initializationOptions.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());
                        
                        await UnityServices.InitializeAsync(/*initializationOptions*/);
                        
                        SetupAuthenticationEvents();
    
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                        
#if UNITY_ANDROID
                        //Initialize PlayGamesPlatform
                        PlayGamesPlatform.Activate();
#endif
                        try
                        {
#if UNITY_ANDROID
                            TrySignInWithGooglePlayGames();
#endif
#if UNITY_IOS
                            //Initialize Game Center
                            await AuthenticationService.Instance.SignInAnonymouslyAsync();
                            SetState(AuthenticationState.Default);
                            /*if (_startManager.GetAuthentication() != null)
                            {
                                Refresh();
                            }
                            else
                            {
                                SetState(AuthenticationState.Default);
                            }*/
#endif
                            AuthenticationHandler.Connect(result =>
                                {
                                    //_startManager.SetPlayerConf(result);
                                    MenuManager.PlayerConfig = result;
                                    Loader.Load(Loader.Scene.MenuScene);
                                },
                                e =>
                                {
                                    if(e.Message == "No account found for this player")
                                        SetState(AuthenticationState.Register);
                                },
                                e =>
                                {
                                    Debug.Log("error : " + e);
                                    SetState(AuthenticationState.Default);
                                });
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
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Start()
        {
            //SetupAuthentication();
        }

        async Task InitializeRemoteConfigAsync()
        {
            // initialize handlers for unity game services
            await UnityServices.InitializeAsync();

            SetupAuthenticationEvents();
            
            // remote config requires authentication for managing environment information
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        public async void SetupAuthentication()
        {
            SetState(AuthenticationState.Loading);
            
            try
            {
                // initialize Unity's authentication and core services, however check for internet connection
                // in order to fail gracefully without throwing exception if connection does not exist
                if (Utilities.CheckForInternetConnection())
                {
                    await InitializeRemoteConfigAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

#if UNITY_ANDROID
                //Initialize PlayGamesPlatform
                PlayGamesPlatform.Activate();
            #endif

            SetState(AuthenticationState.Loading);
            
            try
            {
                #if UNITY_ANDROID
                    TrySignInWithGooglePlayGames();
                #endif
                #if UNITY_IOS
                    //Initialize Game Center
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    SetState(AuthenticationState.Default);
                    /*if (_startManager.GetAuthentication() != null)
                    {
                        Refresh();
                    }
                    else
                    {
                        SetState(AuthenticationState.Default);
                    }*/
                #endif
                
                AuthenticationHandler.Connect(result =>
                {
                    //_startManager.SetPlayerConf(result);
                    MenuManager.PlayerConfig = result;
                    Loader.Load(Loader.Scene.MenuScene);
                },
                e =>
                {
                    if(e.Message == "No account found for this player")
                        SetState(AuthenticationState.Register);
                },
                e =>
                {
                    Debug.Log("error : " + e);
                    SetState(AuthenticationState.Default);
                });
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
                                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(_code);
                                Debug.Log("SignIn with google is successful.");
                            }
                            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
                            {
                                // Prompt the player with an error message.
                                Debug.LogError("This user is already linked with another account. Log in instead.");
                                /*SetState(AuthenticationState.Register);
                                Register();*/
                                try
                                {
                                    await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(_code);
                                }
                                catch (Exception e)
                                {
                                    Debug.LogException(e);
                                }
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
                            Debug.Log("Google account already connected : " + AuthenticationService.Instance.PlayerInfo);
                        }
                    });
                }
                else
                {
                    _error = "Failed to retrieve Google play games authorization code";
                    Debug.Log("Google Login Unsuccessful");
                    
                    Debug.Log("user id : " + AuthenticationService.Instance.PlayerInfo.Id);
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

        public void Submit(string username = "")
        {
            switch (_state)
            {
                /*case AuthenticationState.Connect:
                    Connect();
                    break;*/
                case AuthenticationState.Register:
                    Register(username);
                    break;
            }
        }

        private void Register(string username)
        {
            AuthenticationHandler.Register(
                username, 
                result =>
                {
                    _startManager.SetPlayerConf(result);
                    Loader.Load(Loader.Scene.MenuScene);
                },
                e =>
                {
                    if(e.Message == "No account found for this player")
                        SetState(AuthenticationState.Register);
                },
                e =>
                {
                    Debug.Log("error : " + e);
                    SetState(AuthenticationState.Default);
                });
        }

        private void Login(string username, string password)
        {
            /*StartCoroutine(AuthenticationHandler.Login(this,
                username, 
                password,
                result =>
                {
                    _startManager.SetAuthentication(result);
                    _startManager.LoadMainMenu();
                }
            ));*/
        }

        private void Refresh()
        {
            /*StartCoroutine(AuthenticationHandler.RefreshToken(
                this,
                _startManager.GetAuthentication().refresh_token,
                result =>
                {
                    _startManager.SetAuthentication(result);
                    _startManager.LoadMainMenu();
                }
            ));*/
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
        Register,
        Connect,
    }
}