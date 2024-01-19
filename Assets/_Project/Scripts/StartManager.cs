using Core.Network;
using Core.Player;
using LGamesDev.Core;
using LGamesDev.Core.Player;
using LGamesDev.Fighting;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    [DefaultExecutionOrder(-9999)]
    public class StartManager : MonoBehaviour
    {
        private static StartManager s_Instance;
        
        
#if UNITY_EDITOR
        //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
        //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
        //so this is set to true when destroyed, so we do not reinstantiate a new one
        private static bool s_IsQuitting = false;
#endif
        public static StartManager Instance 
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying || s_IsQuitting)
                    return null;
                
                if (s_Instance == null)
                {
                    //in editor, we can start any scene to test, so we are not sure the game manager will have been
                    //created by the first scene starting the game. So we load it manually. This check is useless in
                    //player build as the 1st scene will have created the StartManager so it will always exists.
                    Instantiate(Resources.Load<StartManager>("StartManager"));
                }
#endif
                return s_Instance;
            }
        }

        //[SerializeField] private SceneLoader sceneLoader;

        //public NetworkManager networkManager;
        //public NetworkService networkService;
        
        public LoadingScreen loadingScreen;
        public ModalWindowPanel modalWindow;
        public AudioManager audioManager;
        public DialogManager dialogManager;
        public ColorLibrary itemQualityColorLibrary;

        //private Authentication _authentication;
        private PlayerConfig _playerConfig;
        //private const string AuthenticationKey = "authentication";
        private const string PlayerConfKey = "playerConf";
        private PlayerOptions _playerOptions;
        private const string OptionsKey = "options";

        private async void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
            
            //Network Manager
            //networkManager = GetComponent<NetworkManager>();
            //Network Manager
            //networkService = GetComponent<NetworkService>();
            //Audio Manager
            audioManager = GetComponent<AudioManager>();
            //Scene Loader
            //sceneLoader = GetComponent<SceneLoader>();
            //Loading Screen
            //loadingScreen = GameObject.Find("/Canvas/LoadingScreen").GetComponent<LoadingScreen>();
            await loadingScreen.DisableLoadingScreen();
            //Modal Window
            modalWindow.Close();

            //Authentication
            //_authentication = JsonConvert.DeserializeObject<Authentication>(PlayerPrefs.GetString(AuthenticationKey));
            //Debug.Log("player prefs authentication : " + _authentication);
            
            _playerConfig = JsonConvert.DeserializeObject<PlayerConfig>(PlayerPrefs.GetString(PlayerConfKey));
            
            //Player Options
            _playerOptions = JsonConvert.DeserializeObject<PlayerOptions>(PlayerPrefs.GetString(OptionsKey)) ?? new PlayerOptions();
            
            //Loader.Load(Loader.Scene.AuthenticationScene);
        }
        
        /*public struct userAttributes {}
        public struct appAttributes {}

        async Task Start()
        {
            // initialize Unity's authentication and core services, however check for internet connection
            // in order to fail gracefully without throwing exception if connection does not exist
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
        }*/

        private void Start()
        {
            /*if (_playerConfig == null)
            {
                sceneLoader.LoadAuthentication(true, true);
            }
            else
            {
                sceneLoader.LoadAuthentication(true, true);
            }*/
            
            /*audioManager.SetMixerVolume(AudioTrack.Music, _playerOptions.MusicVolume);
            audioManager.SetMixerVolume(AudioTrack.Effects, _playerOptions.EffectsVolume);*/
        }

        public void LoadCustomization()
        {
            //sceneLoader.LoadCustomization();
        }
        
        public void LoadMainMenu()
        {
            //sceneLoader.LoadMainMenu();
        }

        public void PlayMainMenuMusic()
        {
            audioManager.PlayMusic(1f);
        }

        public void LoadFight(Fight fight)
        {
            //sceneLoader.LoadFight(fight);
        }
        
        public void PlayFightMusic()
        {
            audioManager.PlayFightMusic(1f);
        }

        public void Logout()
        {
            //networkManager.Disconnect();
            
            PlayerPrefs.DeleteKey(PlayerConfKey);
            _playerConfig = null;
            
            audioManager.StopMusic();

            //sceneLoader.LoadAuthentication(true, true);
            Loader.Load(Loader.Scene.AuthenticationScene);
        }
        
        public PlayerConfig GetPlayerConf()
        {
            return _playerConfig;
        }
        
        public void SetPlayerConf(PlayerConfig playerConfig)
        {
            if (playerConfig != null)
            {
                _playerConfig = playerConfig;
                PlayerPrefs.SetString(PlayerConfKey, JsonConvert.SerializeObject(_playerConfig));
            }
            else
            {
                Debug.LogError("trying to set player conf to null");
            }
        }

        /*public Authentication GetAuthentication()
        {
            return _authentication;
        }

        public void SetAuthentication(Authentication authentication)
        {
            if (authentication != null)
            {
                _authentication = authentication;
                PlayerPrefs.SetString(AuthenticationKey, JsonConvert.SerializeObject(_authentication));
            }
            else
            {
                Debug.LogError("trying to set authentication to null");
            }
        }*/

        public PlayerOptions GetPlayerOptions()
        {
            return _playerOptions;
        }
        
        public void SetPlayerOptions(PlayerOptions playerOptions)
        {
            if (playerOptions != null)
            {
                _playerOptions = playerOptions;
                PlayerPrefs.SetString(OptionsKey, JsonConvert.SerializeObject(_playerOptions));
            }
            else
            {
                Debug.LogError("trying to set options to null");
            }
        }
    }
}