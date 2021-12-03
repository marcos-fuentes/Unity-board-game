using Managers;
using Units;
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
        [SerializeField] internal GameObject _notPossibleMoveHighLight;
        [SerializeField] internal GameObject _possibleMove;
        [SerializeField] internal GameObject _possibleMovementHighlight;
        [SerializeField] internal BaseUnit _tileUnit;

        private bool canBeAttacked;
        
        public bool isAngelSpawnable;
        public bool isOrcSpawnable;
        internal int HorizontalX;
        internal int VerticalY;

        private Color _notWalkableHighlight = new Color(255, 0, 0, 0.5f);
        private Color _walkableHighlight = new Color(255, 255, 255, 0.5f);

        private Color _possibleAttackColor = new Color(255, 0, 0, 0.7f);
        private Color _possibleMoveColor = new Color(255, 247, 0, 0.7f);

        public virtual void Init(int x, int y) { }

        public bool IsWalkable()
        {
            return this is IWalkable iWalkable && iWalkable.IsWalkable();
        }

        public bool IsOccupiedByATeamUnit(BaseUnit baseUnit) =>
            _tileUnit != null && _tileUnit.faction == baseUnit.faction;

        private bool IsPossibleMove() => IsWalkable() && _possibleMove.activeSelf;
        public bool CanBeAttacked() => IsWalkable() && _tileUnit != null;
        

        private void SetHighLightedTile()
        {
            //If it's not walkable is showing a red highlight instead of a white one
            
                //Is RED when it's not walkable
                // 1 - is not possible move
                // 2 - Unit is selected to move
                if (UnitManager.Instance.selectedUnit != null
                    && !IsPossibleMove()
                    && UnitManager.Instance.selectedUnit != _tileUnit) {
                    _notPossibleMoveHighLight.SetActive(true);
                }
                else {
                    _highlight.SetActive(true);
                }
        }

        private void ClearMouseHighLights() {
            _highlight.SetActive(false);
            _notPossibleMoveHighLight.SetActive(false);
        }
        
        public void SetTileAsPossibleMovementAttack() {
            _possibleMovementHighlight.GetComponent<SpriteRenderer>().color = _possibleAttackColor;
            canBeAttacked = true;
            SetTileAsActive(true);
        }
        
        public void SetTileAsPossibleMovement() {
            _possibleMovementHighlight.GetComponent<SpriteRenderer>().color = _possibleMoveColor;
            SetTileAsActive(true);
        }

        public void SetTileAsActive(bool isActive) {
            _possibleMove.SetActive(isActive);
            if (!isActive) canBeAttacked = false;
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
        private void MoveUnit(BaseUnit unitToMove) {
            if (!GameManager.Instance.AreMovementsLeft()) return;
            if (unitToMove == null || !IsPossibleMove()) return;
            SetUnitToTile(unitToMove);
            GameManager.Instance.SubMoveNumber();
            ChangeTurn(unitToMove);
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
        private void ManageUnitTurn(BaseUnit unitSelected, Faction factionTurn) {
            if (unitSelected != null) {
                //MOVE UNIT TO TILE
                if (IsPossibleMove() && _tileUnit == null) MoveUnit(unitSelected);

                //ATTACK
                else if (IsPossibleMove() && _tileUnit != null && _tileUnit.faction != factionTurn) Attack(unitSelected);
                
                //CLEAR MOVE
                else ClearMove();

            } else {
                //SELECT UNIT TO MOVE
                if (_tileUnit == null || _tileUnit.faction != factionTurn) return;
                UnitManager.Instance.SetSelectedUnit(_tileUnit);
                if (UnitManager.Instance.selectedUnit != null) GridManager.Instance.ShowPossibleMoves(_tileUnit, this);
            }
        }

        private void Attack(BaseUnit unit) {
            if (!canBeAttacked) return;
            unit.AttackAnimation();
            var isEnemyDead = _tileUnit.DamageUnit(unit.attackPoints);
            if (isEnemyDead) {
                Destroy(_tileUnit.gameObject);
                _tileUnit = null;
            }

            GridManager.Instance.HideMoves();
            GameManager.Instance.SubAttackNumber();
            ChangeTurn(unit);
        }

        private void ChangeTurn(BaseUnit unit) {
            ClearMove();
            var gameManager = GameManager.Instance;
            if (gameManager.IsTurnOver()) gameManager.ChangeState(unit.faction == Angels ? OrcsTurn : AngelsTurn);
        }

        //EVENTS
    
        /**
 * Enables a highlight resource to each tile when mouse is over the tile
 */
        private void OnMouseEnter() {
            SetHighLightedTile();
        }
        /**
 * Disables a highlight resource to each tile when mouse leaves the tile
 */
        private void OnMouseExit() {
            ClearMouseHighLights();
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

        public bool HasAnEnemy(BaseUnit baseUnit) => _tileUnit != null && _tileUnit.faction != baseUnit.faction;
    }
}