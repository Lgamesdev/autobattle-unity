using System;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class EarningWindow : MonoBehaviour
    {
        public static EarningWindow Instance;

        public float smoothing = 3f;
        private bool animate;
        private Slider experienceBar;
        private Text experienceText;
        private Text goldGained;

        public LevelSystem levelSystem;

        private Text levelText;

        private void Awake()
        {
            Instance = this;

            levelText = transform.Find("levelText").GetComponent<Text>();
            experienceText = transform.Find("experienceText").GetComponent<Text>();
            experienceBar = transform.Find("experienceBar").GetComponent<Slider>();
            goldGained = transform.Find("goldGained").GetComponent<Text>();

            //BattleHandler.instance.onPlayerWin += BattleHandler_OnPlayerWin;
            PlayerWalletManager.Instance.OnCurrencyGained += Wallet_OnCurrencyGained;
        }

        private void Update()
        {
            if (animate)
            {
                if (Mathf.Abs(levelSystem.GetExperienceNormalized() - experienceBar.value) > smoothing * 0.05)
                {
                    SetExperienceBar(Mathf.Lerp(experienceBar.value, levelSystem.GetExperienceNormalized(),
                        smoothing * Time.deltaTime));
                }
                else
                {
                    SetExperienceBar(levelSystem.GetExperienceNormalized());
                    animate = false;
                }
            }
        }

        private void Wallet_OnCurrencyGained(Currency currency)
        {
            goldGained.text = currency.amount.ToString();
        }

        private void SetExperienceBar(float experience)
        {
            experienceBar.value = experience;
        }

        private void SetGainedExperienceNumber(float xpNumber)
        {
            experienceText.text = "+" + xpNumber + " XP";
            //Debug.Log("+" + xpNumber + "xp");
        }

        private void SetLevelNumber(int levelNumber)
        {
            levelText.text = "LEVEL \n" + levelNumber;
        }

        public void SetLevelSystem(LevelSystem levelSystem)
        {
            this.levelSystem = levelSystem;
            SetLevelNumber(levelSystem.GetLevel());

            levelSystem.onExperienceChanged += LevelSystem_OnExperienceChanged;
            levelSystem.onLevelChanged += LevelSystem_onLevelChanged;
        }

        private void LevelSystem_onLevelChanged(object sender, EventArgs e)
        {
            SetLevelNumber(levelSystem.GetLevel());
        }

        private void LevelSystem_OnExperienceChanged(float xpGained)
        {
            SetGainedExperienceNumber(xpGained);
            animate = true;
        }
    }
}