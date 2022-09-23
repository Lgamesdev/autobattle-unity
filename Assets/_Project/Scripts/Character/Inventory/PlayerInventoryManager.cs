using System.Collections.Generic;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public delegate void OnItemChangedEvent(List<Item> items);
        public OnItemChangedEvent OnItemChanged;

        public static PlayerInventoryManager Instance;

        public Inventory inventory;

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
            inventory = playerInventory;
        }

        public void AddItem(Item item)
        {
            inventory.AddItem(item);
            
            OnItemChanged?.Invoke(inventory.items);
        }

        public void RemoveItem(Item item)
        {
            inventory.RemoveItem(item);

            OnItemChanged?.Invoke(inventory.items);
        }
    }
}