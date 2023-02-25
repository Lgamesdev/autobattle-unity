using Core.Player;
using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class InfosUI : MonoBehaviour
    {
        public TextMeshProUGUI username;
        public TextMeshProUGUI level;

        private void Start()
        {
            CharacterManager.Instance.PlayerInfosUpdate += UpdateUI;
        }

        private void UpdateUI(Character character)
        {
            username.text = character.Username;
            level.text = "Level : " + character.Level;
        }
    }
}

