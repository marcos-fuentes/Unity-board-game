using System;
using UnityEngine;

namespace Units {
    public class HealBar : MonoBehaviour {
        internal HealSystem _healSystem;
        [SerializeField] private GameObject _bar;

        public void Setup(HealSystem healSystem) {
            _healSystem = healSystem;
            healSystem.OnHealPointsChanged += HealSystem_OnHealthChanged;
        }

        private void HealSystem_OnHealthChanged(object sender, EventArgs e) {
            if (_healSystem != null) {
                _bar.transform.localScale = new Vector3(_healSystem.GetHealthPercent(), 1);
                Debug.Log(_bar.name + " " + _healSystem.GetHealthPercent());
            }   
        }
    }
}