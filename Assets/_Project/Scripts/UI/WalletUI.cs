using System;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class WalletUI : MonoBehaviour
    {
        public Transform goldAmount;
        public Transform crystalAmount;
        
        private IShopCustomer _shopCustomer;

        private PlayerWalletManager _walletManager;

        private void Start()
        {
            _walletManager = PlayerWalletManager.Instance;

            if (_walletManager != null) _walletManager.OnCurrencyChanged += OnCurrencyChanged;
        }

        public void SetShopCustomer(IShopCustomer shopCustomer)
        {
            this._shopCustomer = shopCustomer;
        }

        private void OnCurrencyChanged(Currency currency)
        {
            switch (currency.currencyType)
            {
                case CurrencyType.Gold:
                    goldAmount.GetComponent<TextMeshProUGUI>().text = currency.amount.ToString();
                    break;
                case CurrencyType.Crystal:
                    crystalAmount.GetComponent<TextMeshProUGUI>().text = currency.amount.ToString();
                    break;
            }
        }
    }
}