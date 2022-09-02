using System;
using System.Collections.Generic;
using LGamesDev.Core.Player;
using LGamesDev.Request.Converters;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class CharacterEquipment
    {
        public Equipment equipment;

        private List<Stat> _modifiers;

        public CharacterEquipment()
        {
            _modifiers = new List<Stat>();
        }

        public void Use()
        {
            equipment.Use();
        }

        public int GetStat(StatType statType)
        {
            int baseValue = equipment.GetStatValue(statType);
            int modifierValue = _modifiers.Find(s => s.statType == statType).GetValue();
            return baseValue + modifierValue;
        }

        public Stat[] GetStats()
        {
            List<Stat> mergedStats = new List<Stat>();

            foreach (Stat stat in equipment.GetStats())
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
                            equipment.ToString() + "\n" +
                         "modifiers : [ \n";

            _modifiers.ForEach(stat => result += stat.ToString() + "\n");
            
            result += "] \n" +
                    "]";

            return result;
        }
    }
}