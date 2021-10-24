using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }
    
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var randomTile = Random.Range(0, 6) == 3 ? _mountainTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -30);
    }

    public Tile GetTileAtPosition(Vector2 pos) {
        return _tiles.TryGetValue(pos, out var tile) ? tile : null;   
    }
}

