using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.UI;
using TMPro;
using UnityEngine;

namespace LGamesDev
{
    public class PlayerWalletManager: MonoBehaviour
    {
        public static PlayerWalletManager Instance;
        
        public delegate void CurrencyChangedEvent(Currency currency);
        public CurrencyChangedEvent CurrencyChanged;

        private Wallet _wallet = new();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of WalletManager found ! ");
                return;
            }

            Instance = this;
        }

        public void SetupManager(Wallet wallet)
        {
            _wallet = wallet;
            CurrencyChanged?.Invoke(_wallet.GetCurrency(CurrencyType.Gold));
            CurrencyChanged?.Invoke(_wallet.GetCurrency(CurrencyType.Crystal));
        }

        public int GetAmount(CurrencyType currencyType)
        {
            return _wallet.GetAmount(currencyType);
        }
        
        public void TryBuyItem(Item item)
        {
            if (GetAmount(CurrencyType.Gold) >= item.cost)
            {
                GameManager.Instance.networkService.TryBuyItem(item);
                
                //Can afford cost
                /*StartCoroutine(PlayerWalletHandler.Buy(
                    this,
                    item,
                    error =>
                    {
                        Debug.Log("error on trying to buy item : " + error);
                    },
                    baseCharacterItem =>
                    {
                        SpendCurrency(CurrencyType.Gold, item.cost);
                        PlayerInventoryManager.Instance.AddItem(baseCharacterItem);
                    }
                ));*/
            }
            else
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot afford " + item.cost + " gold !");
            }
        }

        public void BuyItem(IBaseCharacterItem characterItem)
        {
            SpendCurrency(CurrencyType.Gold, characterItem.Item.cost);
            PlayerInventoryManager.Instance.AddItem(characterItem);
        }
        
        public void TrySellCharacterItem(IBaseCharacterItem characterItem)
        {
            GameManager.Instance.networkService.TrySellItem(characterItem);
            /*StartCoroutine(PlayerWalletHandler.Sell(
                this,
                characterItem,
                error =>
                {
                    Debug.Log("error on item sell : " + error);
                },
                result =>
                {
                    AddCurrency(CurrencyType.Gold, characterItem.Item.cost);
                    PlayerInventoryManager.Instance.RemoveItem(characterItem);
                }
            ));*/
        }

        public void SellCharacterItem(int id)
        {
            IBaseCharacterItem characterItem = CharacterManager.Instance.inventoryManager.GetItemById(id);
            
            AddCurrency(CurrencyType.Gold, characterItem.Item.cost);
            PlayerInventoryManager.Instance.RemoveItem(characterItem);
        }

        private void SpendCurrency(CurrencyType currencyType, int amount)
        {
            _wallet.SpendCurrency(currencyType, amount);
            CurrencyChanged?.Invoke(_wallet.GetCurrency(currencyType));
        }

        public void AddCurrency(CurrencyType currencyType, int amount)
        {
            _wallet.AddCurrency(currencyType, amount);
            CurrencyChanged?.Invoke(_wallet.GetCurrency(currencyType));
        }
    }
}