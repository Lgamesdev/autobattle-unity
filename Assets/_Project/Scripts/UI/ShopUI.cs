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

        private Transform _container;

        private Transform _goldAmount;
        private Transform _shopItemTemplate;

        private PlayerWalletManager _walletManager;

        [SerializeField] private List<Item> shopItems = new();

        private void Awake()
        {
            _container = transform.Find("container");
            _shopItemTemplate = _container.Find("shopItemTemplate");
            _shopItemTemplate.gameObject.SetActive(false);

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

        private void SetupUI()
        {
            for (int i = 0; i < shopItems.Count; i++) {
                CreateItemButton(shopItems[i], i);
            }
        }

        private void CreateItemButton(Item item, int positionIndex)
        {
            var shopItemTransform = Instantiate(_shopItemTemplate, _container);
            shopItemTransform.gameObject.SetActive(true);
            var shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

            var shopItemHeight = 120f;
            shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

            shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = item.name;
            shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().text = item.cost.ToString();

            shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = item.icon;

            shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //Clicked on shop item button
                TryBuyItem(item);
            };
        }

        private void TryBuyItem(Item item)
        {
            if (shopCustomer.TrySpendGoldAmount(item.cost))
                //Can afford cost
                shopCustomer.BoughtItem(item);
            else
                Tooltip_Warning.ShowTooltip_Static("Cannot afford " + item.cost + "gold !");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}