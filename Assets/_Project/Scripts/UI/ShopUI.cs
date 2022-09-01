using CodeMonkey.Utils;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
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

        private void Awake()
        {
            _container = transform.Find("container");
            _shopItemTemplate = _container.Find("shopItemTemplate");
            _shopItemTemplate.gameObject.SetActive(false);

            _walletManager = PlayerWalletManager.Instance;
        }

        private void Start()
        {
            //CreateItemButton(ItemAssets.Instance.helmet, 0);
            //CreateItemButton(Item.ItemType.Armor, Item.GetSprite(Item.ItemType.Armor), "Armor", Item.GetCost(Item.ItemType.Armor), 0);
            //CreateItemButton(Item.ItemType.Sword, Item.GetSprite(Item.ItemType.Sword), "Sword", Item.GetCost(Item.ItemType.Sword), 1);
            //CreateItemButton(Item.ItemType.Helmet, Item.GetSprite(Item.ItemType.Helmet), "Helmet", Item.GetCost(Item.ItemType.Helmet), 2);
            /*CreateItemButton(Item.ItemType.Medkit, Item.GetSprite(Item.ItemType.Medkit), "Medicinal kit", Item.GetCost(Item.ItemType.Medkit), 3);
            CreateItemButton(Item.ItemType.ManaPotion, Item.GetSprite(Item.ItemType.ManaPotion), "Mana Potion", Item.GetCost(Item.ItemType.ManaPotion), 4);
            CreateItemButton(Item.ItemType.HealthPotion, Item.GetSprite(Item.ItemType.HealthPotion), "Health Potion", Item.GetCost(Item.ItemType.HealthPotion), 5);*/
            //Hide();
        }

        private void CreateItemButton(Item item, int positionIndex)
        {
            var shopItemTransform = Instantiate(_shopItemTemplate, _container);
            shopItemTransform.gameObject.SetActive(true);
            var shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

            var shopItemHeight = 100f;
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