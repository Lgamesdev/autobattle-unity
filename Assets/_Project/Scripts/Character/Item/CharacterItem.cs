using System;
using System.Collections.Generic;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class CharacterItem : BaseCharacterItem<Item>
    {
        public override void Use()
        {
            // Use the item
            Debug.Log("Using " + item.name);
        }

        public override void Sell()
        {
            Debug.Log(item.name + " selled.");
            //TODO : sell item
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, (int)(item.cost * 0.25));
        }

        public override string ToString()
        {
            var result = "characterItem : [ \n " +
                            "id : " + id + "\n" +
                            item.ToString() + "\n" +
                            "amount : " + amount + "\n" +
                            "]";
            
            return result;
        }
    }
}