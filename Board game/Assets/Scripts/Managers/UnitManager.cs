using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _angelsUnits;
    private List<ScriptableUnit> _orcsUnits;

    [FormerlySerializedAs("SelectedAngel")] [FormerlySerializedAs("selectedAngels")] public BaseAngel selectedAngel;

    private void Awake()
    {
        Instance = this;

        _angelsUnits = Resources.LoadAll<ScriptableUnit>("Units/Angels").ToList();
        _orcsUnits = Resources.LoadAll<ScriptableUnit>("Units/Orcs").ToList();
    }

    public void SpawnAngels()
    {
        SpawnUnit(_angelsUnits);
        GameManager.Instance.ChamgeState(GameState.SpawnOrcs);
    }

    

    public void SpawnOrcs()
    {
        SpawnUnit(_orcsUnits);
        GameManager.Instance.ChamgeState(GameState.AngelsTurn);
    }
    
    private void SpawnUnit(List<ScriptableUnit> units)
    {
        foreach (var unit in units)
        {
            var spawnedUnit = Instantiate(unit.UnitPrefab);
            var randomSpawnTile = GridManager.Instance.GetSpawnTile(unit.Faction);
            randomSpawnTile.SetUnit(spawnedUnit);
        }
    }

    public void SetSelectedAngel(BaseAngel selectedAngel)
    {
        this.selectedAngel = selectedAngel;
        MenuManager.Instance.ShowSelectedAngel(selectedAngel);
    }
}