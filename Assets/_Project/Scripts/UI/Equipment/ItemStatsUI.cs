using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ItemStatsUI : MonoBehaviour
    {
        public static ItemStatsUI Instance;
        [SerializeField] private StatSlotUI pfUIStatSlot;

        private Transform _itemContainer;

        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemLevel;
        [SerializeField] private Transform itemImage;
        private Image _icon;
        [SerializeField] private Transform statsParent;
        
        [SerializeField] private Button useButton;
        [SerializeField] private Button sellButton;

        private IBaseCharacterItem _currentCharacterItem;

        private void Awake()
        {
            Instance = this;
            _icon = itemImage.Find("icon").GetComponent<Image>();
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

            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(UseItem);
            sellButton.gameObject.SetActive(true);

            SetupUI(_currentCharacterItem.Item.itemType switch
            {
                ItemType.Item => "Use",
                ItemType.LootBox => "Open",
                ItemType.Equipment => "Equip",
                _ => "Use",
            });
        }

        public void ShowEquipped(CharacterEquipment equipment)
        {
            gameObject.SetActive(true);

            _currentCharacterItem = equipment;

            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(UnEquipItem);
            sellButton.gameObject.SetActive(false);

            SetupUI("Un equip");
        }
        
        private void SetupUI(string useButtonText)
        {
            useButton.transform.Find("text").GetComponent<TextMeshProUGUI>().text = useButtonText;
            
            foreach (Transform child in statsParent) Destroy(child.gameObject);

            if (_currentCharacterItem != null)
            {
                _icon.sprite = _currentCharacterItem.Item.icon;
                _icon.gameObject.SetActive(true);
                itemImage.Find("emptyImage").gameObject.SetActive(false);
                
                if (_currentCharacterItem.Item.name != null) itemName.text = _currentCharacterItem.Item.name;

                if (_currentCharacterItem.Item.itemType != ItemType.Equipment
                    && _currentCharacterItem.Item.GetType() != typeof(Equipment)) return;
                if (_currentCharacterItem is not CharacterEquipment characterEquipment) return;
                itemLevel.text = "Lvl." + characterEquipment.item.requiredLevel;

                foreach (Stat stat in characterEquipment.item.GetStats())
                {
                    StatSlotUI statSlot = Instantiate(pfUIStatSlot, statsParent);

                    Stat statModifier = characterEquipment.GetModifier(stat.statType);
                    statSlot.SetupSlot(stat, statModifier);
                }
            }
            else
            {
                _icon.gameObject.SetActive(false);
                itemImage.Find("emptyImage").gameObject.SetActive(true);
            }
        }

        private void UseItem()
        {
            _currentCharacterItem.Use();
            Hide();
        }

        private void UnEquipItem()
        {
            CharacterManager.Instance.equipmentManager.TryUnEquip((int)((Equipment)_currentCharacterItem.Item).equipmentSlot);
            Hide();
        }

        public void SellItem()
        {
            _currentCharacterItem.Sell();
            Hide();
        }
    }
}