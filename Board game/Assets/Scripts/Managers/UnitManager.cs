using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Managers
{
    /**
     * Class responsible of creating and Spawning units
     */
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance;

        private List<ScriptableUnit> _angelsUnits;
        private List<ScriptableUnit> _orcsUnits;
        
        public BaseUnit selectedUnit;
        
        private async void Awake() {
            Instance = this;
            
            //Get the all the units from the resources
            _angelsUnits = Resources.LoadAll<ScriptableUnit>("Units/Angels").ToList();
            _orcsUnits = Resources.LoadAll<ScriptableUnit>("Units/Orcs").ToList();
        }

        /**
         * Method to spawn angels
         */
        internal void SpawnAngels() {
            foreach (var unit in _angelsUnits) {
                SpawnUnit(unit);
            } 
        }

        /**
         * Function to spawn orcs
         */
        internal void SpawnOrcs() {
            foreach (var unit in _orcsUnits) {
                SpawnUnit(unit);
            } 
        }
        
        private static void SpawnUnit(ScriptableUnit unit) {
            var spawnedUnit = Instantiate(unit.UnitPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile(unit.Faction);
            randomSpawnTile.SetUnitToTile(spawnedUnit);
        }

        /**
         * Selects a Unit, if unit is null it deselects the unit.
         */
        public void SetSelectedUnit(BaseUnit unit) {
            if (unit == null) {
                selectedUnit = null;
                UIManager.Instance.ShowSelectedUnit(null);
            } else {
                selectedUnit = unit;
                UIManager.Instance.ShowSelectedUnit(selectedUnit);
                Debug.Log("UNIT SELECTED: " + selectedUnit.unitName);
            } 
        }
    }
}