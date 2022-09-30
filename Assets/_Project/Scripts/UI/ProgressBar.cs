using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        public int minimum;
        public int maximum;
        public int current;
        public Image mask;
        public Image fill;
        public Color color;

        public TextMeshProUGUI progressText;
        public TextMeshProUGUI percentageText;

#if UNITY_EDITOR
        [MenuItem("GameObject/UI/Linear Progress Bar")]
        public static void AddLinearProgressBar()
        {
            Instantiate(Resources.Load<GameObject>("UI/Linear Progress Bar"), Selection.activeGameObject.transform, false);
        }
#endif

        private void Awake()
        {
            progressText = transform.Find("Text Panel").Find("ProgressText").GetComponent<TextMeshProUGUI>();
            percentageText = transform.Find("Text Panel").Find("PercentageText").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            GetCurrentFill();
        }

        private void GetCurrentFill()
        {
            float currentOffset = current - minimum;
            float maximumOffset = maximum - minimum;
            var fillAmount = currentOffset / maximumOffset;
            mask.fillAmount = fillAmount;

            fill.color = color;
        }
    }
}