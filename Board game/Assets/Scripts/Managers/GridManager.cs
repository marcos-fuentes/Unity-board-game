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
        [SerializeField] private BaseTile grassBaseTile, mountainBaseTile, horizontalBaseTile;
        [SerializeField] private Transform _cam;
        [SerializeField] private Transform _grid;

        private Dictionary<Vector2, BaseTile> _tiles;
        private Dictionary<Vector2, BaseTile> _tilesOrcTowerAttackable;
        private Dictionary<Vector2, BaseTile> _tilesAngelTowerAttackable;
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
            _tilesOrcTowerAttackable = new Dictionary<Vector2, BaseTile>();
            _tilesAngelTowerAttackable = new Dictionary<Vector2, BaseTile>();
            foreach (var tile in _grid.transform.GetComponentsInChildren<BaseTile>()) {
                _tiles[new Vector2(tile.position.x, tile.position.y)] = tile;
                if (tile.isOrcTowerAttackable) {
                    _tilesOrcTowerAttackable[new Vector2(tile.position.x, tile.position.y)] = tile;
                }
                else if (tile.isAngelTowerAttackable)
                {
                    _tilesAngelTowerAttackable[new Vector2(tile.position.x, tile.position.y)] = tile;
                }
            }
            
            /***
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var randomTile = Random.Range(0, 10) == 3 ? horizontalBaseTile : grassBaseTile;
                    var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);

                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.HorizontalX = x;
                    spawnedTile.VerticalY = y;
                    spawnedTile.Init(x,y);
                    AssignTileSpawnable(spawnedTile, y);

                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

            //_cam.transform.position = new Vector3((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f, -30);
            */
        }

        /**
     * Assigns whether a tile is spawnable or not
     */
        private void AssignTileSpawnable(BaseTile spawnedBaseTile, int y)
        {
            //Set tiles as spawnable when they are below the height (16 / 4 = 4 ) OR when they are the above height ( 16 - 16 / 4 = 12)
            //  spawnedBaseTile.isAngelSpawnable = y < _height / 4 && spawnedBaseTile.IsWalkable();
            // spawnedBaseTile.isOrcSpawnable = y > _height - _height / 4 && spawnedBaseTile.IsWalkable();
        }

        /**
     * Gets a random spawnable tile that has to be spawnable and walkable
     */
        public BaseTile GetSpawnTile(Faction faction)
        {
            return faction == Angels
                ? _tiles.Where(tile => tile.Value.IsSpawnable() && tile.Value.isAngelSpawnable).OrderBy(t => Random.value)
                    .First().Value
                : _tiles.Where(tile => tile.Value.IsSpawnable() && tile.Value.isOrcSpawnable).OrderBy(t => Random.value)
                    .First().Value;
        }
        
        public Dictionary<Vector2, BaseTile> GetAttackableTilesByTower(Faction faction)
        {
            return faction == Angels
                ? _tilesAngelTowerAttackable
                : _tilesOrcTowerAttackable;
        }


        internal void ShowPossibleMoves(BaseUnit baseUnit, BaseTile baseTile)
        {
            if (baseTile == null || baseUnit == null) return;
            
            var leftMovementBlocked = baseTile.isBlockedLeft;
            var rightMovementBlocked = baseTile.isBlockedRight;
            var upMovementBlocked = baseTile.isBlockedUp;
            var downMovementBlocked = baseTile.isBlockedDown;
            
            
            for (var movementLenght = 1; movementLenght <= baseUnit.movementArea; movementLenght++)
            {
                var movementLeft = baseTile.position.x - movementLenght; //Left movement cannot be less than position 0
                var movementRight = baseTile.position.x + movementLenght; //Right movement cannot be more than position width
                
                var movementDown = baseTile.position.y + movementLenght; //Down movement cannot be more than height
                var movementUp = baseTile.position.y - movementLenght; //Upper movement cannot be less than 0
                
                Debug.Log(baseTile.name + " " + "x = " + baseTile.position.x + "y= " + baseTile.position.y);
                Debug.Log("up: " + movementUp + "down: " + movementDown + "right: " + movementRight + "left: " + movementLeft);
                
                var isInsideAttackArea = movementLenght <= baseUnit.attackArea;
                
                //Check if there's any tile that shouldn't be walkable so in that case there won't be more movement to that direction
                if (movementLeft < 0) leftMovementBlocked = true;
                if (!leftMovementBlocked) {
                    var leftTile = _tiles[new Vector2(movementLeft, baseTile.position.y)];
                    leftMovementBlocked = !CheckPossibleActions(leftTile, baseUnit, isInsideAttackArea) || leftTile.isBlockedLeft;
                }
                
                if (movementRight > _width - 1) rightMovementBlocked = true;
                if (!rightMovementBlocked) {
                    var rightTile = _tiles[new Vector2(movementRight, baseTile.position.y)];
                    rightMovementBlocked = !CheckPossibleActions(rightTile, baseUnit, isInsideAttackArea) || rightTile.isBlockedRight;
                }
                
                if (movementUp < 0) upMovementBlocked = true;
                if (!upMovementBlocked) {
                    var upperTile = _tiles[new Vector2(baseTile.position.x, movementUp)];
                    upMovementBlocked = !CheckPossibleActions(upperTile, baseUnit, isInsideAttackArea) ||
                                        upperTile.isBlockedUp;
                }
                
                if (movementDown > _height - 1) downMovementBlocked = true;
                if (!downMovementBlocked) {
                    var bottomTile = _tiles[new Vector2(baseTile.position.x, movementDown)];
                    downMovementBlocked = !CheckPossibleActions(bottomTile, baseUnit, isInsideAttackArea) || bottomTile.isBlockedDown;
                }
            }
        }

        //It returns if the movement in that direction should be blocked
        private bool CheckPossibleActions(BaseTile tile, BaseUnit selectedUnit, bool isInsideAttackArea) {
            //Check if the tile to move it's walkable
            if (!tile.IsWalkable() || tile.IsTileOccupiedByUnitSelected(selectedUnit)) return false;

            //If there's a unit from the same team it's also a blocker
            if (tile.IsOccupiedByATeamUnit(selectedUnit)) {
                //When its magician we set the tile to be possible to heal
                if (selectedUnit.unitClass == Class.Magician 
                    && tile.UnitCanBeHealed()
                    && selectedUnit.HealSystem.GetHealPoints() > 0
                ) {
                    tile.SetTileAsPossibleMovementHeal();
                    tilesHighlighted.Add(tile);    
                }
                return false;
            }
            //Check if there's a tower to attack
            if (tile.IsOcuppiedByATower()) {
                Debug.Log("IS OCCUPIED BY A TOWER");
                //When its magician we set the tile to be possible to heal
                if (selectedUnit.unitClass == Class.OrcRepair 
                    && tile.TowerCanBeRepaired()
                    && selectedUnit.HealSystem.GetHealPoints() > 0
                   ) {
                    tile.SetTileAsPossibleMovementTowerRepaier();
                    tilesHighlighted.Add(tile);    
                } else if (isInsideAttackArea) {
                    Debug.Log("IS TOWER ATTACKABLE");
                    tile.SetTileAsPossibleMovementTowerAttack();
                    tilesHighlighted.Add(tile);
                }
                return false;
            }

            if (isInsideAttackArea) {
                tile.SetTileAsPossibleMovementAttack();
                tilesHighlighted.Add(tile);
            }
            else if (!tile.HasAnEnemy(selectedUnit)) {
                tile.SetTileAsPossibleMovement();
                tilesHighlighted.Add(tile);
            }
            //If there's an enemy in the movement area, it's also a possible move but it's a block (no more moves in that direction)
            return !tile.HasAnEnemy(selectedUnit);
        }

        internal void HideMoves()
        {
            foreach (var baseTile in tilesHighlighted)
            {
                baseTile.SetTileAsActive(false);
            }
        }
    }
}