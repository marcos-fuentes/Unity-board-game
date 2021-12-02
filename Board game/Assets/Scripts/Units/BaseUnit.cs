using System;
using System.Threading.Tasks;
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units {
    public class BaseUnit : MonoBehaviour {
        [FormerlySerializedAs("OccupiedTile")] public BaseTile occupiedBaseTile;
        public Faction faction;
        public string unitName;
        public int movementArea = 3;
        public HealthBar healthBar;

        // Start is called before the first frame update
        private void Start() {
            var healthSystem = new HealthSystem(3);
            healthBar.Setup(healthSystem);
            Debug.Log($"Unit name: {unitName} | Health: {healthBar.healthSystem.GetHealthPercent()}");
            TestHealth();
        }

        private async Task TestHealth() {
            while (healthBar.healthSystem.GetHealth() > 0) {
                Debug.Log($"Health: {healthBar.healthSystem.GetHealthPercent()}");
                await Task.Delay(TimeSpan.FromSeconds(3));
                healthBar.healthSystem.Damage(1);
            }
        }
    }
}