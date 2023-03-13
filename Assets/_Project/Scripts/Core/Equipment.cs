using System;
using LGamesDev.Core.Character;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Equipment : Item
    {
        public Stat[] stats;
        
        public EquipmentSlot equipmentSlot;

        public int spriteId;

        public int requiredLevel;

        public int GetStatValue(StatType statType)
        {
            return stats[(int)statType].GetValue();
        }

        public Stat[] GetStats()
        {
            return stats;
        }

        public override string ToString()
        {
            var result = "equipment : [ \n " +
                         base.ToString() +
                         "equipmentSlot : " + equipmentSlot + "\n" +
                         //"spritePath : " + (sprites.Count != 0 ? AssetDatabase.GetAssetPath(sprites[0]) : null) + "\n" +
                         "spriteId : " + spriteId + "\n" +
                         "stats : [ \n";

            if (stats != null) Array.ForEach(stats, stat => result += stat + "\n");

            result += "] \n" + 
                      "]";

            return result;
        }
    }

    public enum EquipmentSlot
    {
        Helmet,
        Chest,
        Pants,
        Weapon
        /*Shield,*/
    }
}