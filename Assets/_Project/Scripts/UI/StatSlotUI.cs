using System;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class StatSlotUI : MonoBehaviour
    {
        private Stat _stat;

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private TextMeshProUGUI modifier;
        [SerializeField] private GameObject statPoint;

        public void SetupSlot(Stat stat, Stat statModifier = null, bool buttonActive = false)
        {
            _stat = stat;

            label.text = stat.GetStatType();
            value.text = stat.GetValue().ToString();
            modifier.text = statModifier?.GetValue().ToString();

            if (buttonActive && statPoint != null)
            {
                statPoint.SetActive(CharacterStatsManager.Instance.GetStatPoint() > 0);
                CharacterStatsManager.Instance.StatPointAdded += OnStatPointAdded;

                statPoint.GetComponentInChildren<TextMeshProUGUI>().text = (stat.statType) switch
                {
                    StatType.Health => "+10",
                    StatType.Strength => "+2",
                    StatType.Agility => "+1",
                    StatType.Intelligence => "+1",
                    StatType.Luck => "+1",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                if (statPoint != null)
                {
                    HideStatPointButton();
                }
            }
        }

        private void OnStatPointAdded()
        {
            statPoint.SetActive(CharacterStatsManager.Instance.GetStatPoint() > 0);
        }

        public void ShowStatPointButton()
        {
            statPoint.SetActive(true);
        }

        private void HideStatPointButton()
        {
            statPoint.SetActive(false);
        }

        public void OnStatPointButton()
        {
            CharacterManager.Instance.statsManager.TryAddStatPoint(_stat.statType);
        }
    }
}