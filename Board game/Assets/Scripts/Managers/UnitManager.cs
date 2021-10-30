using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnAngels()
    {
        var angelCount = 1;

        for (int i = 0; i < angelCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseAngels>(Faction.Angels);
            var spawnedAngel = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetAngelsSpawnTile();
            
            randomSpawnTile.SetUnit(spawnedAngel);
        }
        GameManager.Instance.ChamgeState(GameState.SpawnOrcs);
    }
    
    public void SpawnOrcs()
    {
        var orcCount = 1;

        for (int i = 0; i < orcCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseOrcs>(Faction.Orcs);
            var spawnedOrc = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetOrcsSpawnTile();
            
            randomSpawnTile.SetUnit(spawnedOrc);
        }
        GameManager.Instance.ChamgeState(GameState.AngelsTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T) _units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
}