
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
            _defensiveStats = transform.Find("statsParent").Find("Container").Find("Defensive Stats");
            _offensiveStats = transform.Find("statsParent").Find("Container").Find("Offensive Stats");
        }

        private void Start()
        {
            _characterStatsManager = CharacterStatsManager.Instance;

            _characterStatsManager.OnStatsUpdated += OnStatsUpdated;

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
                        or StatType.Armor
                        or StatType.Dodge:
                        CreateStatSlot(stat, _defensiveStats);
                        break;
                    case StatType.Damage
                        or StatType.Critical
                        or StatType.Speed:
                        CreateStatSlot(stat, _offensiveStats);
                        break;
                }
            }
        }

        private void CreateStatSlot(Stat stat, Transform parent)
        {
            StatSlotUI statSlot = Instantiate(pfUIStatSlot, parent);
            statSlot.SetupSlot(stat, null, true);
        }
    }
}