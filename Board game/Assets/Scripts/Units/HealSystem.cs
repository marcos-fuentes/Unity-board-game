using System;

namespace Units {
    public class HealSystem {
        public event EventHandler OnHealPointsChanged;
        
        private readonly int _healPointsMaxPoints;
        private int _healPoints;
        
        public HealSystem(int healPointsMaxPoints) {
            this._healPointsMaxPoints = healPointsMaxPoints;
            _healPoints = healPointsMaxPoints;
        }

        public int GetHealPoints() {
            return _healPoints;
        }

        public float GetHealthPercent() { 
            return (float) _healPoints / _healPointsMaxPoints;
        }

         public void SubtractHealPoints(int healPoints) {
            _healPoints -= healPoints;
            if (_healPoints < 0) _healPoints = 0;
            if (OnHealPointsChanged != null) OnHealPointsChanged(this, EventArgs.Empty);
         }

        public void AddHealPoints(int healPoints) {
            _healPoints += healPoints;
            if (_healPoints > _healPointsMaxPoints) _healPoints = _healPointsMaxPoints;
            if (OnHealPointsChanged != null) OnHealPointsChanged(this, EventArgs.Empty);
        }

        public int GetHealMaxPoints() {
            return _healPointsMaxPoints;
        }
    }
}