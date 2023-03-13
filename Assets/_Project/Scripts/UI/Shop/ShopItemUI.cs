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

        private Item _item;
        private Action<Item> _onBuy;

        [SerializeField] private Transform header;
        [SerializeField] private Transform content;

        private string _message;

        public void SetupCard(Item item, Action<Item> onBuy)
        {
            _item = item;
            _onBuy = onBuy;

            header.Find("nameText").GetComponent<TextMeshProUGUI>().text = item.name;
            header.Find("itemImage").GetComponent<Image>().sprite = item.icon;
            header.Find("costText").GetComponent<TextMeshProUGUI>().text = AbbreviationUtility.AbbreviateNumber(item.cost);

            content.Find("Item Description").GetComponent<TextMeshProUGUI>().text = "Description of " + item.name;

            if (item.GetType() == typeof(Equipment))
            {
                Equipment equipment = item as Equipment;

                header.Find("levelText").GetComponent<TextMeshProUGUI>().text =
                    "Lvl." + equipment.requiredLevel;

                _message = "Stats \n";

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
            _onBuy?.Invoke(_item);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.Instance.modalWindow.ShowAsPrompt(_item.name, _item.icon, _message);
            //content.gameObject.SetActive(!content.gameObject.activeInHierarchy);
        }
    }
}
