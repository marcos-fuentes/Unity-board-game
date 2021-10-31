using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _angelsUnits;
    private List<ScriptableUnit> _orcsUnits;
    
    [FormerlySerializedAs("selectedAngel")] [FormerlySerializedAs("SelectedUnit")] public BaseUnit selectedUnit;
    private void Awake()
    {
        Instance = this;

        _angelsUnits = Resources.LoadAll<ScriptableUnit>("Units/Angels").ToList();
        _orcsUnits = Resources.LoadAll<ScriptableUnit>("Units/Orcs").ToList();
    }

    public void SpawnAngels()
    {
        SpawnUnit(_angelsUnits);
        GameManager.Instance.ChangeState(GameState.SpawnOrcs);
    }

    

    public void SpawnOrcs()
    {
        SpawnUnit(_orcsUnits);
        GameManager.Instance.ChangeState(GameState.AngelsTurn);
    }
    
    private void SpawnUnit(List<ScriptableUnit> units) {
        foreach (var unit in units) {
            var spawnedUnit = Instantiate(unit.UnitPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile(unit.Faction);
            randomSpawnTile.SetUnitToTile(spawnedUnit);
        }
    }

    public void SetSelectedUnit(BaseUnit unit) {
        if (unit == null) {
            selectedUnit = null;
            MenuManager.Instance.ShowSelectedUnit(null);
        } else {
            selectedUnit = unit;
            MenuManager.Instance.ShowSelectedUnit(selectedUnit);
            Debug.Log("UNIT SELECTED: " + selectedUnit.UnitName);
        } 
    }
}