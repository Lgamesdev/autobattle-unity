
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class CharacterStatsUI : MonoBehaviour
    {
        [SerializeField] private StatSlotUI pfUIStatSlot;

        private CharacterStatsManager _characterStatsManager;

        private TextMeshProUGUI _statPointText;
        
        private Transform _defensiveStats;
        private Transform _offensiveStats;

        private void Awake()
        {
            _statPointText = transform.Find("Title").Find("statPoint").GetComponent<TextMeshProUGUI>();
            _defensiveStats = transform.Find("statsParent").Find("Defensive Stats");
            _offensiveStats = transform.Find("statsParent").Find("Offensive Stats");
        }

        private void Start()
        {
            _characterStatsManager = CharacterStatsManager.Instance;

            _characterStatsManager.StatsUpdated += OnStatsUpdated;

            OnStatsUpdated(_characterStatsManager.stats);
        }

        private void OnStatsUpdated(Stat[] stats)
        {
            foreach (Transform child in _defensiveStats) Destroy(child.gameObject);
            foreach (Transform child in _offensiveStats) Destroy(child.gameObject);

            if (_characterStatsManager.GetStatPoint() > 0)
            {
                _statPointText.gameObject.SetActive(true);
                _statPointText.text = "(" + _characterStatsManager.GetStatPoint() + " point(s) available)";
            }
            else
            {
                _statPointText.gameObject.SetActive(false);
            }
            
            foreach (var stat in stats)
            {
                switch (stat.statType)
                {
                    case StatType.Health 
                        or StatType.Armor:
                        if (!(stat.statType.Equals(StatType.Armor)
                            && stat.value <= 0))
                        {
                            CreateStatSlot(stat, _defensiveStats);
                        }
                        break;
                    case StatType.Strength
                        or StatType.Agility
                        or StatType.Intelligence
                        or StatType.Luck:
                        CreateStatSlot(stat, _offensiveStats);
                        break;
                }
            }
        }

        private void CreateStatSlot(Stat stat, Transform parent)
        {
            StatSlotUI statSlot = Instantiate(pfUIStatSlot, parent);
            switch (stat.statType)
            {
                case StatType.Armor:
                    statSlot.SetupSlot(stat);
                    break;
                case StatType.Health
                    or StatType.Strength
                    or StatType.Agility
                    or StatType.Intelligence
                    or StatType.Luck:
                    statSlot.SetupSlot(stat, null, true);
                    break;
            }
        }
    }
}