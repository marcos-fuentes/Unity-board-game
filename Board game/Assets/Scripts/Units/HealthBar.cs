using System;
using UnityEngine;

namespace Units {
    public class HealthBar : MonoBehaviour {
        internal HealthSystem _healthSystem;
        [SerializeField] private GameObject _bar;

        public void Setup(HealthSystem healthSystem) {
            _healthSystem = healthSystem;
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
            if (_healthSystem != null) {
                _bar.transform.localScale = new Vector3(_healthSystem.GetHealthPercent(), 1);
                Debug.Log(_bar.name + " " + _healthSystem.GetHealthPercent());
            }   
        }
    }
}