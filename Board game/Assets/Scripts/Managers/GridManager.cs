using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Faction;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;


    private void Awake()
    {
        Instance = this;
    }

    internal void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var randomTile = Random.Range(0, 10) == 3 ? _mountainTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                  
                spawnedTile.Init(x,y);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -30);

        GameManager.Instance.ChangeState(GameState.SpawnAngels);
    }

    public Tile GetSpawnTile(Faction faction) {
        if (faction == Angels)
        {
            return _tiles.Where(t => t.Key.y < _height / 3 && t.Value.isWalkable).OrderBy(t => Random.value).First().Value;
        } else{
            return _tiles.Where(t => t.Key.y > _height / 3 && t.Value.isWalkable).OrderBy(t => Random.value).First().Value;

        }
    }
    
    public Tile GetTileAtPosition(Vector2 pos) {
        return _tiles.TryGetValue(pos, out var tile) ? tile : null;   
    }
}