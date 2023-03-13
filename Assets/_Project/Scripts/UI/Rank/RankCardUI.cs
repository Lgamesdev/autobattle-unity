using LGamesDev.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LGamesDev.UI
{
    public class RankCardUI : MonoBehaviour
    {
        private Character _character;

        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI rankText;

        private string _message;

        public void SetupCard(Character character, bool isPlayerCharacter)
        {
            _character = character;

            usernameText.text = isPlayerCharacter ? character.Username + " (You)" : character.Username;
            levelText.text = "lvl." + _character.Level;
            rankText.text = AbbreviationUtility.AbbreviateNumber(_character.Ranking);

            _message = "Character Infos of " + _character.Username + " : \n" +
                       "Level : " + _character.Level + "\n" +
                       "Ranking : " + _character.Ranking + "\n";
        }

        public void OnDetailButton()
        {
            GameManager.Instance.modalWindow.ShowAsPrompt(
                _character.Username,
                null,
                _message
            );
        }
    }
}