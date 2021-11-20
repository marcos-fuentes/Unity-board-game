using System;
using Managers;
using UnityEngine;
using static Faction;
using static Managers.GameState;
namespace Tiles
{
    /**
     * Parent class for creation of tiles
     */
    public class BaseTile : MonoBehaviour
    {
        public string TileName;
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] internal bool _isWalkable;
        [SerializeField] internal GameObject _highlight;
        [SerializeField] internal BaseUnit _tileUnit;
        
        public bool isWalkable => _isWalkable && _tileUnit == null;
        public bool isAngelSpawnable;
        public bool isOrcSpawnable;
        internal int HorizontalX;
        internal int VerticalY;

        public virtual void Init(int x, int y) { }

        public void setHighLightedTile(bool highlight) {
            _highlight.SetActive(highlight);
        }
        
        
        /**
        * Assign a Unit to a Tile
        */
        public void SetUnitToTile(BaseUnit unit)
        {
            if (unit.occupiedBaseTile != null) unit.occupiedBaseTile._tileUnit = null;

            //Subtract 0.5 to get the unit centered in vertical
            var transformPosition = transform.position;
            transformPosition.y -= 0.5f;

            unit.transform.position = transformPosition;
            _tileUnit = unit;
            unit.occupiedBaseTile = this;
        }
        
        /**
         * Move Unit to another Tile
         */
        private void MoveUnit(BaseUnit unit)
        {
            if (unit != null) {
                SetUnitToTile(unit);
                GameManager.Instance.ChangeState(unit.faction == Angels ? OrcsTurn : AngelsTurn);
                UnitManager.Instance.SetSelectedUnit(null);
                GridManager.Instance.HideMoves();
            }
        }

        /**
     * Manages the turn of the unit
     * unit: pass the unit that you want to manage
     * factionTurn: the faction of the turn you are using
     */
        private void ManageUnitTurn(BaseUnit unit, Faction factionTurn)
        {
            if (unit != null && unit.faction == factionTurn) {
                if (_tileUnit == null) {
                    MoveUnit(UnitManager.Instance.selectedUnit);
                } 
                else if (_tileUnit.faction != factionTurn) {
                    Destroy(_tileUnit.gameObject);
                    MoveUnit(unit);
                }
            } else {
                if (_tileUnit != null && _tileUnit.faction == factionTurn) {
                    UnitManager.Instance.SetSelectedUnit(_tileUnit);
                    if (UnitManager.Instance.selectedUnit != null) GridManager.Instance.ShowPossibleMoves(_tileUnit, this);
                }
            }
        }

        //EVENTS
    
        /**
 * Enables a highlight resource to each tile when mouse is over the tile
 */
        private void OnMouseEnter() {
            setHighLightedTile(true);
        }
        /**
 * Disables a highlight resource to each tile when mouse leaves the tile
 */
        private void OnMouseExit() {
            setHighLightedTile(false);
        }
    
        /**
 * When a Tile is clicked Units turns are managed to move or destroy another unit.
 */
        private void OnMouseDown()
        {
            Debug.Log("Mouse clicked: " + TileName);
            switch (GameManager.Instance.gameState)
            {
                case AngelsTurn:
                    ManageUnitTurn(UnitManager.Instance.selectedUnit, Angels);
                    break;
                case OrcsTurn:
                    ManageUnitTurn(UnitManager.Instance.selectedUnit, Orcs);
                    break;
                case GenerateGrid:
                    break;
                case SpawnAngels:
                    break;
                case SpawnOrcs:
                    break;
            }
        }
    }
}