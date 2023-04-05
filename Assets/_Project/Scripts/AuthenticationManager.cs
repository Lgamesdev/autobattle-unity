using System;
using System.Threading.Tasks;
using GooglePlayGames;
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

        private async void Awake()
        {
            Instance = this;
            _gameManager = GameManager.Instance;
            
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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
            SetState(AuthenticationState.Loading);
            SetupEvents();
            
            await SignInAnonymouslyAsync();
        }

        // Setup authentication event handlers if desired
        void SetupEvents() {
            AuthenticationService.Instance.SignedIn += () => {
                // Shows how to get a playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            };

            AuthenticationService.Instance.SignInFailed += (err) => {
                Debug.LogError(err);
            };

            AuthenticationService.Instance.SignedOut += () => {
                Debug.Log("Player signed out.");
            };
 
            AuthenticationService.Instance.Expired += () =>
            {
                Debug.Log("Player session could not be refreshed and expired.");
            };
        }
        
        private async Task SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");
        
                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 
                
                #if UNITY_ANDROID
                    //Initialize PlayGamesPlatform
                    PlayGamesPlatform.Activate();
                    Social.localUser.Authenticate(ProcessAuthentication);
                #endif

                #if UNITY_IOS
                    //Initialize Game Center
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
        private void ProcessAuthentication(bool success)
        {
            if (success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, async (string code) =>
                {
                    Debug.Log("Authorization code: " + code);

                    _code = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    if (!AuthenticationService.Instance.SessionTokenExists)
                    {
                        await LinkWithGooglePlayGamesAsync(code);
                    }
                    
                    if (_gameManager.GetAuthentication() == null)
                    {
                        Debug.Log("no credentials");
                        SetState(AuthenticationState.PlatformRegister);
                    }
                    else
                    {
                        Debug.Log("credentials exists : " + _gameManager.GetAuthentication().username);
                        PlatformRegister(_gameManager.GetAuthentication().username);
                        Submit();
                    }
                });
            }
            else
            {
                SetState(AuthenticationState.Default);
                _error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
            }
        }

        private async Task LinkWithGooglePlayGamesAsync(string authCode)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
                Debug.Log("Link is successful.");
                SetState(AuthenticationState.PlatformRegister);
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");

                GameManager.Instance.modalWindow.ShowAsTextPopup(
                    "Account already exist",
                    "This user is already linked with another account, do you want to Log in instead ?",
                    "Login",
                    "No",
                    async () =>
                    {
                        await SignInWithGooglePlayGamesAsync(authCode);
                    },
                    GameManager.Instance.modalWindow.Close
                );
                
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
        
        private async Task SignInWithGooglePlayGamesAsync(string authCode)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
                Debug.Log("SignIn is successful.");
                PlatformRegister("");
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

        public void ClickLoginGooglePlayGames()
        {
            Social.localUser.Authenticate(ProcessAuthentication);
        }
#endif

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
            StartCoroutine(AuthenticationHandler.PlatformRegister(this,
                username,
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
        Register,
        Login,
        Refresh
    }
}