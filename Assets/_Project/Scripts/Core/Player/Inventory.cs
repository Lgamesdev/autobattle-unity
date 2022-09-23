using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Inventory
    {
        public List<Item> items;
        
        public int space = 28;

        public void AddItem(Item item)
        {
            if (!item.isDefaultItem)
            {
                if (items.Count >= space)
                {
                    Debug.Log("Not enough space.");
                    return;
                }

                items.Add(item);
            }
            else
            {
                var itemAlreadyInInventory = false;
                foreach (var inventoryItem in items)
                    if (inventoryItem.name == item.name)
                    {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }

                if (!itemAlreadyInInventory)
                {
                    items.Add(item);
                }
            }
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);

            /*if (item.IsStackable())
            {
                Item itemInInventory = null;
                foreach (Item inventoryItem in itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        //inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }
                */ /*if (itemInInventory != null && itemInInventory.amount <= 0)
                {
                    GetInventorySlotWithItem(itemInInventory).RemoveItem();
                    itemList.Remove(itemInInventory);
                }*/ /*
            }
            else
            {
                GetInventorySlotWithItem(item).RemoveItem();
                itemList.Remove(item);
            }*/
        }
        
        public override string ToString()
        {
            string result = "inventory : { \n" +
                            " items : [";

            foreach (Item item in items)
            {
                result += item.ToString() + "\n";
            }

            result += "] \n" +
                      "space : " + space + "\n" +
                      "]}";

            return result;
        }
    }
}