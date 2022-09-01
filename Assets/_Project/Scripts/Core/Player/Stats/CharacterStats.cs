using System;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class CharacterStats : MonoBehaviour
    {
        public Stat[] stats;
        protected int maxHealth { get; private set; }
        protected int currentHealth { get; set; }

        protected virtual void Awake()
        {
            var numSlots = Enum.GetNames(typeof(StatType)).Length;

            stats = new Stat[numSlots];

            for (var i = 0; i < numSlots; i++) 
                stats[i] = new Stat { statType = (StatType)i };

            //Mockup
            /*stats[(int)StatType.Health].SetValue(100);
            stats[(int)StatType.Damage].SetValue(20);
            stats[(int)StatType.Critical].SetValue(15);
            stats[(int)StatType.Armor].SetValue(1);
            stats[(int)StatType.Speed].SetValue(2);*/

            maxHealth = stats[(int)StatType.Health].GetValue();
            currentHealth = maxHealth;
        }

        public float GetHealthPercent()
        {
            return (float)currentHealth / maxHealth;
        }

        public int GetStat(StatType statType)
        {
            return stats[(int)statType].GetValue();
        }
    }
}