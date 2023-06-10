using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Player;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Helper;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev
{
    public class Initialisation : MonoBehaviour
    {
        public static Initialisation Current;

        public float progress;
        public bool isDone;
        public InitialisationStage currentStage;
        private float _finishedStage;

        private readonly Dictionary<InitialisationStage, IEnumerator> _coroutinesLoading = new();

        private GameManager _gameManager;
        
        private readonly Character _character = new();

        private void Awake()
        {
            Current = this;
            _gameManager = GameManager.Instance;
        }

        public void LoadMainMenu()
        {
            isDone = false;
            progress = 0f;

            if (!_gameManager.networkManager.isConnected)
            {
                _gameManager.networkManager.Connect();
            }

            StartCoroutine(SetupInitialisation());
        }

        private IEnumerator SetupInitialisation()
        {
            _finishedStage = 0;
            
            yield return new WaitUntil(() => _gameManager.networkManager.isConnected);

            _gameManager.networkService.SubscribeToMainChannel();
            
            int totalStages = Enum.GetValues(typeof(InitialisationStage)).Length - 1;

            while (_finishedStage < totalStages) 
            {
                yield return new WaitForEndOfFrame();
            }

            currentStage = InitialisationStage.Character;
            yield return StartCoroutine(CharacterManager.Instance.SetupCharacter(_character));

            yield return new WaitForSeconds(0.25f);

            isDone = true;
        }
        
        public void SetResult(InitialisationResult result)
        {
            currentStage = result.Stage;
            
            JsonSerializerSettings settings;
            switch (result.Stage)
            {
                case InitialisationStage.Body:
                    Body playerBody = JsonConvert.DeserializeObject<Body>(result.Value);
                    _character.Body = playerBody;
                    break;
                case InitialisationStage.Progression:
                    PlayerProgression playerProgression = JsonConvert.DeserializeObject<PlayerProgression>(result.Value);
                    _character.level = playerProgression.Level;
                    _character.Experience = playerProgression.Experience;
                    _character.MaxExperience = playerProgression.MaxExperience;
                    _character.StatPoint = playerProgression.StatPoint;
                    _character.Ranking = playerProgression.Ranking;
                    break;
                case InitialisationStage.Wallet:
                    Wallet wallet = JsonConvert.DeserializeObject<Wallet>(result.Value);
                    _character.Wallet = wallet;
                    break;
                case InitialisationStage.Equipment:
                    settings = new JsonSerializerSettings();
                    settings.Converters.Add(new BaseCharacterItemConverter());
                    Gear gear = JsonConvert.DeserializeObject<Gear>(result.Value, settings);
                    _character.Gear = gear;
                    break;
                case InitialisationStage.Inventory:
                    settings = new JsonSerializerSettings();
                    settings.Converters.Add(new BaseCharacterItemConverter());
                    //Debug.Log("inventory : " + result.Value);
                    Inventory inventory = JsonConvert.DeserializeObject<Inventory>(result.Value, settings);
                    _character.Inventory = inventory;
                    break;
                case InitialisationStage.CharacterStats:
                    Stat[] stats = JsonConvert.DeserializeObject<Stat[]>(result.Value);
                    _character.stats = stats;
                    break;
            }

            _finishedStage++;
            progress = (_finishedStage / (Enum.GetValues(typeof(InitialisationStage)).Length - 1)) * 100f;
        }

        private void ShowErrorWindow(string error)
        {
            string text = "error on game loading : \n " + error;
            
            _gameManager.modalWindow.ShowAsTextPopup(
                "Something get wrong...", 
                text, 
                "Retry", 
                "",
                () => {
                    SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
                }
            );
        }
    }

    public enum InitialisationStage
    {
        Body,
        Progression,
        Wallet,
        Equipment,
        Inventory,
        CharacterStats,
        Character
    }
}