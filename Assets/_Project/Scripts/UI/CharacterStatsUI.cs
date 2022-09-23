
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class CharacterStatsUI : MonoBehaviour
    {
        [SerializeField] private Transform pfUIStatSlot;

        private CharacterStatsManager _characterStatsManager;

        private Transform _statsParent;

        private void Awake()
        {
            _statsParent = transform.Find("statsParent");
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
            foreach (Transform child in _statsParent) Destroy(child.gameObject);

            foreach (var stat in stats)
                if (stat.GetValue() != 0)
                {
                    var statSlotRectTransform = Instantiate(pfUIStatSlot, _statsParent).GetComponent<RectTransform>();

                    var label = statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>();
                    var value = statSlotRectTransform.Find("value").GetComponent<TextMeshProUGUI>();

                    label.text = stat.GetStatType();
                    value.text = stat.GetValue().ToString();
                }
        }
    }
}