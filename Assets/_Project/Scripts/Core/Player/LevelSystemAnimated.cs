using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class LevelSystemAnimated
    {
        private float experience;
        private bool IsAnimating;

        private int level;

        private Character_LevelSystem playerLevelSystem;
        private float updateTimer;
        private readonly float updateTimerMax;
        
        // public event EventHandler OnExperienceChanged;
        // public event EventHandler OnLevelChanged;

        public LevelSystemAnimated(Character_LevelSystem levelSystem)
        {
            SetPlayerLevelSystem(levelSystem);
            updateTimerMax = .016f;

            FunctionUpdater.Create(() => Update());
        }

        private void SetPlayerLevelSystem(Character_LevelSystem playerLevelSystem)
        {
            this.playerLevelSystem = playerLevelSystem;

            level = playerLevelSystem.GetLevel();
            experience = playerLevelSystem.GetExperience();

            //playerLevelSystem.onExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
            playerLevelSystem.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        }

        private void LevelSystemAnimated_OnExperienceChanged(float xpGained)
        {
            IsAnimating = true;
        }

        private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e)
        {
            IsAnimating = true;
        }

        private void Update()
        {
            if (IsAnimating)
            {
                //Check if its time to update
                updateTimer += Time.deltaTime;
                while (updateTimer > updateTimerMax)
                {
                    //Time to update 
                    updateTimer -= updateTimerMax;
                    UpdateAddExperience();
                }
            }
        }

        private void UpdateAddExperience()
        {
            if (level < playerLevelSystem.GetLevel())
            {
                //Local level under target level
                AddExperience();
            }
            else
            {
                //Local level equals the target level
                if (experience > playerLevelSystem.GetExperience())
                    AddExperience();
                else
                    IsAnimating = false;
            }
        }

        private void AddExperience()
        {
            /*experience++;
        if(experience >= playerLevelSystem.CalculateRequiredXp(level))
        {
            level++;
            experience = 0;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);*/
        }

        public int GetLevelNumber()
        {
            return level;
        }

        public float GetExperienceNormalized()
        {
            /*if (playerLevelSystem.IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return experience / playerLevelSystem.CalculateRequiredXp(level);
        }*/

            return 1f;
        }
    }
}