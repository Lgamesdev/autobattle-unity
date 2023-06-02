using System;
using LGamesDev.Core.Player;
using LGamesDev.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class RewardUI : MonoBehaviour
    {
        public static RewardUI Instance;
        
        public float smoothing = 3f;
        private bool _animate;
        
        private Slider _experienceBar;
        private TextMeshProUGUI _experienceGain;
        private TextMeshProUGUI _goldGain;
        private TextMeshProUGUI _rankGain;
        
        private TextMeshProUGUI _currentExperienceText;
        private TextMeshProUGUI _maxExperienceText;

        private LevelSystem _levelSystem;

        private TextMeshProUGUI _levelText;

        private LTDescr _actualAnim; 

        private void Awake()
        {
            Instance = this;
            
            _levelText = transform.Find("levelText").GetComponent<TextMeshProUGUI>();
            
            _experienceBar = transform.Find("experienceBar").GetComponent<Slider>();
            
            _currentExperienceText = transform.Find("experienceBar").Find("Fill Area").Find("ExperienceText")
                .Find("CurrentExperience").GetComponent<TextMeshProUGUI>();
            _maxExperienceText = transform.Find("experienceBar").Find("Fill Area").Find("ExperienceText")
                .Find("MaxExperience").GetComponent<TextMeshProUGUI>();
            
            
            _experienceGain = transform.Find("Rewards").Find("ExperienceAndGold").Find("Experience").Find("experienceText").GetComponent<TextMeshProUGUI>();
            _goldGain = transform.Find("Rewards").Find("ExperienceAndGold").Find("Gold").Find("goldText").GetComponent<TextMeshProUGUI>();
            _rankGain = transform.Find("Rewards").Find("RankingAndItems").Find("Ranking").Find("rankingText").GetComponent<TextMeshProUGUI>();

            FightManager.Instance.FightOver += OnFightOver;
        }

        private void OnFightOver(Fight fight)
        {
            foreach (Currency currency in fight.Reward.Currencies)
            {
                switch (currency.currencyType)
                {
                    case CurrencyType.Gold:
                        _goldGain.text = AbbreviationUtility.AbbreviateNumber(currency.amount);
                        break;
                    case CurrencyType.Crystal:
                        Debug.Log("crystal gained : +" +  AbbreviationUtility.AbbreviateNumber(currency.amount));
                        break;
                }
            }

            if (fight.Reward.Ranking > 0)
            {
                _rankGain.text = "+" + AbbreviationUtility.AbbreviateNumber(fight.Reward.Ranking);
            }
            else
            {
                _rankGain.text = AbbreviationUtility.AbbreviateNumber(fight.Reward.Ranking);
            }
            
            _experienceGain.text = "+" + AbbreviationUtility.AbbreviateNumber(fight.Reward.Experience);
        }

        private void AddToExperienceBar(int level, int oldExperience, int aimedExperience, int maxExperience)
        {
            float animTime = 2f;

            if (_actualAnim != null)
            {
                _actualAnim.setOnComplete(() =>
                {
                    _levelText.text = "LEVEL \n" + level;
                    _maxExperienceText.text = maxExperience.ToString();
                    AnimExperienceBar(oldExperience, aimedExperience, maxExperience, animTime);
                });
            }
            else
            {
                _levelText.text = "LEVEL \n" + level;
                _maxExperienceText.text = maxExperience.ToString();
                AnimExperienceBar(oldExperience, aimedExperience, maxExperience, animTime);
            }
        }

        private void AnimExperienceBar(int actualExperience, int aimedExperience, int maxExperience, float time)
        {
            _actualAnim = LeanTween.value(_experienceBar.gameObject, actualExperience, aimedExperience, time)
                .setEase(LeanTweenType.linear)
                .setOnUpdate(val =>
                {
                    _experienceBar.value = val / maxExperience;
                    _currentExperienceText.text = Mathf.RoundToInt(val).ToString();
                });
        }

        public void SetLevelSystem(LevelSystem levelSystem)
        {
            _levelSystem = levelSystem;
            _levelText.text = "LEVEL \n" + levelSystem.GetLevel();

            levelSystem.OnExperienceChanged += AddToExperienceBar;
        }
    }
}