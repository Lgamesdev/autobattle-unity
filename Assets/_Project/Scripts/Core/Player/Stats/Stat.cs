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
            return value;
        }

        public void SetValue(int val)
        {
            this.value = val;
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
        Speed,
        Critical
    }
}