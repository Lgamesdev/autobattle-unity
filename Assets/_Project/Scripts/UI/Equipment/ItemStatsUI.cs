using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ItemStatsUI : MonoBehaviour
    {
        public static ItemStatsUI Instance;
        [SerializeField] private Transform pfUIStatSlot;

        private Transform _useButton;

        private Image _icon;

        private Transform _itemContainer;

        private TextMeshProUGUI _itemName;

        private Item _itemShowed;

        private Transform _itemSlot;
        private Transform _sellButton;

        private Transform _statsParent;

        private void Awake()
        {
            Instance = this;

            _itemSlot = transform.Find("itemSlot");

            _icon = _itemSlot.Find("icon").GetComponent<Image>();

            _itemName = transform.Find("itemName").GetComponent<TextMeshProUGUI>();

            _useButton = transform.Find("useButton");
            _sellButton = transform.Find("sellButton");

            _statsParent = transform.Find("StatsParent");
        }

        private void Start()
        {
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(Item item)
        {
            gameObject.SetActive(true);

            _itemShowed = item;

            _useButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _useButton.GetComponent<Button>().onClick.AddListener(UseItem);

            SetupUI(_itemShowed.GetType() == typeof(Equipment) ? "Equip" : "Use");
        }

        public void ShowEquipped(Equipment equipment)
        {
            gameObject.SetActive(true);

            _itemShowed = equipment;

            _useButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _useButton.GetComponent<Button>().onClick.AddListener(UnEquipItem);

            SetupUI("UnEquip");
        }
        
        private void SetupUI(string useButtonText)
        {
            _useButton.Find("text").GetComponent<TextMeshProUGUI>().text = useButtonText;
            
            foreach (Transform child in _statsParent) Destroy(child.gameObject);

            if (_itemShowed != null)
            {
                _icon.sprite = _itemShowed.icon;
                _icon.gameObject.SetActive(true);
                _itemSlot.Find("emptyImage").gameObject.SetActive(false);

                if (_itemShowed.name != null) _itemName.text = _itemShowed.name;

                if (_itemShowed.GetType() == typeof(Equipment))
                {
                    foreach (Stat stat in ((Equipment)_itemShowed).GetStats())
                    {
                        RectTransform statSlotRectTransform =
                            Instantiate(pfUIStatSlot, _statsParent).GetComponent<RectTransform>();

                        statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>().text =
                            stat.statType.ToString();
                        statSlotRectTransform.Find("value").GetComponent<TextMeshProUGUI>().text =
                            stat.GetValue().ToString();
                    }
                }
            }
            else
            {
                _icon.gameObject.SetActive(false);
                _itemSlot.Find("emptyImage").gameObject.SetActive(true);
            }
        }

        public void UseItem()
        {
            _itemShowed.Use();
            Hide();
        }

        private void UnEquipItem()
        {
            //CharacterEquipmentManager.Instance.UnEquip((int)((Equipment)_itemShowed).equipmentType);
            CharacterHandler.Instance.equipmentManager.UnEquip((int)((Equipment)_itemShowed).equipmentType);
            Hide();
        }

        public void SellItem()
        {
            _itemShowed.Sell();
            Hide();
        }
    }
}