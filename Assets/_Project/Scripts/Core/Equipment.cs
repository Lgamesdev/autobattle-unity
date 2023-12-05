using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class Equipment : Item
    {
        //public new ItemType itemType = ItemType.Equipment;
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

    public class EquipmentData : ItemData
    {
        //public new ItemType itemType = ItemType.Equipment;
        public EquipmentSlot EquipmentSlot;
        public int spriteId;
        public Stat[] stats;
    }

    public enum EquipmentSlot
    {
        Helmet,
        Chest,
        Pants,
        Weapon,
        OffHand,
    }

    public enum WeaponType
    {
        Hand = 0,
        Sword = 1,
        Staff = 2,
        Dagger = 3,
        MagicalDagger = 4
    }

    public enum OffHandType
    {
        Hand = 0,
        Shield = 1,
        Book = 2,
        Dagger = 3
    }
}