using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class WalletUI : MonoBehaviour
    {
        public Transform goldAmount;
        private Transform _container;
        private IShopCustomer _shopCustomer;

        private PlayerWalletManager _walletManager;

        private void Awake()
        {
            goldAmount = transform.Find("goldAmount");
            _container = transform.Find("container");
        }

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
            goldAmount.GetComponent<TextMeshProUGUI>().text = currency.amount.ToString();
        }
    }
}