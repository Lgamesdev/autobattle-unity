using System;
using LGamesDev.Fighting;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class CharacterLootBox : BaseCharacterItem<LootBox>
    {
        public Reward Reward;
        
        public override void Use()
        {
            // Use the item
            Debug.Log("Using " + item.name);
            //GameManager.Instance.networkService.TryOpenLootBox(this);
        }

        public override void Sell()
        {
            PlayerWalletManager.Instance.TrySellCharacterItem(this);
            //Debug.Log(item.name + " selled.");
        }

        public override string ToString()
        {
            var result = "characterItem : [ \n " +
                            "id : " + id + "\n" +
                            item + "\n" +
                            "amount : " + amount + "\n" +
                            "reward : " + Reward + "\n" +
                            "]";
            
            return result;
        }
    }
}