using UnityEngine;

namespace Units {
    public class HealthBar : MonoBehaviour {
        internal HealthSystem healthSystem;

        public void Setup(HealthSystem healthSystem) {
            this.healthSystem = healthSystem;
        }
        
        private void Update() {
            if (healthSystem != null) {
                transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
            }
        }
    }
}