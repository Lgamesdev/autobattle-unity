using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class Tooltip : MonoBehaviour
    {
        private static Tooltip instance;
        private RectTransform backgroundRectTransform;

        private TextMeshProUGUI tooltipText;

        private void Awake()
        {
            instance = this;
            backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
            tooltipText = backgroundRectTransform.Find("text").GetComponent<TextMeshProUGUI>();

            HideTooltip();

            //ShowTooltip("Random tooltip text");
        }

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
                Input.mousePosition, null, out var localPoint);
            transform.localPosition = localPoint;
        }

        private void ShowTooltip(string tooltipString)
        {
            gameObject.SetActive(true);

            tooltipText.text = tooltipString;
            var textPaddingSize = 4f;
            var backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f,
                tooltipText.preferredHeight + textPaddingSize * 2f);
            backgroundRectTransform.sizeDelta = backgroundSize;
        }

        private void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        public static void ShowTooltip_Static(string tooltipString)
        {
            instance.ShowTooltip(tooltipString);
        }

        public static void HideTooltip_Static()
        {
            instance.HideTooltip();
        }
    }
}