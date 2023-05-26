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

            usernameText.text = isPlayerCharacter ? character.username + " (You)" : character.username;
            levelText.text = "lvl." + _character.level;
            rankText.text = AbbreviationUtility.AbbreviateNumber(_character.Ranking);

            _message = "Character Infos of " + _character.username + " : \n" +
                       "level : " + _character.level + "\n" +
                       "Ranking : " + _character.Ranking + "\n";
        }

        public void OnDetailButton()
        {
            GameManager.Instance.modalWindow.ShowAsPrompt(
                _character.username,
                null,
                _message
            );
        }
    }
}