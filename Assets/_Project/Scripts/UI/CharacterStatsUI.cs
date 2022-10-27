
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class CharacterStatsUI : MonoBehaviour
    {
        [SerializeField] private Transform pfUIStatSlot;

        private CharacterStatsManager _characterStatsManager;

        private Transform _defensiveStats;
        private Transform _offensiveStats;

        private void Awake()
        {
            _defensiveStats = transform.Find("statsParent").Find("Container").Find("Defensive Stats");
            _offensiveStats = transform.Find("statsParent").Find("Container").Find("Offensive Stats");
        }

        private void Start()
        {
            /*transform.Find("attackBtn").GetComponent<Button_UI>().MouseOverOnceFunc = () => Tooltip.ShowTooltip_Static("Attack");
        transform.Find("attackBtn").GetComponent<Button_UI>().MouseOutOnceFunc = () => Tooltip.HideTooltip_Static();

        transform.Find("defendBtn").GetComponent<Button_UI>().MouseOverOnceFunc = () => Tooltip.ShowTooltip_Static("Defend");
        transform.Find("defendBtn").GetComponent<Button_UI>().MouseOutOnceFunc = () => Tooltip.HideTooltip_Static();

        transform.Find("patrolBtn").GetComponent<Button_UI>().MouseOverOnceFunc = () => Tooltip.ShowTooltip_Static("Patrol");
        transform.Find("patrolBtn").GetComponent<Button_UI>().MouseOutOnceFunc = () => Tooltip.HideTooltip_Static();*/

            _characterStatsManager = CharacterStatsManager.Instance;

            _characterStatsManager.OnStatsUpdated += OnStatsUpdated;

            OnStatsUpdated(_characterStatsManager.stats);
        }

        private void OnStatsUpdated(Stat[] stats)
        {
            foreach (Transform child in _defensiveStats) Destroy(child.gameObject);
            foreach (Transform child in _offensiveStats) Destroy(child.gameObject);

            foreach (var stat in stats)
            {
                //if (stat.GetValue() != 0)
                //{
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
                //}
            }
        }

        private void CreateStatSlot(Stat stat, Transform parent)
        {
            var statSlotRectTransform = Instantiate(pfUIStatSlot, parent).GetComponent<RectTransform>();

            var label = statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>();
            var value = statSlotRectTransform.Find("values").Find("value").GetComponent<TextMeshProUGUI>();
            statSlotRectTransform.Find("values").Find("modifier").gameObject.SetActive(false);

            label.text = stat.GetStatType();
            value.text = stat.GetValue().ToString();
        }
    }
}