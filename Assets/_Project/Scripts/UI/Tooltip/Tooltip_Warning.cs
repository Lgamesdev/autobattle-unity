using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class Tooltip_Warning : MonoBehaviour
    {
        private static Tooltip_Warning _instance;
        private Image _backgroundImage;
        private RectTransform _backgroundRectTransform;
        private int _flashState;
        private float _flashTimer;

        private float _showTimer;

        private TextMeshProUGUI _tooltipText;

        private void Awake()
        {
            _instance = this;
            _backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
            _tooltipText = _backgroundRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            _backgroundImage = transform.Find("background").GetComponent<Image>();

            HideTooltip();

            //ShowTooltip("Random tooltip text");
        }

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
                Input.mousePosition, null, out var localPoint);
            transform.localPosition = localPoint;

            _flashTimer += Time.deltaTime;
            var flashTimerMax = .033f;
            if (_flashTimer > flashTimerMax)
            {
                _flashState++;
                switch (_flashState)
                {
                    case 1:
                    case 3:
                    case 5:
                        _tooltipText.color = new Color(1, 1, 1, 1);
                        _backgroundImage.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                        break;
                    case 2:
                    case 4:
                        _tooltipText.color = new Color(178f / 255f, 0 / 255f, 0 / 255f, 1);
                        _backgroundImage.color = new Color(1, 1, 1, 1);
                        break;
                }
            }

            _showTimer -= Time.deltaTime;
            if (_showTimer <= 0f) HideTooltip();
        }

        private void ShowTooltip(string tooltipString, float showTimerMax = 2f)
        {
            gameObject.SetActive(true);

            _tooltipText.text = tooltipString;
            var textPaddingSize = 4f;
            var backgroundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize * 2f,
                _tooltipText.preferredHeight + textPaddingSize * 2f);
            _backgroundRectTransform.sizeDelta = backgroundSize;
            _showTimer = showTimerMax;
            _flashTimer = 0f;
            _flashState = 0;
        }

        private void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        public static void ShowTooltip_Static(string tooltipString)
        {
            _instance.ShowTooltip(tooltipString);
        }

        public static void HideTooltip_Static()
        {
            _instance.HideTooltip();
        }
    }
}