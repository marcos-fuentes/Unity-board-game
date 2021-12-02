using UnityEngine;

namespace Units {
    public class HealthBar : MonoBehaviour {
        internal HealthSystem HealthSystem;

        public void Setup(HealthSystem healthSystem) {
            this.HealthSystem = healthSystem;

            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
            if (HealthSystem != null) {
                transform.Find("Bar").localScale = new Vector3(HealthSystem.GetHealthPercent(), 1);
            }   
        }
    }
}