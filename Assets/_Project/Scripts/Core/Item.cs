using System;
using System.Runtime.Serialization;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    [Serializable]
    public class Item
    {
        public string? ID = null;
        public string name = "New Item";
        [JsonIgnore]
        public Sprite icon;
        //public bool isDefaultItem = true;
        public int cost = 0;
        public ItemType itemType = ItemType.Item;
        public ItemQuality itemQuality = ItemQuality.Normal;

        public override string ToString()
        {
            return "item : [ \n " +
                   "name : " + name + "\n" +
                   //"isDefaultItem : " + isDefaultItem + "\n" +
                   /*"iconPath : " + AssetDatabase.GetAssetPath(icon) + "\n" +*/
                   "cost : " + cost + "\n" +
                   "itemType : " + itemType + "\n" +
                   "itemQuality : " + itemQuality + "\n" +
                   "]";
        }
    }

    [Serializable]
    public class ItemData
    {
        public string name;
        public string iconPath;
        [JsonIgnore]
        public Sprite icon;
        //public bool isDefaultItem = true;
        public ItemQuality itemQuality = ItemQuality.Normal;
        public ItemType itemType = ItemType.Item;
        public int requiredLevel;
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemType
    {
        [EnumMember(Value = "Item")]
        Item,
        [EnumMember(Value = "LootBox")]
        LootBox,
        [EnumMember(Value = "Equipment")]
        Equipment,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemQuality
    {
        [EnumMember(Value = "Normal")]
        Normal,
        [EnumMember(Value = "Rare")]
        Rare,
        [EnumMember(Value = "Epic")]
        Epic,
        [EnumMember(Value = "Legendary")]
        Legendary
    }
}