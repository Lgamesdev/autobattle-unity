using System;

namespace LGamesDev.Fighting
{
    public class HealthSystem
    {
        private int health;
        private readonly int healthMax;

        public HealthSystem(int health)
        {
            healthMax = health;
            this.health = health;
        }

        public event EventHandler OnHealthChanged;

        public int GetHealthAmount()
        {
            return health;
        }

        public float GetHealthPercent()
        {
            return (float)health / healthMax;
        }

        public void Damage(int damageAmount)
        {
            health -= damageAmount;
            if (health < 0) health = 0;
            if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        }

        public void Heal(int healAmount)
        {
            health += healAmount;
            if (health > healthMax) health = healthMax;
            if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        }

        public bool IsDead()
        {
            if (health <= 0)
                return true;
            return false;
        }
    }
}