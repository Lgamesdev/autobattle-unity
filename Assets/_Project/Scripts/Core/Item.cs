using System;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class Item
    {
        public int? ID = null;
        public string name = "New Item";
        [JsonIgnore]
        public Sprite icon;
        public bool isDefaultItem = true;
        public int cost = 0;
        
        public override string ToString()
        {
            return "item : [ \n " +
                   "name : " + name + "\n" +
                   "isDefaultItem : " + isDefaultItem + "\n" +
                   /*"iconPath : " + AssetDatabase.GetAssetPath(icon) + "\n" +*/
                   "cost : " + cost + "\n" +
                   "]";
        }
    }
}