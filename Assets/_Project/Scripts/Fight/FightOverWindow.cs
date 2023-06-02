using TMPro;
using UnityEngine;

namespace LGamesDev.Fighting
{
    public class FightOverWindow : MonoBehaviour
    {
        private TextMeshProUGUI _winnerText;
    
        private void Awake()
        {
            _winnerText = transform.Find("FightOverPanel").Find("winnerText").GetComponent<TextMeshProUGUI>();
        }
        
        private void Start()
        {
            Hide();
            
            FightManager.Instance.FightOver += OnFightOver;
        }

        private void OnFightOver(Fight fight)
        {
            Show(fight.PlayerWin ? "Congrats, You win !" : "You lose !");
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