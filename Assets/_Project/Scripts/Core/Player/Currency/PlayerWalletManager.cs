using System;
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
        
        public delegate void OnCurrencyChangedEvent(Currency currency);
        public OnCurrencyChangedEvent OnCurrencyChanged;

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
            
            PlayerWalletHandler.SetManager(this);
        }

        public int GetAmount(CurrencyType currencyType)
        {
            return _wallet.GetAmount(currencyType);
        }

        public void SpendCurrency(CurrencyType currencyType, int amount)
        {
            _wallet.SpendCurrency(currencyType, amount);

            OnCurrencyChanged?.Invoke(_wallet.GetCurrency(currencyType));
        }

        public void AddCurrency(CurrencyType currencyType, int amount)
        {
            _wallet.AddCurrency(currencyType, amount);

            OnCurrencyChanged?.Invoke(_wallet.GetCurrency(currencyType));
        }
    }
}