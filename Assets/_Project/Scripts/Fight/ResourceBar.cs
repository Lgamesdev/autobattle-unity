using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class ResourceBar : MonoBehaviour
    {
        private Slider _bar;

        private TextMeshProUGUI _currentResource;
        private TextMeshProUGUI _maxResource;
        
        private void Start()
        {
            _bar = GetComponent<Slider>();

            _currentResource = transform.Find("Fill Area").Find("HealthText").Find("CurrentHealth").GetComponent<TextMeshProUGUI>();
            _maxResource = transform.Find("Fill Area").Find("HealthText").Find("MaxHealth").GetComponent<TextMeshProUGUI>();
        }

        public void SetSize(float sizeNormalized)
        {
            _bar.value = sizeNormalized;
        }

        public void SetCurrentResource(int healthAmount)
        {
            _currentResource.text = healthAmount.ToString();
        }

        public void SetMaxResource(int healthAmount)
        {
            _maxResource.text = healthAmount.ToString();
        }
    }
}