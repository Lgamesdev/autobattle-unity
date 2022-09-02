using System;
using LGamesDev.Core.Character;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Equipment : Item
    {
        public Stat[] stats;
        
        public EquipmentSlot equipmentType;
        
        public Sprite sprite;

        public override void Use()
        {
            base.Use();
            //Equip the item 
            CharacterEquipmentManager.Instance.Equip(this);
            RemoveFromInventory();
        }

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
                         "name : " + name + "\n" +
                         "iconPath : " + AssetDatabase.GetAssetPath(icon) + "\n" +
                         "equipmentslot : " + equipmentType + "\n" +
                         "spritePath : " + AssetDatabase.GetAssetPath(sprite) + "\n" +
                         "cost : " + cost + "\n" +
                         "amount : " + amount + "\n" +
                         "stats : [ \n";

            if (stats != null) Array.ForEach(stats, stat => result += stat.ToString() + "\n");

            result += "] \n" + 
                      "]";

            return result;
        }
    }

    public enum EquipmentSlot
    {
        Head,
        Chest,
        Weapon
        /*Shield,*/
    }
}