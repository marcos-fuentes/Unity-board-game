using System.Collections.Generic;
using System.Linq;
using Tiles;
using Units;
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
        private List<BaseTile> tilesHighlighted = new List<BaseTile>();


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
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var randomTile = Random.Range(0, 10) == 3 ? mountainBaseTile : grassBaseTile;
                    var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);

                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.HorizontalX = x;
                    spawnedTile.VerticalY = y;
                    spawnedTile.Init(x,y);
                    AssignTileSpawnable(spawnedTile, y);

                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

            _cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -30);
        }

        /**
     * Assigns whether a tile is spawnable or not
     */
        private void AssignTileSpawnable(BaseTile spawnedBaseTile, int y)
        {
            //Set tiles as spawnable when they are below the height (16 / 4 = 4 ) OR when they are the above height ( 16 - 16 / 4 = 12)
            spawnedBaseTile.isAngelSpawnable = y < _height / 4 && spawnedBaseTile.IsWalkable();
            spawnedBaseTile.isOrcSpawnable = y > _height - _height / 4 && spawnedBaseTile.IsWalkable();
        }

        /**
     * Gets a random spawnable tile that has to be spawnable and walkable
     */
        public BaseTile GetSpawnTile(Faction faction)
        {
            return faction == Angels
                ? _tiles.Where(tile => tile.Value.IsWalkable() && tile.Value.isAngelSpawnable).OrderBy(t => Random.value)
                    .First().Value
                : _tiles.Where(tile => tile.Value.IsWalkable() && tile.Value.isOrcSpawnable).OrderBy(t => Random.value)
                    .First().Value;
        }


        internal void ShowPossibleMoves(BaseUnit baseUnit, BaseTile baseTile)
        {
            if (baseTile == null || baseUnit == null) return;
            
            var leftMovementBlocked = false;
            var rightMovementBlocked = false;
            var upMovementBlocked = false;
            var downMovementBlocked = false;
            
            
            for (var movementLenght = 1; movementLenght <= baseUnit.movementArea; movementLenght++)
            {
                var movementLeft = (baseTile.HorizontalX - movementLenght < 0) ? 0 : baseTile.HorizontalX - movementLenght; //Left movement cannot be less than position 0
                var movementRight = (baseTile.HorizontalX + movementLenght > _width - 1) ? _width - 1 : baseTile.HorizontalX + movementLenght; //Right movement cannot be more than position width
                
                var movementUp = (baseTile.VerticalY + movementLenght > _height - 1) ? _height - 1 : baseTile.VerticalY + movementLenght; //Upper movement cannot be more than height
                var movementDown = (baseTile.VerticalY - movementLenght < 0) ? 0 : baseTile.VerticalY - movementLenght; //Right movement cannot be less than 0

                var leftTile = _tiles[new Vector2(movementLeft, baseTile.VerticalY)];
                var rightTile = _tiles[new Vector2(movementRight, baseTile.VerticalY)];
                var upperTile = _tiles[new Vector2(baseTile.HorizontalX, movementUp)];
                var bottomTile = _tiles[new Vector2(baseTile.HorizontalX, movementDown)];

                //Check if there's any tile that shouldn't be walkable so in that case there won't be more movement to that direction
                if (!leftMovementBlocked) leftMovementBlocked = !CheckIfTileIsWalkable(leftTile);
                if (!rightMovementBlocked) rightMovementBlocked = !CheckIfTileIsWalkable(rightTile);
                if (!upMovementBlocked) upMovementBlocked = !CheckIfTileIsWalkable(upperTile);
                if (!downMovementBlocked) downMovementBlocked = !CheckIfTileIsWalkable(bottomTile);
            }
        }

        private bool CheckIfTileIsWalkable(BaseTile tile)
        {
            if (tile.IsWalkable())
            {
                tile.SetTileAsPossibleMovement(true);
                tilesHighlighted.Add(tile);
                return true;
            }
            return false;
        }

        internal void HideMoves()
        {
            foreach (var baseTile in tilesHighlighted)
            {
                baseTile.SetTileAsPossibleMovement(false);
            }
        }
    }
}