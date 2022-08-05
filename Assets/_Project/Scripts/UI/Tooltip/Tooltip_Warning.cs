using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class Tooltip_Warning : MonoBehaviour
    {
        private static Tooltip_Warning instance;
        private Image backgroundImage;
        private RectTransform backgroundRectTransform;
        private int flashState;
        private float flashTimer;

        private float showTimer;

        private TextMeshProUGUI tooltipText;

        private void Awake()
        {
            instance = this;
            backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
            tooltipText = transform.Find("text").GetComponent<TextMeshProUGUI>();
            backgroundImage = transform.Find("background").GetComponent<Image>();

            HideTooltip();

            //ShowTooltip("Random tooltip text");
        }

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
                Input.mousePosition, null, out var localPoint);
            transform.localPosition = localPoint;

            flashTimer += Time.deltaTime;
            var flashTimerMax = .033f;
            if (flashTimer > flashTimerMax)
            {
                flashState++;
                switch (flashState)
                {
                    case 1:
                    case 3:
                    case 5:
                        tooltipText.color = new Color(1, 1, 1, 1);
                        backgroundImage.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                        break;
                    case 2:
                    case 4:
                        tooltipText.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                        backgroundImage.color = new Color(1, 1, 1, 1);
                        break;
                }
            }

            showTimer -= Time.deltaTime;
            if (showTimer <= 0f) HideTooltip();
        }

        private void ShowTooltip(string tooltipString, float showTimerMax = 2f)
        {
            gameObject.SetActive(true);

            tooltipText.text = tooltipString;
            var textPaddingSize = 4f;
            var backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f,
                tooltipText.preferredHeight + textPaddingSize * 2f);
            backgroundRectTransform.sizeDelta = backgroundSize;
            showTimer = showTimerMax;
            flashTimer = 0f;
            flashState = 0;
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