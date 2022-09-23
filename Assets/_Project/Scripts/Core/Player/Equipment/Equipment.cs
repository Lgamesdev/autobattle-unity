using System;
using LGamesDev.Core.Character;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Equipment : Item
    {
        public Stat[] stats;
        
        public EquipmentType equipmentType;

        public int spriteId;

        public override void Use()
        {
            //base.Use();
            //Equip the item 
            Debug.Log("Equip the item");
            //CharacterEquipmentManager.Instance.Equip(this);
            CharacterHandler.Instance.equipmentManager.Equip(this);
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
                         base.ToString() +
                         "equipmentSlot : " + equipmentType + "\n" +
                         //"spritePath : " + (sprites.Count != 0 ? AssetDatabase.GetAssetPath(sprites[0]) : null) + "\n" +
                         "spriteId : " + spriteId + "\n" +
                         "stats : [ \n";

            if (stats != null) Array.ForEach(stats, stat => result += stat.ToString() + "\n");

            result += "] \n" + 
                      "]";

            return result;
        }
    }

    public enum EquipmentType
    {
        Helmet,
        Chest,
        Pants,
        Weapon
        /*Shield,*/
    }
}