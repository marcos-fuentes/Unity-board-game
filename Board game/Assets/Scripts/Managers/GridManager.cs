using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;
using static Faction;

/*
 * Class that is responsible of creating the Grid of tiles
 */
namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        [SerializeField] private int _width, _height;
        [SerializeField] private BaseTile grassBaseTile, mountainBaseTile;
        [SerializeField] private Transform _cam;
        private Dictionary<Vector2, BaseTile> _tiles;


        private void Awake()
        {
            Instance = this;
        }

        /**
     * Generation the grid of tiles with the vertical and horizontal size setted before
     */
        internal void GenerateGrid()
        {
            _tiles = new Dictionary<Vector2, BaseTile>();
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var randomTile = Random.Range(0, 10) == 3 ? mountainBaseTile : grassBaseTile;
                    var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                
                    spawnedTile.name = $"Tile {x} {y}";
                    AssignTileSpawnable(spawnedTile, y);
                    spawnedTile.Init(x,y);
                
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

            _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -30);
        }

        /**
     * Assigns whether a tile is spawnable or not
     */
        private void AssignTileSpawnable(BaseTile spawnedBaseTile, int y) {
            //Set tiles as spawnable when they are below the height (16 / 4 = 4 ) OR when they are the above height ( 16 - 16 / 4 = 12)
            spawnedBaseTile.isAngelSpawnable = (y < _height / 4) && spawnedBaseTile._isWalkable;
            spawnedBaseTile.isOrcSpawnable = y > _height - _height / 4 && spawnedBaseTile._isWalkable;
        }

        /**
     * Gets a random spawnable tile that has to be spawnable and walkable
     */
        public BaseTile GetSpawnTile(Faction faction) {
            return faction == Angels ? 
                _tiles.Where(tile => tile.Value.isWalkable && tile.Value.isAngelSpawnable).OrderBy(t => Random.value).First().Value 
                : _tiles.Where(tile => tile.Value.isWalkable && tile.Value.isOrcSpawnable).OrderBy(t => Random.value).First().Value;
        }
    }
}