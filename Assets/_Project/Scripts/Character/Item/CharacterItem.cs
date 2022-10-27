using System;
using UnityEngine;

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
            PlayerWalletManager.Instance.SellCharacterItem(this);
            //Debug.Log(item.name + " selled.");
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