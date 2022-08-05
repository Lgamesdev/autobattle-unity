using System;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class Character_LevelSystem : MonoBehaviour
    {
        private LevelSystem levelSystem;
        public event EventHandler OnLevelChanged;

        public void SetLevelSystem(LevelSystem levelSystem)
        {
            this.levelSystem = levelSystem;
            this.levelSystem.onLevelChanged += LevelSystem_onLevelChanged;
        }

        private void LevelSystem_onLevelChanged(object sender, EventArgs e)
        {
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetLevel()
        {
            return levelSystem.GetLevel();
        }

        public float GetExperience()
        {
            return levelSystem.GetExperience();
        }
    }
}