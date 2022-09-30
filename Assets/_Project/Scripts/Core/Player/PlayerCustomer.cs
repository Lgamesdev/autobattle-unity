using System;
using LGamesDev.Core.Character;
using LGamesDev.UI;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class PlayerCustomer : MonoBehaviour, IShopCustomer
    {
        [SerializeField] private WalletUI uiWallet;

        private PlayerWalletManager walletManager;

        private void Start()
        {
            //uiWallet.SetShopCustomer(this);

            walletManager = PlayerWalletManager.Instance;
        }

        public int GetGoldAmount()
        {
            return walletManager.GetAmount(CurrencyType.Gold);
        }

        public void BoughtItem(Item item)
        {
            Debug.Log("Request to buy item");
            
            //PlayerInventoryManager.Instance.inventory.AddItem(item);
        }

        public void SellItem(CharacterItem item)
        {
            Debug.Log("Request to sell item");
            
            //item.Sell();
        }

        public bool TrySpendGoldAmount(int spendGoldAmount)
        {
            if (GetGoldAmount() < spendGoldAmount) return false;
            
            walletManager.SpendCurrency(CurrencyType.Gold, spendGoldAmount);
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;

        }

        public event EventHandler OnGoldAmountChanged;
    }
}