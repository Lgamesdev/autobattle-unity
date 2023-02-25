using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Player;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class CharacterEquipment : BaseCharacterItem<Equipment>
    {
        public List<Stat> modifiers;

        public CharacterEquipment()
        {
            modifiers = new List<Stat>();
        }

        public override void Use()
        {
            //TryEquip the item 
            //Debug.Log("TryEquip the item");
            CharacterManager.Instance.equipmentManager.TryEquip(this);
        }

        public override void Sell()
        {
            PlayerWalletManager.Instance.TrySellCharacterItem(this);
            //Debug.Log(item.name + " selled.");
        }

        public Stat GetStat(StatType statType)
        {
            Stat mergedStat = new()
            {
                statType = statType,
                value = item.GetStatValue(statType) 
                        + modifiers.Find(s => s.statType == statType).GetValue()
            };
            return mergedStat;
        }
        
        public List<Stat> GetStats()
        {
            List<Stat> mergedStats = item.GetStats().ToList();

            foreach (Stat stat in GetModifiers())
            {
                int existingStatIndex = mergedStats.FindIndex(s => s.statType == stat.statType);
                
                if (existingStatIndex != -1)
                {
                    mergedStats[existingStatIndex].value += stat.value;
                }
                else
                {
                    mergedStats.Add(stat);
                }
            }

            return mergedStats;
        }

        public Stat GetModifier(StatType statType)
        {
            return modifiers.Find(s => s.statType == statType);
        }

        public List<Stat> GetModifiers()
        {
            List<Stat> mergedStats = new List<Stat>();

            if (modifiers != null){
                foreach (Stat modifier in modifiers)
                {
                    if (!mergedStats.Exists(s => s.statType == modifier.statType))
                    {
                        mergedStats.Add(modifier);
                    }
                }
            }

            return mergedStats;
        }

        public override string ToString()
        {
            var result = "characterEquipment: [ \n " +
                            "\t id : " + id + "\n" +
                            "\t " + item.ToString() + "\n" +
                            "\t modifiers : [ \n";

            modifiers.ForEach(stat => result += "\t " + stat.ToString() + "\n");
            
            result += "\t ] \n" +
                    "]";

            return result;
        }
    }
}