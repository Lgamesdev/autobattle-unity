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
        [SerializeField] private StatSlotUI pfUIStatSlot;

        private Transform _itemContainer;

        private Transform _itemSlot;
        private Image _icon;
        private TextMeshProUGUI _itemName;

        private Transform _useButton;
        private Transform _sellButton;
        
        private Transform _statsParent;
        private IBaseCharacterItem _currentCharacterItem;

        private void Awake()
        {
            Instance = this;

            _itemSlot = transform.Find("itemSlot");
            _icon = _itemSlot.Find("icon").GetComponent<Image>();
            _itemName = transform.Find("itemName").GetComponent<TextMeshProUGUI>();

            _useButton = transform.Find("Buttons").Find("useButton");
            _sellButton = transform.Find("Buttons").Find("sellButton");
            
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

        public void Show(IBaseCharacterItem item)
        {
            gameObject.SetActive(true);

            _currentCharacterItem = item;

            _useButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _useButton.GetComponent<Button>().onClick.AddListener(UseItem);
            _sellButton.gameObject.SetActive(true);

            SetupUI(_currentCharacterItem.GetType() == typeof(CharacterEquipment) ? "Equip" : "Use");
        }

        public void ShowEquipped(CharacterEquipment equipment)
        {
            gameObject.SetActive(true);

            _currentCharacterItem = equipment;

            _useButton.GetComponent<Button>().onClick.RemoveAllListeners();
            _useButton.GetComponent<Button>().onClick.AddListener(UnEquipItem);
            _sellButton.gameObject.SetActive(false);

            SetupUI("UnEquip");
        }
        
        private void SetupUI(string useButtonText)
        {
            _useButton.Find("text").GetComponent<TextMeshProUGUI>().text = useButtonText;
            
            foreach (Transform child in _statsParent) Destroy(child.gameObject);

            if (_currentCharacterItem != null && _currentCharacterItem.Item.isDefaultItem == false)
            {
                _icon.sprite = _currentCharacterItem.Item.icon;
                _icon.gameObject.SetActive(true);
                _itemSlot.Find("emptyImage").gameObject.SetActive(false);

                if (_currentCharacterItem.Item.name != null) _itemName.text = _currentCharacterItem.Item.name;

                if (_currentCharacterItem.Item.GetType() == typeof(Equipment))
                {
                    CharacterEquipment characterEquipment = _currentCharacterItem as CharacterEquipment;
                    
                    foreach (Stat stat in characterEquipment.item.GetStats())
                    {
                        StatSlotUI statSlot = Instantiate(pfUIStatSlot, _statsParent);

                        Stat statModifier = characterEquipment.GetModifier(stat.statType);
                        statSlot.SetupSlot(stat, statModifier);
                    }
                }
            }
            else
            {
                _icon.gameObject.SetActive(false);
                _itemSlot.Find("emptyImage").gameObject.SetActive(true);
            }
        }

        private void UseItem()
        {
            _currentCharacterItem.Use();
            Hide();
        }

        private void UnEquipItem()
        {
            CharacterManager.Instance.equipmentManager.UnEquip((int)((Equipment)_currentCharacterItem.Item).equipmentSlot);
            Hide();
        }

        public void SellItem()
        {
            _currentCharacterItem.Sell();
            Hide();
        }
    }
}