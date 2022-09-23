using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Stat
    {
        public int value;
        
        public StatType statType;
        
        [SerializeField]
        private List<int> modifiers = new List<int>();

        public string GetStatType()
        {
            return statType.ToString();
        }
        
        public void SetStatType(StatType type)
        {
            statType = type;
        }
        
        public int GetValue()
        {
            int finalValue = value;
            modifiers.ForEach(modifier => finalValue += modifier);
            return finalValue;
        }

        public void SetValue(int val)
        {
            value = val;
        }
        
        public void AddModifiers(int modifier)
        {
            if(modifier != 0)
            {
                modifiers.Add(modifier);
            }
        }

        public void RemoveModifier(int modifier)
        {
            if(modifier != 0)
            {
                modifiers.Remove(modifier);
            }
        }

        public override string ToString()
        {
            return "stat : [ \n " +
                   "statType : " + statType + "\n" +
                   "value : " + GetValue() + "\n" +
                   "]";
        }
    }
    
    public enum StatType
    {
        Health,
        Armor,
        Damage,
        Dodge,
        Speed,
        Critical
    }
}