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

        private void OnFightOver(Reward reward, bool playerWin)
        {
            foreach (Currency currency in reward.Currencies)
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

            _rankGain.text = "+" + AbbreviationUtility.AbbreviateNumber(reward.Ranking);
        }

        private void AddToExperienceBar(int xpGained)
        {
            int level = _levelSystem.GetLevel();
            int maxExperience = _levelSystem.CalculateRequiredXp(level);
            
            int oldExperience = _levelSystem.GetExperience() - xpGained;
            int aimedExperience = _levelSystem.GetExperience();
            
            float animTime = 2f;
            Action onComplete = null;

            //if level up
            if (oldExperience < 0)
            {
                level -= 1;
                maxExperience = _levelSystem.CalculateRequiredXp(level);

                oldExperience = maxExperience + oldExperience;
                aimedExperience = maxExperience;

                //Debug.Log("old experience : " + oldExperience);

                animTime /= 2;
                
                onComplete = () =>
                {
                    level += 1;
                    maxExperience = _levelSystem.CalculateRequiredXp(level);
                    
                    aimedExperience = _levelSystem.GetExperience();
                    
                    _maxExperienceText.text = maxExperience.ToString();
                    _levelText.text = "LEVEL \n" + level;
                    
                    AnimExperienceBar(0, aimedExperience, level, animTime);
                };
            }
            
            
            _maxExperienceText.text = maxExperience.ToString();
            AnimExperienceBar(oldExperience, aimedExperience, level, animTime, onComplete);
        }

        private void AnimExperienceBar(float actualExperience, float aimedExperience, int level, float time, Action onComplete = null)
        {
            int maxExperience = _levelSystem.CalculateRequiredXp(level);
            
            LeanTween.value(_experienceBar.gameObject, actualExperience, aimedExperience, time).setEase(LeanTweenType.linear)
                .setOnUpdate(val =>
                {
                    _experienceBar.value = val / maxExperience;
                    _currentExperienceText.text = Mathf.RoundToInt(val).ToString();
                })
                .setOnComplete(() => {
                    onComplete?.Invoke();
                }
            );
        }

        private void SetGainedExperience(float xpNumber)
        {
            _experienceGain.text = "+" + AbbreviationUtility.AbbreviateNumber(xpNumber);
        }

        public void SetLevelSystem(LevelSystem levelSystem)
        {
            _levelSystem = levelSystem;
            _levelText.text = "LEVEL \n" + levelSystem.GetLevel();

            levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        }

        private void LevelSystem_OnExperienceChanged(int xpGained)
        {
            SetGainedExperience(xpGained);
            AddToExperienceBar(xpGained);
        }
    }
}