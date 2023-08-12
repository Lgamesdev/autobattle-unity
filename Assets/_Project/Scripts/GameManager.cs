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
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private SceneLoader sceneLoader;

        public NetworkManager networkManager;
        public NetworkService networkService;
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

        private void Awake()
        {
            Instance = this;
            
            //Network Manager
            networkManager = GetComponent<NetworkManager>();
            //Network Manager
            networkService = GetComponent<NetworkService>();
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
            //_authentication = JsonConvert.DeserializeObject<Authentication>(PlayerPrefs.GetString(AuthenticationKey));
            //Debug.Log("player prefs authentication : " + _authentication);
            
            _playerConfig = JsonConvert.DeserializeObject<PlayerConfig>(PlayerPrefs.GetString(PlayerConfKey));
            
            //Player Options
            _playerOptions = JsonConvert.DeserializeObject<PlayerOptions>(PlayerPrefs.GetString(OptionsKey)) ?? new PlayerOptions();
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
            if (_playerConfig == null)
            {
                StartCoroutine(sceneLoader.LoadAuthentication(true, true));
            }
            else
            {
                StartCoroutine(sceneLoader.LoadAuthentication(true, true));
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
            networkManager.Disconnect();
            
            PlayerPrefs.DeleteKey(PlayerConfKey);
            _playerConfig = null;
            
            audioManager.StopMusic();
            
            StartCoroutine(sceneLoader.LoadAuthentication(true, true));
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

        public SceneIndexes GetActiveScene()
        {
            return sceneLoader.activeScene;
        }
    }
}