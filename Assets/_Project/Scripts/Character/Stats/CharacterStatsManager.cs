using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Request;
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
        
        public delegate void OnStatPointAddedEvent();
        public OnStatPointAddedEvent OnStatPointAdded;
        
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

            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentGear.equipments) {
                if (characterEquipment != null) {
                    OnEquipmentChanged(characterEquipment, null);
                }
            }

            maxHealth = stats[(int)StatType.Health].GetValue();
            currentHealth = maxHealth;
        }

        public void AddPoint(StatType statType)
        {
            if (CharacterManager.Instance.Character.StatPoint > 0)
            {
                StartCoroutine(CharacterStatHandler.AddStatPoint(
                    this,
                    statType,
                    error =>
                    {
                        Debug.Log("error on adding stat point : " + error);
                    },
                    response=>
                    {
                        Debug.Log("point stat successfully added : " + response);
                        CharacterManager.Instance.Character.StatPoint--;

                        stats[(int)statType].value += statType switch
                        {
                            StatType.Health => 10,
                            StatType.Damage => 2,
                            StatType.Armor 
                            or StatType.Dodge
                            or StatType.Speed 
                            or StatType.Critical => 1,
                        };
                        
                        OnStatsUpdated?.Invoke(stats);
                    }
                ));
            }
            else
            {
                //Todo : you don't have any stat point
            }
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