using System;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class LevelSystem
    {
        public delegate void OnExperienceChangedEvent(int level, int oldExperience, int aimedExperience, int maxExperience);
        public OnExperienceChangedEvent OnExperienceChanged;

        private const int XpAdditionMultiplier = 300;
        private const int XpPowerMultiplier = 2;
        private const int XpDivisionMultiplier = 14;
        
        //public event EventHandler onLevelChanged;

        private readonly int _level;
        private readonly int _currentExperience;

        public LevelSystem(int level, int experience)
        {
            _level = level;
            _currentExperience = experience;
        }
        
        public void AddExperience(int level, int oldExperience, int aimedExperience, int maxExperience)
        {
            if (IsMaxLevel()) return;
            OnExperienceChanged?.Invoke(level, oldExperience, aimedExperience, maxExperience);
        }

        public int GetLevel()
        {
            return _level;
        }

        public int GetExperience()
        {
            return _currentExperience;
        }

        public int CalculateRequiredXp(int level)
        {
            var solveForRequiredXp = 0;
            for (int levelCycle = 1; levelCycle <= level; levelCycle++)
            {
                solveForRequiredXp += Mathf.FloorToInt(levelCycle + XpAdditionMultiplier *
                    Mathf.Pow(XpPowerMultiplier, levelCycle / XpDivisionMultiplier));
            }

            return solveForRequiredXp / 4;
        }

        private bool IsMaxLevel()
        {
            return IsMaxLevel(_level);
        }

        private bool IsMaxLevel(int level)
        {
            return level == 100;
        }
    }
}