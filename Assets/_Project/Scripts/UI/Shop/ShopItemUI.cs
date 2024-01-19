using System;
using LGamesDev.Core.Player;
using LGamesDev.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ShopItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform pfUIStatSlot;

        private ShopPurchase purchase;
        private Action<ShopPurchase> _onBuy;

        [SerializeField] private Transform header;
        [SerializeField] private Transform content;

        private string _message;

        public void SetupCard(ShopPurchase purchase, Action<ShopPurchase> onBuy)
        {
            this.purchase = purchase;
            _onBuy = onBuy;

            header.Find("nameText").GetComponent<TextMeshProUGUI>().text = purchase.item.name;
            header.Find("Item Panel").GetComponent<Image>().color =
                StartManager.Instance.itemQualityColorLibrary.colors[(int)purchase.item.itemQuality];
            header.Find("Item Panel/Item Image").GetComponent<Image>().sprite = purchase.item.icon;
            header.Find("costText").GetComponent<TextMeshProUGUI>().text = AbbreviationUtility.AbbreviateNumber(purchase.item.cost);

            content.Find("Item Description").GetComponent<TextMeshProUGUI>().text = "Description of " + purchase.item.name;

            if (purchase.GetType() == typeof(Equipment))
            {
                Equipment equipment = purchase.item as Equipment;

                header.Find("levelText").GetComponent<TextMeshProUGUI>().text =
                    "Lvl." + equipment.requiredLevel;

                _message = "stats \n";

                foreach (Stat stat in equipment.GetStats())
                {
                    _message += stat.GetStatType() + " : " + stat.GetValue() + "\n";
                }
            }
            else
            {
                header.Find("levelText").gameObject.SetActive(false);
            }

            content.gameObject.SetActive(false);
        }

        public void OnBuyButton()
        {
            _onBuy?.Invoke(purchase);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartManager.Instance.modalWindow.ShowAsPrompt(purchase.item.name, purchase.item.icon, _message);
            //content.gameObject.SetActive(!content.gameObject.activeInHierarchy);
        }
    }
}
