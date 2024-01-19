using System;
using System.Collections.Generic;
using System.Text;
using LGamesDev.Core.Character;
using LGamesDev.Core.Request;
using Newtonsoft.Json;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public Stat[] stats;
        private int maxHealth { get; set; }
        private int currentHealth { get; set; }
        
        public delegate void StatsUpdatedEvent(Stat[] stats);
        public StatsUpdatedEvent StatsUpdated;
        
        public delegate void StatPointAddedEvent();
        public StatPointAddedEvent StatPointAdded;
        
        public event EventHandler OnHealthChanged;

        public static CharacterStatsManager Instance;

        private CharacterEquipmentManager _equipmentManager;

        protected void Awake()
        {
            Instance = this;
        }

        public void SetupManager(Stat[] characterStats)
        {
            _equipmentManager = GetComponent<CharacterEquipmentManager>();
            _equipmentManager.EquipmentChanged += OnEquipmentChanged;
            
            stats = characterStats;

            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentGear.equipments) {
                if (characterEquipment != null) {
                    OnEquipmentChanged(characterEquipment, null);
                }
            }

            maxHealth = stats[(int)StatType.Health].GetValue();
            currentHealth = maxHealth;
        }

        public void TryAddStatPoint(StatType statType)
        {
            if (CharacterManager.Instance.Character.StatPoint > 0)
            {
                //StartManager.Instance.networkService.TryAddStatPoint(statType);
            }
            else
            {
                //Todo : you don't have any stat point
            }
        }

        public void AddStatPoint(Stat stat)
        {
            //Debug.Log(stat.value + " points stat added in " + stat.statType);
            CharacterManager.Instance.Character.StatPoint--;
            stats[(int)stat.statType].value += stat.value;
                        
            StatsUpdated?.Invoke(stats);
        }

        private void OnEquipmentChanged(CharacterEquipment newEquipment, CharacterEquipment oldEquipment)
        {
            if (newEquipment?.item?.stats != null)
            {
                foreach (var stat in newEquipment.GetStats())
                {
                    //Debug.Log("stat " + stat.statType + " : " + stats[(int)stat.statType].value + " +" + stat.GetValue());
                    stats[(int)stat.statType].AddModifiers(stat.GetValue());
                }
            }

            if (oldEquipment?.item?.stats != null)
            {
                foreach (var stat in oldEquipment.GetStats())
                {
                    //Debug.Log("stat " + stat.statType + " : " + stats[(int)stat.statType].value + " -" + stat.GetValue());
                    stats[(int)stat.statType].RemoveModifier(stat.GetValue());
                }
            }

            StatsUpdated?.Invoke(stats);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
                
            //Debug.Log(transform.name + " takes " + damage + " damage. \n current health : " + currentHealth);

            if (currentHealth < 0) currentHealth = 0;
            if (currentHealth == 0) Die();
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Heal(int amount)
        {
            currentHealth += amount;
            //Debug.Log(transform.name + " heals " + amount + " hp.");

            if (currentHealth > maxHealth) currentHealth = maxHealth;
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }

        private void Die()
        {
            //Die in some way 
            //Method to override in some way
            //Debug.Log(transform.name + " died.");
        }

        public float GetHealthPercent()
        {
            return (float)currentHealth / (float)maxHealth;
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        
        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetStat(StatType statType)
        {
            return stats[(int)statType].GetValue();
        }

        public int GetStatPoint()
        {
            return CharacterManager.Instance.Character.StatPoint;
        }
    }
}