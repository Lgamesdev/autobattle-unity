using TMPro;
using UnityEngine;

namespace LGamesDev.Fighting
{
    public class FightOverWindow : MonoBehaviour
    {
        private TextMeshProUGUI _winnerText;
    
        private void Awake()
        {
            _winnerText = transform.Find("BattleOverPanel").Find("winnerText").GetComponent<TextMeshProUGUI>();
        
            FightManager.Instance.onPlayerWin += () => Show("You Win ! ");
            FightManager.Instance.onPlayerLose += () => Show("Enemy Wins ! ");
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