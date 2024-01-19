using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Request;
using UnityEngine;

namespace LGamesDev.UI
{
    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance;

        [SerializeField] private ShopItemUI pfShopCardUI;

        private Transform _containerWrapper;
        private Transform _container;

        private PlayerWalletManager _walletManager;

        private List<ShopPurchase> _shopItems = new();
        
        // Dictionary of all Virtual Purchase transactions ids to lists of costs & rewards.
        /*public Dictionary<string, (List<ItemAndAmountSpec> costs, List<ItemAndAmountSpec> rewards)>
            virtualPurchaseTransactions
        {
            get;
            private set;
        }*/

        public List<ShopPurchase> ShopItems
        {
            get => _shopItems;
            set
            {
                _shopItems = value.OrderBy(i => i.item.cost).ToList();
                SetupUI();
            }
        }

        private void Awake()
        {
            Instance = this;
            
            _containerWrapper = transform.Find("Container Scroll Rect");
            _container = _containerWrapper.Find("Container");

            _walletManager = PlayerWalletManager.Instance;
        }

        private void Start()
        {
            //StartManager.Instance.networkService.GetItemsList();
            ShopHandler.GetPurchase(
                shopList => ShopItems = shopList,
                e => Debug.Log("error on shopList : " + e), 
                e => Debug.Log("error on shopList : " + e)
            );
        }

        private void SetupUI(EquipmentSlot? equipmentSlot = null)
        {
            foreach (Transform child in _container) Destroy(child.gameObject);

            if (equipmentSlot.HasValue)
            {
                foreach (ShopPurchase shopPurchase in _shopItems) 
                {
                    if(shopPurchase.item.GetType() != typeof(Equipment)) continue;
                    Equipment equipment = shopPurchase.item as Equipment;

                    if (equipmentSlot == equipment.equipmentSlot) {
                        CreateItemButton(shopPurchase);
                    }
                }
            }
            else
            {
                foreach (ShopPurchase shopPurchase in _shopItems) 
                {
                    CreateItemButton(shopPurchase);
                }
            }
        }

        private void CreateItemButton(ShopPurchase purchase)
        {
            ShopItemUI shopItem = Instantiate(pfShopCardUI, _container).GetComponent<ShopItemUI>();
            shopItem.SetupCard(purchase, TryBuyItem);
        }

        private void TryBuyItem(ShopPurchase shopPurchase)
        {
            _walletManager.TryBuyItem(shopPurchase);
        }

        public void NoFilter()
        {
            SetupUI();
        }

        public void FilterByHelmet()
        {
            SetupUI(EquipmentSlot.Helmet);
        }
        
        public void FilterByChest()
        {
            SetupUI(EquipmentSlot.Chest);
        }
        
        public void FilterByPants()
        {
            SetupUI(EquipmentSlot.Pants);
        }
        
        public void FilterByWeapon()
        {
            SetupUI(EquipmentSlot.Weapon);
        }
    }
}