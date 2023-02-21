using System.Collections.Generic;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;

namespace LGamesDev.UI
{
    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance;
        
        [SerializeField] private Transform pfShopCardUI;

        private Transform _containerWrapper;
        private Transform _container;

        private PlayerWalletManager _walletManager;

        private List<Item> _shopItems = new();

        public List<Item> ShopItems
        {
            get => _shopItems;
            set
            {
                _shopItems = value;
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
            GameManager.Instance.networkManager.GetItemsList();
            /*StartCoroutine(ShopHandler.Load(
                this,
                result =>
                {
                    _shopItems = result;
                    SetupUI();
                }
            ));*/
        }

        private void SetupUI(EquipmentSlot? equipmentSlot = null)
        {
            foreach (Transform child in _container) Destroy(child.gameObject);
            
            if (equipmentSlot.HasValue)
            {
                foreach (Item shopItem in _shopItems) 
                {
                    if(shopItem.GetType() != typeof(Equipment)) continue;
                    Equipment equipment = shopItem as Equipment;

                    if (equipmentSlot == equipment.equipmentSlot) {
                        CreateItemButton(equipment);
                    }
                }
            }
            else
            {
                foreach (Item shopItem in _shopItems) 
                {
                    CreateItemButton(shopItem);
                }
            }
        }

        private void CreateItemButton(Item item)
        {
            ShopItemUI shopItem = Instantiate(pfShopCardUI, _container).GetComponent<ShopItemUI>();
            shopItem.SetupCard(item, TryBuyItem);
        }

        private void TryBuyItem(Item item)
        {
            _walletManager.TryBuyItem(item);
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