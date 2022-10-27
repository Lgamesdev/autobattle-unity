using System.Collections.Generic;
using CodeMonkey.Utils;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] public PlayerCustomer shopCustomer;
        
        [SerializeField] private Transform pfShopCardUI;

        private Transform _containerWrapper;
        private Transform _container;

        private PlayerWalletManager _walletManager;

        [SerializeField] private List<Item> shopItems = new();

        private void Awake()
        {
            _containerWrapper = transform.Find("Container Scroll Rect");
            _container = _containerWrapper.Find("Container");

            _walletManager = PlayerWalletManager.Instance;
        }

        private void Start()
        {
            StartCoroutine(ShopHandler.Load(
                this,
                result =>
                {
                    shopItems = result;
                    SetupUI();
                }
            ));
        }

        private void SetupUI(EquipmentSlot? equipmentSlot = null)
        {
            foreach (Transform child in _container) Destroy(child.gameObject);
            
            if (equipmentSlot.HasValue)
            {
                foreach (Item shopItem in shopItems) 
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
                foreach (Item shopItem in shopItems) 
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