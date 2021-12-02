using System;
using UnityEngine;

namespace Units {
    public class HealthSystem {
        public event EventHandler OnHealthChanged;
        
        private int healthMax;
        private int health;
        
        public HealthSystem(int healthMax) {
            this.healthMax = healthMax;
            health = healthMax;
        }

        public int GetHealth() {
            return health;
        }

        public float GetHealthPercent() { 
            return (float) health / healthMax;
        }

         public void Damage(int damageAmount) {
            health -= damageAmount;
            if (health < 0) health = 0;
            if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
         }

        public void Heal(int healthAmount) {
            health += healthAmount;
            if (health > healthMax) health = healthMax;
            if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        }

        public int GetHealthMax() {
            return healthMax;
        }
    }
}