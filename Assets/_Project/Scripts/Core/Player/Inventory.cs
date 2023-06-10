using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Inventory
    {
        public List<IBaseCharacterItem> Items = new();
        public int space = 28;

        public void AddItem(IBaseCharacterItem characterItem)
        {
            if (characterItem.Item.itemType == ItemType.Equipment)
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
                foreach (IBaseCharacterItem inventoryItem in Items)
                {
                    //Debug.Log("inventory Item id : " + inventoryItem.Item.ID + " || character item id : " + characterItem.Item.ID);
                    
                    if (inventoryItem.Item.ID != characterItem.Item.ID) continue;
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
            int index;
            
            //if (!Items.Contains(item)) return;
            if (item.Item.itemType is ItemType.Equipment) 
            {
                index = Items.FindIndex(characterItem => characterItem.Id == item.Id); 
            } 
            else 
            {
                index = Items.FindIndex(characterItem => characterItem.Item.ID == item.Item.ID);
            }

            if (index != -1)
            {
                Items[index].Amount--;

                if (Items[index].Amount < 1)
                {
                    Items.RemoveAt(index);
                }
            }
            else
            {
                Debug.LogError(item.Item.name + " not found in inventory while removing");
            }
        }
        
        public override string ToString()
        {
            string result = "inventory : { \n" +
                            " items : [";

            foreach (IBaseCharacterItem item in Items)
            {
                result += item + "\n";
            }

            result += "] \n" +
                      "space : " + space + "\n" +
                      "]}";

            return result;
        }
    }
}