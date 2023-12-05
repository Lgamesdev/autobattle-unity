using System;
using System.Threading.Tasks;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Request.Converters;
using LGamesDev.UI;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace LGamesDev
{
    public class PlayerWalletManager: MonoBehaviour
    {
        const int k_EconomyPurchaseCostsNotMetStatusCode = 10504;
        
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
        
        public async void TryBuyItem(ShopPurchase shopPurchase)
        {
            try
            {
                //var result = await MakeVirtualPurchaseAsync(shopPurchase.ID);
                ShopHandler.BuyItem(shopPurchase.ID, async () =>
                    {
                        try
                        {
                            GetInventoryResult inventoryResult = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();
                            JsonSerializerSettings settings = new JsonSerializerSettings();
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

                            PlayerInventoryManager.Instance.SetInventory(inventory);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("error while trying to get inventory : " + e.Message);
                        }
                    }, 
                    null,
                    null
                );
                if (this == null) return;

                await RefreshCurrencyBalances();
                if (this == null) return;

                //ShowRewardPopup(result.Rewards);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                Debug.LogError(e.Message);
                //virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private async Task<MakeVirtualPurchaseResult> MakeVirtualPurchaseAsync(string virtualPurchaseId)
        {
            try
            {
                return await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(virtualPurchaseId);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                // Rethrow purchase-cost-not-met exception to be handled by shops manager.
                throw;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default;
            }
        }
        
        private async Task RefreshCurrencyBalances()
        {
            GetBalancesResult balanceResult = null;

            try
            {
                balanceResult = await GetEconomyBalances();
            }
            catch (EconomyRateLimitedException e)
            {
                balanceResult = await Utils.RetryEconomyFunction(GetEconomyBalances, e.RetryAfter);
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
            }

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null)
                return;

            foreach (PlayerBalance balance in balanceResult.Balances)
            {
                CurrencyType currencyType = Enum.Parse<CurrencyType>(balance.CurrencyId, true);
                CurrencyChanged?.Invoke(new Currency()
                {
                    currencyType = currencyType,
                    amount = (int)balance.Balance
                });
            }
            //currencyHudView.SetBalances(balanceResult);
        }
        
        private static Task<GetBalancesResult> GetEconomyBalances()
        {
            var options = new GetBalancesOptions { ItemsPerFetch = 100 };
            return EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
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

        public void SellCharacterItem(string id)
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