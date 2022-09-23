using System.Collections;
using System.Collections.Generic;
using Core.Player;
using LGamesDev.Core;
using LGamesDev.Core.Authentication;
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
        private static readonly int End = Animator.StringToHash("End");
        private static readonly int Start1 = Animator.StringToHash("Start");

        private readonly List<AsyncOperation> _scenesLoading = new();

        private Authentication _authentication;

        private RawImage _background;
        private RawImage _backMountains;
        private RawImage _clouds;

        private GameObject _loadingScreen;
        private RawImage _mountains;
        
        private ProgressBar _progressBar;
        private TextMeshProUGUI _progressText;

        private float _totalSceneProgress;
        private float _totalSetupProgress;
        private Animator _transition;
        private RawImage _treesGround;

        private ModalWindowPanel _modalWindow;
        
        private PlayerConfig _playerConfig;

        private void Awake()
        {
            Instance = this;

            //Loading Screen
            _loadingScreen = GameObject.Find("/Canvas/LoadingScreen");
            _transition = GameObject.Find("/Canvas").GetComponent<Animator>();
            _backMountains = _loadingScreen.transform.Find("ParallaxBackground").Find("BackMountains")
                .GetComponent<RawImage>();
            _clouds = _loadingScreen.transform.Find("ParallaxBackground").Find("Clouds")
                .GetComponent<RawImage>();
            _mountains = _loadingScreen.transform.Find("ParallaxBackground").Find("Mountains")
                .GetComponent<RawImage>();
            _treesGround = _loadingScreen.transform.Find("ParallaxBackground").Find("TreesGround")
                .GetComponent<RawImage>();

            _progressBar = _loadingScreen.transform.Find("ProgressBar").GetComponent<ProgressBar>();
            _progressText = _loadingScreen.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();

            //Modal Window
            _modalWindow = GameObject.Find("/Canvas/Modal Window Panel").GetComponent<ModalWindowPanel>();
            _modalWindow.Close();

            //Authentication
            _authentication = JsonConvert.DeserializeObject<Authentication>(PlayerPrefs.GetString("authentication"));

            //Request Error Event
            RequestHandler.OnRequestError += OnRequestError;
        }

        private void OnRequestError(string error)
        {
            _modalWindow.ShowAsTextPopup("error occured", error, "close", "", _modalWindow.Close);
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(EnableLoadingScreen());
            
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Authentication, LoadSceneMode.Additive));
            yield return StartCoroutine(GetSceneLoadProgress());
            yield return StartCoroutine(DisableLoadingScreen());
        }

        public void LoadGame()
        {
            StartCoroutine(LoadGameCoroutine());
        }

        private IEnumerator LoadGameCoroutine()
        {
            yield return StartCoroutine(EnableLoadingScreen());

            // Load / unload scene below to load Main menu
            if(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Authentication).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Authentication));
            if(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Customization).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Customization));
            
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MainMenu, LoadSceneMode.Additive));

            yield return StartCoroutine(GetSceneLoadProgress());
            yield return StartCoroutine(GetTotalProgress());
            
            yield return StartCoroutine(DisableLoadingScreen());
        }

        public void LoadFight(Fight fight)
        {
            StartCoroutine(LoadFightCoroutine(fight));
        }
        
        private IEnumerator LoadFightCoroutine(Fight fight)
        {
            yield return StartCoroutine(EnableLoadingScreen());

            // Load / unload scene below to load Main menu
            if(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));

            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Fight).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Fight));

            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Fight, LoadSceneMode.Additive));

            yield return StartCoroutine(GetSceneLoadProgress());
            
            yield return StartCoroutine(FightManager.Instance.SetupFight(fight));
            
            yield return StartCoroutine(DisableLoadingScreen());
            FightManager.Instance.StartFight();
        }

        public IEnumerator LoadCustomization()
        {
            yield return StartCoroutine(EnableLoadingScreen());

            // Load / unload scene below to load Customization
            _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Customization, LoadSceneMode.Additive));

            yield return StartCoroutine(GetSceneLoadProgress());
            yield return StartCoroutine(DisableLoadingScreen());
        }
        
        public IEnumerator Logout()
        {
            yield return StartCoroutine(EnableLoadingScreen());
            PlayerPrefs.DeleteKey("authentication");
            
            // Load / unload scene below to load Customization
            _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
            
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Authentication, LoadSceneMode.Additive));

            yield return StartCoroutine(GetSceneLoadProgress());
            yield return StartCoroutine(DisableLoadingScreen());
        }

        private IEnumerator GetSceneLoadProgress()
        {
            foreach (var sceneLoading in _scenesLoading)
            {
                while (!sceneLoading.isDone)
                {
                    _totalSceneProgress = 0;

                    foreach (var operation in _scenesLoading) _totalSceneProgress += operation.progress;

                    _totalSceneProgress = _totalSceneProgress / _scenesLoading.Count * 100f;

                    _progressBar.current = Mathf.RoundToInt(_totalSceneProgress);
                    //_progressText.text = Mathf.RoundToInt(_totalSceneProgress) + "%";

                    yield return new WaitForEndOfFrame();
                }
            }

            _scenesLoading.Clear();
            
            yield return new WaitForSeconds(0.5f);
        }
        
        /**
        * For player setup initialisation after loading scenes
        */
        private IEnumerator GetTotalProgress()
        {
            while (Initialisation.Current == null || !Initialisation.Current.isDone)
            {
                if (Initialisation.Current == null)
                {
                    _totalSetupProgress = 0;
                }
                else
                {
                    _totalSetupProgress = Mathf.Round(Initialisation.Current.progress * 100f);

                    _progressText.text = Initialisation.Current.currentStage switch
                    {
                        InitialisationStage.Body => "Loading Body",
                        InitialisationStage.Infos => "Loading Player Infos",
                        InitialisationStage.Progression => "Loading Player Infos",
                        InitialisationStage.Wallet => "Loading Wallet",
                        InitialisationStage.Equipment => "Loading Equipment",
                        InitialisationStage.Inventory => "Loading Inventory",
                        InitialisationStage.Character or InitialisationStage.CharacterStats => "Loading Character",
                        _ => "Loading some things..."
                    };
                }

                var totalProgress = Mathf.Round((_totalSceneProgress + _totalSetupProgress) / 2f);

                _progressBar.current = Mathf.RoundToInt(totalProgress);
                
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator EnableLoadingScreen()
        {
            _loadingScreen.SetActive(true);
            StartCoroutine(ScrollingBackground());

            LTDescr anim = LeanTween.alpha(_loadingScreen.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.linear).setOnComplete(() =>
            {
                LeanTween.alpha(_loadingScreen.GetComponent<RectTransform>(), 1f, 1f).setEase(LeanTweenType.linear);
            });

            yield return new WaitForSeconds(anim.time);
        }

        public IEnumerator DisableLoadingScreen()
        {
            LTDescr anim = LeanTween.alpha(_loadingScreen.GetComponent<RectTransform>(), 1f, 1f).setEase(LeanTweenType.linear).setOnComplete(() =>
            {
                LeanTween.alpha(_loadingScreen.GetComponent<RectTransform>(), 0f, 1f).setEase(LeanTweenType.linear);
            });

            yield return new WaitForSeconds(anim.time);
            
            _loadingScreen.SetActive(false);
        }

        private IEnumerator ScrollingBackground()
        {
            while (_loadingScreen.activeInHierarchy)
            {
                _backMountains.uvRect = new Rect(_backMountains.uvRect.position + new Vector2(0.005f, 0) * Time.deltaTime,
                    _backMountains.uvRect.size);
                _clouds.uvRect = new Rect(_clouds.uvRect.position + new Vector2(0.01f, 0) * Time.deltaTime,
                    _clouds.uvRect.size);
                _mountains.uvRect = new Rect(_mountains.uvRect.position + new Vector2(0.015f, 0) * Time.deltaTime,
                    _mountains.uvRect.size);
                _treesGround.uvRect = new Rect(_treesGround.uvRect.position + new Vector2(0.02f, 0) * Time.deltaTime,
                    _treesGround.uvRect.size);

                yield return new WaitForEndOfFrame();
            }
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