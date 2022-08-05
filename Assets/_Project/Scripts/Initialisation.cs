using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.UI;
using TMPro;
using UnityEngine;

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

            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            // Load coroutines to setup main menu
            _coroutinesLoading.Add(InitialisationStage.Body, PlayerBody.Load(
                this,
                result => { _gameManager.SetPlayerBody(result); }
            ));
            _coroutinesLoading.Add(InitialisationStage.Infos, PlayerConfigHandler.Load(
                this,
                result =>
                {
                    _gameManager.SetPlayerConfig(result);

                    if (!result.creationDone) StartCoroutine(_gameManager.LoadCustomization());
                }
            ));
            /*_coroutinesLoading.Add(InitialisationStage.Infos, PlayerInfosHandler.Load(
                this,
                result =>
                {
                    _gameManager.SetPlayerInfos(result);
                }
            ));*/
            _coroutinesLoading.Add(InitialisationStage.Wallet, PlayerWalletHandler.Load(
                this,
                result =>
                {
                    PlayerWalletManager walletManager = PlayerWalletManager.Instance;

                    walletManager.SetCurrencies(result);
                    FindObjectOfType<WalletUI>().GetComponent<WalletUI>().goldAmount.GetComponent<TextMeshProUGUI>().text 
                        = walletManager.GetAmount(CurrencyType.Gold).ToString();
                    FindObjectOfType<WalletUI>().GetComponent<WalletUI>().crystalAmount.GetComponent<TextMeshProUGUI>().text 
                        = walletManager.GetAmount(CurrencyType.Crystal).ToString();
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Equipment, CharacterEquipmentHandler.LoadEquipments(
                this,
                result =>
                {
                    CharacterEquipmentManager.Instance.currentEquipment = result;
                }
            ));
            _coroutinesLoading.Add(InitialisationStage.Inventory, PlayerInventoryHandler.Load(
                this,
                result =>
                {
                    PlayerInventoryManager.Instance.Inventory = result;
                }
            ));
            
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
        Wallet,
        Equipment,
        Inventory
    }
}