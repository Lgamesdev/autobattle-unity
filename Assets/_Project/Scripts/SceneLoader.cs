using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Loader.Scene activeScene;
        
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        public async void LoadAuthentication(bool loadingScreenEnabled, bool loadingScreenDisabled)
        {
            await SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load authentication
                    //UnloadAdditiveScenes();
                    
                    _scenesLoading.Add(SceneManager.LoadSceneAsync(Loader.Scene.AuthenticationScene.ToString(), LoadSceneMode.Additive));
                },
                loadingScreenEnabled,
                loadingScreenDisabled
            );

            AuthenticationManager.Instance.SetupAuthentication();
            activeScene = Loader.Scene.AuthenticationScene;
        }

        public async void LoadMainMenu()
        {
            await SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load Main menu
                    //UnloadAdditiveScenes();

                    _scenesLoading.Add(SceneManager.LoadSceneAsync(Loader.Scene.MenuScene.ToString(), LoadSceneMode.Additive));
                },
                true,
                true,
                GetTotalProgress()
            );

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Loader.Scene.MenuScene.ToString()));
            activeScene = Loader.Scene.MenuScene;
            
            MainMenuManager.Instance.HandleTutorial();
        }

        public async void LoadFight(Fight fight)
        {
            await _gameManager.loadingScreen.EnableLoadingScreen();

            // Load / unload scene below to load Fight
            //UnloadAdditiveScenes();
                    
            _scenesLoading.Add(SceneManager.LoadSceneAsync(Loader.Scene.CustomizationScene.ToString(), LoadSceneMode.Additive));
            
            // Get Scene load progress
            await GetSceneLoadProgress();
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Loader.Scene.GameScene.ToString()));
            
            // When scene loaded setup the fight
            await FightManager.Instance.SetupFight(fight);
            
            await _gameManager.loadingScreen.DisableLoadingScreen();

            FightManager.Instance.StartFight();
            activeScene = Loader.Scene.GameScene;
        }

        public async void LoadCustomization()
        {
            await SetupSceneTransition(
                () =>
                {
                    // Load / unload scene below to load Customization
                    //UnloadAdditiveScenes();
                    
                    _scenesLoading.Add(SceneManager.LoadSceneAsync(Loader.Scene.CustomizationScene.ToString(), LoadSceneMode.Additive));
                }    
            );
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Loader.Scene.CustomizationScene.ToString()));
            activeScene = Loader.Scene.CustomizationScene;//SceneIndexes.Customization;
            
            CustomizationManager.Instance.HandleTutorial();
        }

        private async Task SetupSceneTransition(Action onLoadingScreenFunction, bool loadingScreenEnabled = true, bool loadingScreenDisabled = true, Task onSceneLoadComplete = null)
        {
            if (loadingScreenEnabled)
            {
                await _gameManager.loadingScreen.EnableLoadingScreen();
            }

            onLoadingScreenFunction?.Invoke();
            
            await GetSceneLoadProgress();
            
            if (onSceneLoadComplete != null)
            {
                await onSceneLoadComplete;
            }

            if (loadingScreenDisabled)
            {
                await _gameManager.loadingScreen.DisableLoadingScreen();
            }
        }

        private async Task GetSceneLoadProgress()
        {
            foreach (var sceneLoading in _scenesLoading)
            {
                while (!sceneLoading.isDone)
                {
                    _totalSceneProgress = 0;

                    foreach (var operation in _scenesLoading) _totalSceneProgress += operation.progress;

                    _totalSceneProgress = _totalSceneProgress / _scenesLoading.Count * 100f;

                    _gameManager.loadingScreen.progressBar.progressText.text = "Loading";
                    _gameManager.loadingScreen.progressBar.current = Mathf.RoundToInt(_totalSceneProgress);
                    _gameManager.loadingScreen.progressBar.percentageText.text = Mathf.RoundToInt(_totalSceneProgress).ToString() + "%";

                    await Task.Yield();
                }
            }

            _scenesLoading.Clear();

            await Task.Delay(250);
        }

        /**
        * For player setup initialisation after loading scenes
        */
        private async Task GetTotalProgress()
        {
            while (Initialisation.Current == null || !Initialisation.Current.isDone)
            {
                if (Initialisation.Current == null)
                {
                    _totalSetupProgress = 0;
                }
                else
                {
                    _totalSetupProgress = Initialisation.Current.progress;

                    _gameManager.loadingScreen.progressBar.progressText.text = Initialisation.Current.currentStage switch
                    {
                        InitialisationStage.Body => "Loading Body",
                        InitialisationStage.Progression => "Loading Player Infos",
                        InitialisationStage.Wallet => "Loading Wallet",
                        InitialisationStage.Equipment => "Loading Equipment",
                        InitialisationStage.Inventory => "Loading Inventory",
                        InitialisationStage.Character or InitialisationStage.CharacterStats => "Loading Character",
                        _ => "Loading some things"
                    };
                }

                int totalProgress = Mathf.RoundToInt((_totalSceneProgress + _totalSetupProgress) / 2f);

                _gameManager.loadingScreen.progressBar.current = Mathf.RoundToInt(totalProgress);
                _gameManager.loadingScreen.progressBar.percentageText.text = Mathf.RoundToInt(totalProgress) + "%";

                await Task.Yield();
            }

            await Task.Delay(500);
        }

        /*private void UnloadAdditiveScenes()
        {
            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MainMenu).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MainMenu));
                    
            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Authentication).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Authentication));
                    
            if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.Fight).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Fight));
                    
            if (SceneManager.GetSceneByBuildIndex(Loader.Scene.Customization).isLoaded)
                _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Customization));
        }*/
    }
}
