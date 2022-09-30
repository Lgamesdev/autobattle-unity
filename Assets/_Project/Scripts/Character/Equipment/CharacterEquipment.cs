using System;
using System.Collections.Generic;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class CharacterEquipment : BaseCharacterItem<Equipment>
    {
        private List<Stat> _modifiers;

        public CharacterEquipment()
        {
            _modifiers = new List<Stat>();
        }

        public override void Use()
        {
            //Equip the item 
            //Debug.Log("Equip the item");
            CharacterHandler.Instance.equipmentManager.Equip(this);
        }

        public override void Sell()
        {
            Debug.Log(item.name + " selled.");
            //TODO : sell item
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, (int)(item.cost * 0.25));
        }

        public int GetStat(StatType statType)
        {
            int baseValue = item.GetStatValue(statType);
            int modifierValue = _modifiers.Find(s => s.statType == statType).GetValue();
            return baseValue + modifierValue;
        }

        public Stat[] GetStats()
        {
            List<Stat> mergedStats = new List<Stat>();

            foreach (Stat stat in item.GetStats())
            {
                Stat mergedStat = new Stat() { statType = stat.statType };

                int baseValue = stat.GetValue();

                if (_modifiers != null)
                {
                    Stat modifier = _modifiers.Find(s => s.statType == stat.statType);

                    if (modifier != null)
                    {
                        mergedStat.SetValue(baseValue + modifier.GetValue());
                    }
                }
                else
                {
                    mergedStat.SetValue(baseValue);
                }

                mergedStats.Add(mergedStat);
            }

            if (_modifiers != null){
                foreach (Stat modifier in _modifiers)
                {
                    if (!mergedStats.Exists(s => s.statType == modifier.statType))
                    {
                        mergedStats.Add(modifier);
                    }
                }
            }

            return mergedStats.ToArray();
        }

        public override string ToString()
        {
            var result = "characterEquipment: [ \n " +
                            "id : " + id + "\n" +
                            item.ToString() + "\n" +
                            "modifiers : [ \n";

            _modifiers.ForEach(stat => result += stat.ToString() + "\n");
            
            result += "] \n" +
                    "]";

            return result;
        }
    }
}