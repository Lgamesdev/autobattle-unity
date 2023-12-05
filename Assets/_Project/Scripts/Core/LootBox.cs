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
    public class LootBox : Item
    {
        public new ItemType itemType = ItemType.LootBox;
    }

    public class LootBoxData : ItemData
    {
        //public new ItemType itemType = ItemType.LootBox;
    }
}