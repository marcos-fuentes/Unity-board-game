using System;
using System.Threading.Tasks;
using Tiles;
using UnityEngine;

namespace Units {
    public class BaseUnit : MonoBehaviour { 
        public BaseTile occupiedBaseTile;
        public Faction faction;
        public Class unitClass;
        public string unitName;
        public int movementArea = 3;
        public int attackArea = 1;
        public HealthBar healthBar;
        internal int unitMaxHealth = 3;
        internal HealthSystem healthSystem;

        public int attackDamage = 1;

        //ANIMATIONS
        private Animator anim;

        // Start is called before the first frame update
        private void Start() {
            healthSystem = new HealthSystem(unitMaxHealth);
            healthBar.Setup(healthSystem);
            Debug.Log($"Unit name: {unitName} | Health: {healthBar._healthSystem.GetHealthPercent()}");

            anim = gameObject.GetComponent<Animator>();
        }

        public bool DamageUnit(int damage) {
            var isDead = false;
            healthSystem.Damage(damage);
            if (healthSystem.GetHealth() > 0) anim.Play("Hurt");
            else {
                Dying(); 
                isDead = true;
            }
            return isDead;
        }

        private void Dying() => anim.Play("Dying");

        public void HealUnit(int heal) => healthSystem.Heal(heal);
        
        public void AttackAnimation() => anim.Play("Slashing");
        public void HealAnimation() => anim.Play("Throwing");
    }
}