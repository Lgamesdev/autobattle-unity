using System;
using LGamesDev.Core.Player;
using LGamesDev.Helper;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class WalletUI : MonoBehaviour
    {
        public Transform goldAmount;
        public Transform crystalAmount;

        private PlayerWalletManager _walletManager;

        private void Start()
        {
            _walletManager = PlayerWalletManager.Instance;

            if (_walletManager != null) _walletManager.CurrencyChanged += OnCurrencyChanged;
        }

        private void OnCurrencyChanged(Currency currency)
        {
            switch (currency.currencyType)
            {
                case CurrencyType.Gold:
                    goldAmount.GetComponent<TextMeshProUGUI>().text = AbbreviationUtility.AbbreviateNumber(currency.amount);
                    break;
                case CurrencyType.Crystal:
                    crystalAmount.GetComponent<TextMeshProUGUI>().text = AbbreviationUtility.AbbreviateNumber(currency.amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}