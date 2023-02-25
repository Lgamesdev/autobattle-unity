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

        [SerializeField] private Transform infos;
        [SerializeField] private Transform button;

        public void SetupSlot(Stat stat, Stat modifier = null, bool buttonActive = false)
        {
            _stat = stat;

            infos.Find("label").GetComponent<TextMeshProUGUI>().text = stat.GetStatType();
            infos.Find("values").Find("value").GetComponent<TextMeshProUGUI>().text = stat.GetValue().ToString();
            infos.Find("values").Find("modifier").GetComponent<TextMeshProUGUI>().text = modifier?.GetValue().ToString();

            if (buttonActive)
            {
                button.gameObject.SetActive(CharacterStatsManager.Instance.GetStatPoint() > 0);
                CharacterStatsManager.Instance.StatPointAdded += OnStatPointAdded;
            }
            else
            {
                HideStatPointButton();
            }
        }

        private void OnStatPointAdded()
        {
            button.gameObject.SetActive(CharacterStatsManager.Instance.GetStatPoint() > 0);
        }

        public void ShowStatPointButton()
        {
            button.gameObject.SetActive(true);
        }

        private void HideStatPointButton()
        {
            button.gameObject.SetActive(false);
        }

        public void OnStatPointButton()
        {
            CharacterManager.Instance.statsManager.TryAddStatPoint(_stat.statType);
        }
    }
}