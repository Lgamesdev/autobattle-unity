using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class ShopPurchase
    {
        public string? ID = null;
        public Item item;

        public override string ToString()
        {
            return "[ \n " + 
                   "id : " + ID + "\n " + 
                   "item : " + item + "\n " + 
                   "]";
        }
    }
}