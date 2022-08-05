using System;
using System.Collections.Generic;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class Inventory
    {
        public List<Item> Items;
        
        public int Space = 28;

        public void AddItem(Item item)
        {
            if (!item.isDefaultItem)
            {
                if (Items.Count >= Space)
                {
                    Debug.Log("Not enough room.");
                    return;
                }

                Items.Add(item);
            }
            else
            {
                var itemAlreadyInInventory = false;
                foreach (var inventoryItem in Items)
                    if (inventoryItem.name == item.name)
                    {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }

                if (!itemAlreadyInInventory)
                {
                    Items.Add(item);
                }
            }
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);

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

            foreach (Item item in Items)
            {
                result += item + "\n";
            }

            result += "] \n" +
                      "space : " + Space + "\n" +
                      "]}";

            return result;
        }
    }
}