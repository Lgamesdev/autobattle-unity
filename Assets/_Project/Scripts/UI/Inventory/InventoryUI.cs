using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform pfUIInventorySlot;

        private PlayerInventoryManager _inventoryManager;

        private Transform _itemsParent;

        private InventorySlotUI[] _slots;

        private void Awake()
        {
            _itemsParent = transform.Find("ItemsParent");
        }

        private void Start()
        {
            _inventoryManager = PlayerInventoryManager.Instance;
            _inventoryManager.OnItemChanged += Inventory_OnItemChanged;
            
            _slots = new InventorySlotUI[_inventoryManager.space];
            
            SetupInventoryUI();
        }

        private void Inventory_OnItemChanged(List<IBaseCharacterItem> items)
        {
            UpdateInventoryUI();
        }

        private void UpdateInventoryUI()
        {
            for (var i = 0; i < _slots.Length; i++)
                if (i < _inventoryManager.Items.Count)
                    _slots[i].AddItem(_inventoryManager.Items[i]);
                else
                    _slots[i].ClearSlot();
        }

        private void SetupInventoryUI()
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                var itemSlotRectTransform = Instantiate(pfUIInventorySlot, _itemsParent).GetComponent<RectTransform>();

                var slot = itemSlotRectTransform.GetComponent<InventorySlotUI>();

                _slots[i] = slot;
            }

            UpdateInventoryUI();
        }
    }
}