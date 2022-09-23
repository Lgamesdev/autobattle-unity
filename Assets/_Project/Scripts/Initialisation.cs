using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.UI;
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

        private readonly Dictionary<InitialisationStage, IEnumerator> _coroutinesLoading = new();

        private GameManager _gameManager;

        private void Awake()
        {
            Current = this;
            _gameManager = GameManager.Instance;

            isDone = false;
        }

        private void Start()
        {
            if (_gameManager == null)
            {
                SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            } else {
                LoadMainMenu();
            }
        }

        private void LoadMainMenu()
        {
            Character character = new Character();
            
            // Load coroutines to setup main menu
            _coroutinesLoading.Add(InitialisationStage.Body, PlayerBodyHandler.Load(
                this,
                result =>
                {
                    character.Body = result;
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Infos, PlayerConfigHandler.Load(
                this,
                result =>
                {
                    _gameManager.SetPlayerConfig(result);

                    if (!result.creationDone) StartCoroutine(_gameManager.LoadCustomization());
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Progression, PlayerProgressionHandler.Load(
                this,
                result =>
                {
                    character.Level = result.level;
                    character.Experience = result.xp;
                    character.Ranking = result.ranking;

                    InfosUI infosUI = FindObjectOfType<InfosUI>();
                    
                    infosUI.username.GetComponent<TextMeshProUGUI>().text
                        = GameManager.Instance.GetAuthentication().user;
                    
                    infosUI.level.GetComponent<TextMeshProUGUI>().text 
                        = result.level.ToString();
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Wallet, PlayerWalletHandler.Load(
                this,
                result =>
                {
                    character.Wallet = result;
                    
                    WalletUI walletUI = FindObjectOfType<WalletUI>();
                    
                    walletUI.goldAmount.GetComponent<TextMeshProUGUI>().text 
                     = result.GetAmount(CurrencyType.Gold).ToString();
                    
                    walletUI.crystalAmount.GetComponent<TextMeshProUGUI>().text
                        = result.GetAmount(CurrencyType.Crystal).ToString();
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Equipment, CharacterEquipmentHandler.LoadEquipments(
                this,
                result =>
                {
                    //foreach (CharacterEquipment characterEquipment in result) Debug.Log(characterEquipment.ToString());
                    
                    character.Equipments = result;
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Inventory, PlayerInventoryHandler.Load(
                this,
                result =>
                {
                    //Debug.Log(result.ToString());
                    character.Inventory = result;
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.CharacterStats, CharacterStatHandler.LoadStats(
                this,
                result =>
                {
                    //Debug.Log(result.ToString());
                    character.Stats = result;
                }
            ));
            
            //TODO : change dictionnary to another collection for multiple coroutine on 1 InitialisationStage
            
            _coroutinesLoading.Add(InitialisationStage.Character, CharacterHandler.Instance.SetupCharacter(character));
            
            StartCoroutine(SetupCoroutines());
        }

        private IEnumerator SetupCoroutines()
        {
            for (var i = 0; i < _coroutinesLoading.Count; i++)
            {
                currentStage = _coroutinesLoading.ElementAt(i).Key;

                yield return StartCoroutine(_coroutinesLoading.ElementAt(i).Value);

                progress += i / _coroutinesLoading.Count * 100f;

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.7f);
            
            isDone = true;
        }
    }

    public enum InitialisationStage
    {
        Body,
        Infos,
        Progression,
        Wallet,
        Equipment,
        Inventory,
        CharacterStats,
        Character
    }
}