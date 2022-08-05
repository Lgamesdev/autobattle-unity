using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipment
    {
        public Equipment equipment;

        private Stat[] _modifiers;

        public void Use()
        {
            equipment.Use();
        }

        public int GetStat(StatType statType)
        {
            int baseValue = equipment.GetStatValue(statType);
            int modifierValue = Array.Find(_modifiers, s => s.statType == statType).GetValue();
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
                    Stat modifier = Array.Find(_modifiers, s => s.statType == stat.statType);

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
                         "name : " + equipment.name + "\n" +
                         "equipmentslot : " + equipment.equipmentType + "\n" +
                         "stats : \n";

            if (GetStats() != null) Array.ForEach(GetStats(), stat => result += stat + "\n");

            result += "cost : " + equipment.cost + "\n" +
                      "amount : " + equipment.amount + "\n" +
                      "]";

            return result;
        }
    }
}