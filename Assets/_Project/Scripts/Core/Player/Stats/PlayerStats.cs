using System;
using LGamesDev.Core.Character;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class PlayerStats : CharacterStats
    {
        public delegate void OnStatsUpdated(Stat[] stats);

        public static PlayerStats instance;

        private CharacterEquipmentManager _equipmentManager;
        public OnStatsUpdated onStatsUpdated;

        protected override void Awake()
        {
            base.Awake();

            currentHealth = stats[(int)StatType.Health].GetValue();

            instance = this;
        }

        private void Start()
        {
            _equipmentManager = CharacterEquipmentManager.Instance;

            _equipmentManager.OnEquipmentChanged += OnEquipmentChanged;

            SetupStats();
        }

        public event EventHandler OnHealthChanged;

        private void SetupStats()
        {
            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentEquipment)
                if (characterEquipment.equipment != null)
                    OnEquipmentChanged(characterEquipment.equipment, null);
        }

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                foreach (var stat in newItem.GetStats())
                {
                    //Debug.Log("stat : " + stat.GetType() + " +" + stat.GetValue());
                    //stats[(int)stat.type].AddModifiers(stat.GetValue());
                }
            }

            if (oldItem != null)
            {
                foreach (var stat in oldItem.GetStats())
                {
                    //Debug.Log("stat : " + stat.GetType() + " -" + stat.GetValue());
                    //stats[(int)stat.type].RemoveModifier(stat.GetValue());
                }
            }

            onStatsUpdated?.Invoke(stats);
        }

        public void TakeDamage(int damage)
        {
            damage -= stats[(int)StatType.Armor].GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentHealth -= damage;
            //Debug.Log(transform.name + " takes " + damage + " damage.");

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
            if (currentHealth <= 0)
                return true;
            return false;
        }

        public virtual void Die()
        {
            //Die in some way 
            //Method to override in some way
            //Debug.Log(transform.name + " died.");
        }
    }
}