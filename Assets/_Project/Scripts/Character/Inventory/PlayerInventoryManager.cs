using System.Collections.Generic;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class PlayerInventoryManager : MonoBehaviour
    {
        public delegate void OnItemChangedEvent(List<Item> items);
        public OnItemChangedEvent OnItemChanged;

        public static PlayerInventoryManager Instance;

        public Inventory Inventory;

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

        private void Start()
        {
            //items = PlayerInventoryHandler.Load_Static();

            //Mockup
            /*Inventory.AddItem(ItemAssets.Instance.armor);
            Inventory.AddItem(ItemAssets.Instance.sword);
            Inventory.AddItem(ItemAssets.Instance.helmet);
            Inventory.AddItem(ItemAssets.Instance.healthPotion);*/
        }

        public void AddItem(Item item)
        {
            Inventory.AddItem(item);
            
            OnItemChanged?.Invoke(Inventory.Items);
        }

        public void RemoveItem(Item item)
        {
            Inventory.RemoveItem(item);

            OnItemChanged?.Invoke(Inventory.Items);
        }
    }
}