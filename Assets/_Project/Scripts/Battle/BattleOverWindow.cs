using TMPro;
using UnityEngine;

namespace LGamesDev.Battle
{
    public class BattleOverWindow : MonoBehaviour
    {
        private TextMeshProUGUI _winnerText;
    
        private void Awake()
        {
            _winnerText = transform.Find("BattleOverPanel").Find("winnerText").GetComponent<TextMeshProUGUI>();
        
            BattleHandler.Instance.onPlayerWin += () => Show("You Win ! ");
            BattleHandler.Instance.onPlayerLose += () => Show("Enemy Wins ! ");
        }

        private void Start()
        {
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show(string winnerText)
        {
            gameObject.SetActive(true);

            _winnerText.text = winnerText;
        }
    }
}