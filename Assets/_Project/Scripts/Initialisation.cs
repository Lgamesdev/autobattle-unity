using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Player;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Helper;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
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

        public async void LoadMainMenu()
        {
            isDone = false;
            progress = 0f;

            /*if (!_gameManager.networkManager.isConnected)
            {
                _gameManager.networkManager.Connect();
            }*/

            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
            SetupInitialisation();
        }

        private async Task SetupInitialisation()
        {
            _finishedStage = 0;
            
            await GetResult();
            
            //total initialisation stage - character
            int totalStages = Enum.GetValues(typeof(InitialisationStage)).Length - 1;
            
            while (_finishedStage < totalStages)
            {
                await Task.Yield();
            }

            currentStage = InitialisationStage.Character;
            
            await CharacterManager.Instance.SetupCharacter(_character);

            await Task.Delay(250);

            isDone = true;
        }

        private async Task GetResult()
        {
            List<InitialisationResult> results = null;

            /*async void OnSuccess(List<InitialisationResult> r)
            {
                results = r;
            }*/

            await InitialisationHandler.GetInitialisation(
                list =>
                {
                    results = list;
                }, 
                e =>
                {
                    Debug.Log("error on initialisation : " + e);
                },
                e => 
                {
                    Debug.Log("error on initialisation : " + e);
                }
            );
            
            foreach (InitialisationResult result in results)
            {
                currentStage = result.Stage;
                //Debug.Log("current stage : " + currentStage);

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
                        //Player inventory : 
                        try
                        {
                            GetInventoryResult inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
                            settings = new JsonSerializerSettings();
                            settings.Converters.Add(new ItemConverter());
                            Inventory inventory = new Inventory();
                            foreach (PlayersInventoryItem inventoryItem in inventoryResult.PlayersInventoryItems)
                            {
                                //Debug.Log("inventory item data : " + inventoryItem.GetItemDefinition().CustomDataDeserializable.GetAsString());
                                Item item = JsonConvert.DeserializeObject<Item>(
                                    inventoryItem.GetItemDefinition().CustomDataDeserializable.GetAsString(), settings);
                                item.ID = inventoryItem.InventoryItemId;
                                //item.icon = null;

                                IBaseCharacterItem characterItem = item.itemType switch
                                {
                                    ItemType.Item => new CharacterItem(),
                                    ItemType.LootBox => new CharacterLootBox(),
                                    ItemType.Equipment => new CharacterEquipment(),
                                    _ => throw new ArgumentOutOfRangeException()
                                };
                                characterItem.Id = inventoryItem.PlayersInventoryItemId;
                                characterItem.Item = item;

                                inventory.AddItem(characterItem);
                            }

                            _character.Inventory = inventory;
                        }
                        catch (Exception e)
                        {
                            Debug.Log("error while trying to get inventory : " + e.Message);
                        }
                        break;
                    case InitialisationStage.CharacterStats:
                        Stat[] stats = JsonConvert.DeserializeObject<Stat[]>(result.Value);
                        _character.stats = stats;
                        break;
                }
                
                await Task.Yield();
                _finishedStage++;
                progress = (_finishedStage / (Enum.GetValues(typeof(InitialisationStage)).Length - 1)) * 100f;
            }

            //yield return new WaitForEndOfFrame();
        }
        
        /*public void SetResult(InitialisationResult result)
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
                    Debug.Log("inventory : " + result.Value);
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
        }*/

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