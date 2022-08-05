using System;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class LevelSystem
    {
        public delegate void OnExperienceChanged(float amount);

        private const int XP_ADDITION_MULITPLIER = 300;
        private const int XP_POWER_MULITPLIER = 2;
        private const int XP_DIVISION_MULITPLIER = 14;
        private float currentXp;

        private int level;
        public OnExperienceChanged onExperienceChanged;

        public LevelSystem(int level, float experience)
        {
            this.level = level;
            currentXp = experience;
        }

        public event EventHandler onLevelChanged;

        public void AddExperienceScalable(float xpGained, int passedLevel)
        {
            if (!IsMaxLevel())
            {
                if (passedLevel < level)
                {
                    var multiplier = 1 + (level - passedLevel) * 0.1f;
                    xpGained = xpGained * multiplier;
                }

                currentXp += xpGained;

                while (!IsMaxLevel() && currentXp >= CalculateRequiredXp(level))
                {
                    //Enough experience to level up
                    currentXp -= CalculateRequiredXp(level);
                    level++;
                    if (onLevelChanged != null) onLevelChanged(this, EventArgs.Empty);
                }

                onExperienceChanged?.Invoke(xpGained);
            }
        }

        public void AddExperienceFlat(int xpGained)
        {
            if (!IsMaxLevel())
            {
                currentXp += xpGained;
                while (!IsMaxLevel() && currentXp >= CalculateRequiredXp(level))
                {
                    //Enough experience to level up
                    currentXp -= CalculateRequiredXp(level);
                    level++;
                    if (onLevelChanged != null) onLevelChanged(this, EventArgs.Empty);
                }

                onExperienceChanged?.Invoke(xpGained);
            }
        }

        public int GetLevel()
        {
            return level;
        }

        public float GetExperienceNormalized()
        {
            if (IsMaxLevel())
                return 1f;
            return currentXp / CalculateRequiredXp(level);
        }

        public float GetExperience()
        {
            return currentXp;
        }

        public int CalculateRequiredXp(int level)
        {
            var solveForRequiredXp = 0;
            for (var levelCycle = 1; levelCycle <= level; levelCycle++)
                solveForRequiredXp += Mathf.FloorToInt(levelCycle + XP_ADDITION_MULITPLIER *
                    Mathf.Pow(XP_POWER_MULITPLIER, levelCycle / XP_DIVISION_MULITPLIER));

            return solveForRequiredXp / 4;
        }

        public bool IsMaxLevel()
        {
            return IsMaxLevel(level);
        }

        public bool IsMaxLevel(int level)
        {
            return level == 200;
        }
    }
}