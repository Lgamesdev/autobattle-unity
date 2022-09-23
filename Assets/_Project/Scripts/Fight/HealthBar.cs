using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class HealthBar : MonoBehaviour
    {
        private Slider _bar;

        private TextMeshProUGUI _currentHealth;
        private TextMeshProUGUI _maxHealth;
        
        private void Start()
        {
            _bar = GetComponent<Slider>();

            _currentHealth = transform.Find("Fill Area").Find("HealthText").Find("CurrentHealth").GetComponent<TextMeshProUGUI>();
            _maxHealth = transform.Find("Fill Area").Find("HealthText").Find("MaxHealth").GetComponent<TextMeshProUGUI>();
        }

        public void SetSize(float sizeNormalized)
        {
            _bar.value = sizeNormalized;
        }

        public void SetCurrentHealth(int healthAmount)
        {
            _currentHealth.text = healthAmount.ToString();
        }

        public void SetMaxHealth(int healthAmount)
        {
            _maxHealth.text = healthAmount.ToString();
        }
    }
}