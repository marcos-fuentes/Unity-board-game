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

    public void SpawnHeroes()
    {
        var orcCount = 1;

        for (int i = 0; i < orcCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseAngels>(Faction.Angels);
            var spawnedAngel = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetAngelsSpawnTile();
            
            randomSpawnTile.SetUnit(spawnedAngel);
        }
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T) _units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
}