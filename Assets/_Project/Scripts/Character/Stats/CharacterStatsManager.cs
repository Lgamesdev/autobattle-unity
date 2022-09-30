using System;
using LGamesDev.Core.Character;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public Stat[] stats;
        private int maxHealth { get; set; }
        private int currentHealth { get; set; }
        
        public delegate void OnStatsUpdatedEvent(Stat[] stats);
        public OnStatsUpdatedEvent OnStatsUpdated;
        
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
            _equipmentManager.OnEquipmentChanged += OnEquipmentChanged;
            
            stats = characterStats;

            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentEquipment) {
                if (characterEquipment.item != null) {
                    OnEquipmentChanged(characterEquipment, null);
                }
            }

            maxHealth = stats[(int)StatType.Health].GetValue();
            currentHealth = maxHealth;
        }

        private void OnEquipmentChanged(CharacterEquipment newEquipment, CharacterEquipment oldEquipment)
        {
            if (newEquipment?.item?.stats != null)
            {
                foreach (var stat in newEquipment.GetStats())
                {
                    //Debug.Log("stat : " + stat.GetType() + " +" + stat.GetValue());
                    stats[(int)stat.statType].AddModifiers(stat.GetValue());
                }
            }

            if (oldEquipment?.item?.stats != null)
            {
                foreach (var stat in oldEquipment.GetStats())
                {
                    //Debug.Log("stat : " + stat.GetType() + " -" + stat.GetValue());
                    stats[(int)stat.statType].RemoveModifier(stat.GetValue());
                }
            }

            OnStatsUpdated?.Invoke(stats);
        }

        public void TakeDamage(int damage)
        {
            /*damage -= stats[(int)StatType.Armor].GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);*/

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
            return (float) currentHealth / maxHealth;
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
    }
}