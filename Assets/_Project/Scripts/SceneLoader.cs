using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Core;
using LGamesDev.Fighting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class SceneLoader : MonoBehaviour
    {
        private readonly List<AsyncOperation> _scenesLoading = new();
        private float _totalSceneProgress;
        private float _totalSetupProgress;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        public IEnumerator LoadAuthentication(bool loadingScreenEnabled, bool loadingScreenDisabled)
        {
            yield return StartCoroutine(SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load authentication
                    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu).isLoaded)
                        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
                    
                    _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Authentication, LoadSceneMode.Additive));
                },
                loadingScreenEnabled,
                loadingScreenDisabled)
            );

            AuthenticationManager.Instance.SetupAuthentication();
        }

        public IEnumerator LoadMainMenu()
        {
            yield return StartCoroutine(SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load Main menu
                    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Authentication).isLoaded)
                        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Authentication));

                    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Customization).isLoaded)
                        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Customization));

                    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Fight).isLoaded)
                        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Fight));

                    _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MainMenu, LoadSceneMode.Additive));
                },
                true,
                true,
                GetTotalProgress()
            ));

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu));
        }

        public IEnumerator LoadFight(Fight fight)
        {
            yield return StartCoroutine(_gameManager.loadingScreen.EnableLoadingScreen());

            // Load / unload scene below to load Fight
            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
                    
            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Fight).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Fight));
                    
            _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Fight, LoadSceneMode.Additive));
            
            // Get Scene load progress
            yield return StartCoroutine(GetSceneLoadProgress());
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Fight));
            
            // When scene loaded setup the fight
            yield return StartCoroutine(FightManager.Instance.SetupFight(fight));
            
            yield return StartCoroutine(_gameManager.loadingScreen.DisableLoadingScreen());

            FightManager.Instance.StartFight();
        }

        public IEnumerator LoadCustomization()
        {
            yield return StartCoroutine(SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load Customization
                    if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu).isLoaded)
                        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
                    
                    _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Customization, LoadSceneMode.Additive));
                }    
            ));
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Customization));
        }

        private IEnumerator SetupSceneTransition(Action onLoadingScreenFunction, bool loadingScreenEnabled = true, bool loadingScreenDisabled = true, IEnumerator onSceneLoadComplete = null)
        {
            if (loadingScreenEnabled)
            {
                yield return StartCoroutine(_gameManager.loadingScreen.EnableLoadingScreen());
            }

            onLoadingScreenFunction?.Invoke();
            
            yield return StartCoroutine(GetSceneLoadProgress());
            
            if (onSceneLoadComplete != null)
            {
                yield return StartCoroutine(onSceneLoadComplete);

            }

            if (loadingScreenDisabled)
            {
                yield return StartCoroutine(_gameManager.loadingScreen.DisableLoadingScreen());
            }
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

                    _gameManager.loadingScreen.progressBar.progressText.text = "Loading ...";
                    _gameManager.loadingScreen.progressBar.current = Mathf.RoundToInt(_totalSceneProgress);
                    _gameManager.loadingScreen.progressBar.percentageText.text = Mathf.RoundToInt(_totalSceneProgress).ToString() + "%";

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

                    _gameManager.loadingScreen.progressBar.progressText.text = Initialisation.Current.currentStage switch
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

                _gameManager.loadingScreen.progressBar.current = Mathf.RoundToInt(totalProgress);
                _gameManager.loadingScreen.progressBar.percentageText.text = Mathf.RoundToInt(totalProgress).ToString() + "%";

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
