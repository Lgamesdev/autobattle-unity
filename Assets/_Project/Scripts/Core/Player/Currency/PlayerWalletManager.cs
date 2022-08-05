using System;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class PlayerWalletManager: MonoBehaviour
    {
        public static PlayerWalletManager Instance;
        
        public delegate void OnCurrencyChangedEvent(Currency currency);
        public OnCurrencyChangedEvent OnCurrencyChanged;
        public delegate void OnCurrencyGainedEvent(Currency currency);
        public OnCurrencyGainedEvent OnCurrencyGained;

        public Wallet Wallet;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of WalletManager found ! ");
                return;
            }

            Instance = this;
            
            Wallet = new Wallet();
        }

        public int GetAmount(CurrencyType currencyType)
        {
            return Wallet.GetAmount(currencyType);
        }

        public void SpendCurrency(CurrencyType currencyType, int amount)
        {
            Wallet.SpendCurrency(currencyType, amount);

            OnCurrencyChanged?.Invoke(Wallet.GetCurrency(currencyType));
            StartCoroutine(PlayerWalletHandler.SaveWallet(this, Wallet.GetCurrency(currencyType)));
        }

        public void AddCurrency(CurrencyType currencyType, int amount)
        {
            Wallet.AddCurrency(currencyType, amount);

            OnCurrencyChanged?.Invoke(Wallet.GetCurrency(currencyType));
            StartCoroutine(PlayerWalletHandler.SaveWallet(this, Wallet.GetCurrency(currencyType)));
            //onCurrencyGained?.Invoke(new Currency() { type = currencyType, amount = amount });
        }

        public void SetCurrencies(Currency[] currencies)
        {
            Wallet.Currencies = currencies;
        }
    }
}