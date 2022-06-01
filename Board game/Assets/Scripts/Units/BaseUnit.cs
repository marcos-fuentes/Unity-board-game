using Managers;
using Tiles;
using UnityEngine;

namespace Units
{
    public class BaseUnit : MonoBehaviour
    {
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
        internal bool isAlive = true;

        public int attackDamage = 1;

        //ANIMATIONS
        private Animator anim;

        // Start is called before the first frame update
        private void Start()
        {
            HealthSystem = new HealthSystem(unitMaxHealth);
            healthBar.Setup(HealthSystem);
            Debug.Log($"Unit name: {unitName} | Health: {healthBar._healthSystem.GetHealthPercent()}");

            if (unitClass == Class.Magician || unitClass == Class.Warlock)
            {
                HealSystem = new HealSystem(3);
                healBar.Setup(HealSystem);
            }

            anim = gameObject.GetComponent<Animator>();
        }

        public bool DamageUnit(int damage)
        {
            var isDead = false;
            HealthSystem.Damage(damage);
            HitSound();
            if (HealthSystem.GetHealth() > 0) anim.Play("Hurt");
            else
            {
                Debug.Log("Dying");
                Dying();
                DyingSound();
                isDead = true;
                isAlive = false;
                occupiedBaseTile._tileUnit = null;
                occupiedBaseTile = null;
            }

            return isDead;
        }

        public void SubtractHealPoints(int healPoints)
        {
            if (unitClass == Class.Magician || unitClass == Class.Warlock) HealSystem.SubtractHealPoints(healPoints);
        }

        private void Dying() => anim.Play("Dying");

        public void HealUnit(int heal) => HealthSystem.Heal(heal);
        public void ManaUnit(int heal) => HealSystem.AddHealPoints(heal);

        public void AttackAnimation() => anim.Play("Slashing");

        public void HealAnimation() => anim.Play("Throwing");


        public void MaxOutAnimation() => anim.Play("MaxOut");


        public void ManaAnimation() => anim.Play("Mana");

        public void AttackSound() => SoundManager.Instance.playAttackSound();
        public void HitSound() => SoundManager.Instance.playHurtSound();
        public void HealSound() => SoundManager.Instance.playHealSound();
        private void DyingSound() => SoundManager.Instance.playDeadSound();
        public void WalkSound() => SoundManager.Instance.playWalkSound();
    }
}