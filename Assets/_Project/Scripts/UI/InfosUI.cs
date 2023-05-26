using Core.Player;
using LGamesDev.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class InfosUI : MonoBehaviour
    {
        public TextMeshProUGUI username;
        public TextMeshProUGUI level;
        [SerializeField] private Slider experienceBar;
        [SerializeField] private TextMeshProUGUI currentExperienceText;
        [SerializeField] private TextMeshProUGUI maxExperienceText;

        private void Start()
        {
            CharacterManager.Instance.PlayerInfosUpdate += UpdateUI;
        }

        private void UpdateUI(Character character)
        {
            username.text = character.username;
            level.text = "level : " + character.level;
            experienceBar.value = (float)character.Experience / character.MaxExperience;
            currentExperienceText.text = AbbreviationUtility.AbbreviateNumber(character.Experience);
            maxExperienceText.text = AbbreviationUtility.AbbreviateNumber(character.MaxExperience);
        }
    }
}

