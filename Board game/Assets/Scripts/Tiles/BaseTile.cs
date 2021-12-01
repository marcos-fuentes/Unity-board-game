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
        [SerializeField] internal GameObject _highlight;
        [SerializeField] internal GameObject _possibleMove;
        [SerializeField] internal BaseUnit _tileUnit;
        
        

        public bool isAngelSpawnable;
        public bool isOrcSpawnable;
        internal int HorizontalX;
        internal int VerticalY;

        private Color _notWalkableHighlight = new Color(255, 0, 0, 0.5f);
        private Color _walkableHighlight = new Color(255, 255, 255, 0.5f);

        public virtual void Init(int x, int y) { }

        public bool IsWalkable()
        {
            return this is IWalkable iWalkable && iWalkable.IsWalkable();
        }

        private bool IsPossibleMove()  {
            return IsWalkable() && _possibleMove.activeSelf;
        }

        private void SetHighLightedTile(bool highlight)
        {
            //If it's not walkable is showing a red highlight instead of a white one
            _highlight.GetComponent<SpriteRenderer>().color =
                //Is RED when it's not walkable
                // 1 - is not possible move
                // 2 - Unit is selected to move
                UnitManager.Instance.selectedUnit != null && !IsPossibleMove() && UnitManager.Instance.selectedUnit != _tileUnit
                    ? _notWalkableHighlight : _walkableHighlight;
            
            _highlight.SetActive(highlight);
        }
        
        public void SetTileAsPossibleMovement(bool possibleMovement) {
            _possibleMove.SetActive(possibleMovement);
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
        private void MoveUnit(BaseUnit unitToMove)
        {
            if (unitToMove == null || !IsPossibleMove()) return;
            SetUnitToTile(unitToMove);
            GameManager.Instance.ChangeState(unitToMove.faction == Angels ? OrcsTurn : AngelsTurn);
            ClearMove();
        }
        
        private static void ClearMove()
        {
            UnitManager.Instance.SetSelectedUnit(null);
            GridManager.Instance.HideMoves();
        }

        /**
     * Manages the turn of the unit
     * unit: pass the unit that you want to manage
     * factionTurn: the faction of the turn you are using
     */
        private void ManageUnitTurn(BaseUnit unit, Faction factionTurn)
        {
            if (unit != null) {
                if (IsPossibleMove())
                {
                    //STEP MOVE UNIT TO TILE
                    if (_tileUnit == null) MoveUnit(unit);

                    //STEP ATTACK
                    else Attack(unit);
                }
                //CLEAR MOVE
                else ClearMove();

            } else {
                //SELECT UNIT TO MOVE
                if (_tileUnit == null || _tileUnit.faction != factionTurn) return;
                UnitManager.Instance.SetSelectedUnit(_tileUnit);
                if (UnitManager.Instance.selectedUnit != null) GridManager.Instance.ShowPossibleMoves(_tileUnit, this);
            }
        }

        private void Attack(BaseUnit unit)
        {
            Destroy(_tileUnit.gameObject);
            _tileUnit = null;
            MoveUnit(unit);
        }

        //EVENTS
    
        /**
 * Enables a highlight resource to each tile when mouse is over the tile
 */
        private void OnMouseEnter() {
            SetHighLightedTile(true);
        }
        /**
 * Disables a highlight resource to each tile when mouse leaves the tile
 */
        private void OnMouseExit() {
            SetHighLightedTile(false);
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