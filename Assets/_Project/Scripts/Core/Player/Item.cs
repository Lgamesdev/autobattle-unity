using System;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class Item
    {
        public string name = "New Item";

        [JsonConverter(typeof(EquipmentConverter))]
        public Sprite icon;

        public bool isDefaultItem;

        public int cost;

        public int amount = 1;

        public virtual void Use()
        {
            // Use the item
            //Debug.Log("Using " + name);
        }

        public virtual void Sell()
        {
            Debug.Log(name + " selled.");
            //TODO : sell item
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, (int)(cost * 0.25));
            RemoveFromInventory();
        }

        public void RemoveFromInventory()
        {
            PlayerInventoryManager.Instance.Inventory.RemoveItem(this);
        }

        public override string ToString()
        {
            return "[ \n " +
                   "name : " + name + "\n" +
                   "cost : " + cost + "\n" +
                   "amount : " + amount + "\n" +
                   "]";
        }
    }
}