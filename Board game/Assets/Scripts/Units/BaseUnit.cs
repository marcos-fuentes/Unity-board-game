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
        public HealBar healBar;
        internal int unitMaxHealth = 3;
        internal HealthSystem HealthSystem;
        internal HealSystem HealSystem;

        public int attackDamage = 1;

        //ANIMATIONS
        private Animator anim;

        // Start is called before the first frame update
        private void Start() {
            HealthSystem = new HealthSystem(unitMaxHealth);
            healthBar.Setup(HealthSystem);
            Debug.Log($"Unit name: {unitName} | Health: {healthBar._healthSystem.GetHealthPercent()}");

            if (unitClass == Class.Magician) {
                HealSystem = new HealSystem(3);
                healBar.Setup(HealSystem);
            }
            
            anim = gameObject.GetComponent<Animator>();
        }

        public bool DamageUnit(int damage) {
            var isDead = false;
            HealthSystem.Damage(damage);
            if (HealthSystem.GetHealth() > 0) anim.Play("Hurt");
            else {
                Dying(); 
                isDead = true;
            }
            return isDead;
        }
        
        public void SubtractHealPoints(int healPoints) {
            if (unitClass == Class.Magician) HealSystem.SubtractHealPoints(healPoints);
        }

        private void Dying() => anim.Play("Dying");

        public void HealUnit(int heal) => HealthSystem.Heal(heal);
        
        public void AttackAnimation() => anim.Play("Slashing");
        public void HealAnimation() => anim.Play("Throwing");
    }
}