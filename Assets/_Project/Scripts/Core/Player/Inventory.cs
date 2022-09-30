using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Inventory
    {
        public List<IBaseCharacterItem> Items;
        
        public int space = 28;

        public void AddItem(IBaseCharacterItem characterItem)
        {
            if (!characterItem.Item.isDefaultItem)
            {
                if (Items.Count >= space)
                {
                    Debug.Log("Not enough space.");
                    return;
                }

                Items.Add(characterItem);
            }
            else
            {
                var itemAlreadyInInventory = false;
                foreach (var inventoryItem in Items)
                    if (inventoryItem.Item.name == characterItem.Item.name)
                    {
                        inventoryItem.Amount += characterItem.Amount;
                        itemAlreadyInInventory = true;
                    }

                if (!itemAlreadyInInventory)
                {
                    Items.Add(characterItem);
                }
            }
        }

        public void RemoveItem(IBaseCharacterItem item)
        {
            CharacterItem characterItem = item as CharacterItem;
            
            Items.Remove(characterItem);

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

            foreach (IBaseCharacterItem item in Items)
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