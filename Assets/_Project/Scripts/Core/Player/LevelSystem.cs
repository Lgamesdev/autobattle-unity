using System;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class LevelSystem
    {
        public delegate void OnExperienceChangedEvent(int amount);
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
        
        public void AddExperience(int xpGained)
        {
            if (IsMaxLevel()) return;
            
            /*_currentExperience += xpGained;
            while (!IsMaxLevel() && _currentExperience >= CalculateRequiredXp(_level))
            {
                //Enough experience to level up
                _currentExperience -= CalculateRequiredXp(_level);
                _level++;
                onLevelChanged?.Invoke(this, EventArgs.Empty);
            }*/

            OnExperienceChanged?.Invoke(xpGained);
        }

        public int GetLevel()
        {
            return _level;
        }

        public int GetExperienceNormalized()
        {
            if (IsMaxLevel())
                return 1;
            
            return _currentExperience / CalculateRequiredXp(_level);
        }

        public int GetExperience()
        {
            return _currentExperience;
        }

        public int CalculateRequiredXp(int level)
        {
            var solveForRequiredXp = 0;
            for (var levelCycle = 1; levelCycle <= level; levelCycle++)
                solveForRequiredXp += Mathf.FloorToInt(levelCycle + XpAdditionMultiplier *
                    Mathf.Pow(XpPowerMultiplier, levelCycle / XpDivisionMultiplier));

            return solveForRequiredXp / 4;
        }

        private bool IsMaxLevel()
        {
            return IsMaxLevel(_level);
        }

        private bool IsMaxLevel(int level)
        {
            return level == 200;
        }
    }
}