using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ItemStatsUI : MonoBehaviour
    {
        private static ItemStatsUI _instance;
        [SerializeField] private Transform pfUIStatSlot;

        private Transform _equipButton;

        private Image _icon;

        private Transform _itemContainer;

        private TextMeshProUGUI _itemName;

        private Item _itemShowed;

        private Transform _itemSlot;
        private Transform _sellButton;

        private Transform _statsParent;

        private void Awake()
        {
            _instance = this;

            _itemSlot = transform.Find("itemSlot");

            _icon = _itemSlot.Find("icon").GetComponent<Image>();

            _itemName = transform.Find("itemName").GetComponent<TextMeshProUGUI>();

            _equipButton = transform.Find("equipButton");
            _sellButton = transform.Find("sellButton");

            _statsParent = transform.Find("StatsParent");

            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show(Item item)
        {
            gameObject.SetActive(true);

            _itemShowed = item;

            _equipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _equipButton.GetComponent<Button>().onClick.AddListener(UseItem);

            foreach (Transform child in _statsParent) Destroy(child.gameObject);

            if (_itemShowed != null)
            {
                /*Transform uiItemTransform = Instantiate(pfUI_ItemImage, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlot.GetComponent<RectTransform>().anchoredPosition;
            Image uiItemImage = uiItemTransform.GetComponent<Image>();*/
                _icon.sprite = _itemShowed.icon;
                _icon.gameObject.SetActive(true);
                _itemSlot.Find("emptyImage").gameObject.SetActive(false);

                if (_itemShowed.name != null) _itemName.text = _itemShowed.name;

                if (_itemShowed.GetType() == typeof(Equipment))
                {
                    _equipButton.Find("text").GetComponent<TextMeshProUGUI>().text = "Equip";

                    foreach (var stat in ((Equipment)_itemShowed).GetStats())
                    {
                        var statSlotRectTransform = Instantiate(pfUIStatSlot, _statsParent).GetComponent<RectTransform>();

                        var label = statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>();
                        var value = statSlotRectTransform.Find("value").GetComponent<TextMeshProUGUI>();

                        //label.text = stat.type.ToString();
                        value.text = stat.GetValue().ToString();
                    }
                }
                else
                {
                    _equipButton.Find("text").GetComponent<TextMeshProUGUI>().text = "Use";
                }
            }
            else
            {
                _icon.gameObject.SetActive(false);
                _itemSlot.Find("emptyImage").gameObject.SetActive(true);
            }
        }

        private void ShowEquiped(Item item)
        {
            gameObject.SetActive(true);

            _itemShowed = item;

            _equipButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _equipButton.GetComponent<Button>().onClick.AddListener(UnEquipItem);

            if (_itemShowed != null)
            {
                /*Transform uiItemTransform = Instantiate(pfUI_ItemImage, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlot.GetComponent<RectTransform>().anchoredPosition;
            Image uiItemImage = uiItemTransform.GetComponent<Image>();
            uiItemImage.sprite = itemShowed.icon;*/
                _icon.sprite = _itemShowed.icon;
                _icon.gameObject.SetActive(true);
                _itemSlot.Find("emptyImage").gameObject.SetActive(false);

                if (_itemShowed.name != null) _itemName.text = _itemShowed.name;

                _equipButton.Find("text").GetComponent<TextMeshProUGUI>().text = "UnEquip";

                foreach (Transform child in _statsParent) Destroy(child.gameObject);

                foreach (var stat in ((Equipment)_itemShowed).GetStats())
                {
                    var statSlotRectTransform = Instantiate(pfUIStatSlot, _statsParent).GetComponent<RectTransform>();

                    var label = statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>();
                    var value = statSlotRectTransform.Find("value").GetComponent<TextMeshProUGUI>();

                    label.text = stat.GetStatType();
                    value.text = stat.GetValue().ToString();
                }
            }
            else
            {
                _icon.gameObject.SetActive(false);
                _itemSlot.Find("emptyImage").gameObject.SetActive(true);
            }
        }

        public static void Show_Static(Item item)
        {
            _instance.Show(item);
        }

        public static void ShowEquiped_Static(Equipment equipment)
        {
            _instance.ShowEquiped(equipment);
        }

        public void UnEquipItem()
        {
            CharacterEquipmentManager.Instance.Unequip((int)((Equipment)_itemShowed).equipmentType);
            Hide();
        }

        public void SellItem()
        {
            _itemShowed.Sell();
            Hide();
        }

        public void UseItem()
        {
            _itemShowed.Use();
            Hide();
        }
    }
}