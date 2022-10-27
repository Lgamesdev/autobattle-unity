using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public delegate void OnItemChangedEvent(List<IBaseCharacterItem> items);
        public OnItemChangedEvent OnItemChanged;

        public static PlayerInventoryManager Instance;

        private Inventory _inventory;

        public List<IBaseCharacterItem> Items => _inventory.Items;
        public int space => _inventory.space;
        

        #region Singleton
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of Inventory found ! ");
                return;
            }

            Instance = this;
        }
        #endregion

        public void SetupManager(Inventory playerInventory)
        {
            _inventory = playerInventory;
        }

        public void AddItem(IBaseCharacterItem item)
        {
            _inventory.AddItem(item);
            
            OnItemChanged?.Invoke(_inventory.Items);
        }

        public void RemoveItem(IBaseCharacterItem item)
        {
            _inventory.RemoveItem(item);

            OnItemChanged?.Invoke(_inventory.Items);
        }
    }
}