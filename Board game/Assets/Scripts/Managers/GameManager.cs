using System;
using UnityEngine;

namespace Managers
{
    /**
 * This class is the one who is responsible of the game state.
 */
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameState gameState;
        private int maxAttackPerTurn = 1;
        private int maxMovesPerTurn = 2;

        private int _currentAttacks;
        private int _currentMoves;
        public bool isPaused = false;
        [SerializeField] internal BaseTower _orcTower;
        [SerializeField] internal BaseTower _angelTower;
        

        private void Awake() => Instance = this;

        public bool IsTurnOver() => !AreMovementsLeft() || !AreAttacksLeft();
        public bool AreMovementsLeft() => _currentMoves > 0;
        public bool AreAttacksLeft() => _currentAttacks > 0;
    

        private void Start() => ChangeState(GameState.GenerateGrid); 
    
        /**
     * Changes the state of the game.
     */
        internal void ChangeState(GameState newState)
        {
            gameState = newState;
            Debug.Log("GAME STATE: " + newState);
            switch (newState)
            {
                case GameState.GenerateGrid:
                    GridManager.Instance.GenerateGrid();
                    ChangeState(GameState.SpawnAngels);
                    break;
                case GameState.SpawnAngels:
                    UnitManager.Instance.SpawnAngels();
                    ChangeState(GameState.SpawnOrcs);
                    break;
                case GameState.SpawnOrcs:
                    UnitManager.Instance.SpawnOrcs();
                    ChangeState(GameState.AngelsTurn);
                    break;
                case GameState.AngelsTurn:
                    ResetTurnValues();
                    UIManager.Instance.ChangeTurn(newState);
                    CheckTowerAttack();
                    break;
                case GameState.OrcsTurn:
                    ResetTurnValues();
                    UIManager.Instance.ChangeTurn(newState);
                    CheckTowerAttack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
         
        private void ResetTurnValues(){
            _currentAttacks = maxAttackPerTurn;
            _currentMoves = maxMovesPerTurn;
            UIManager.Instance.UpdateUITurnInfo(_currentMoves, _currentAttacks);
        }
        
        private void CheckTowerAttack()
        {
            var currentFactionTurn = gameState == GameState.AngelsTurn ? Faction.Angels : Faction.Orcs;
            var attackableTilesByTower = GridManager.Instance.GetAttackableTilesByTower(currentFactionTurn);
            
            foreach (var tile in attackableTilesByTower) {
                if (tile.Value._tileUnit!=null && tile.Value._tileUnit.faction != currentFactionTurn)
                {
                    if (currentFactionTurn == Faction.Orcs) {
                        _orcTower.Attack();
                    }
                    else {
                        _angelTower.Attack();
                    }
                    return;
                }
            }
        }
        
        public void TowerAttack()
        {
            var currentFactionTurn = gameState == GameState.AngelsTurn ? Faction.Angels : Faction.Orcs;
            var attackableTilesByTower = GridManager.Instance.GetAttackableTilesByTower(currentFactionTurn);
            
            foreach (var tile in attackableTilesByTower) {
                if (tile.Value._tileUnit!=null && tile.Value._tileUnit.faction != currentFactionTurn)
                {
                    tile.Value.AttackFromTower();
                }
            }
        }

        public void SubAttackNumber() {
            _currentAttacks--;
            if (_currentAttacks < 0) _currentAttacks = 0;
            UIManager.Instance.UpdateUITurnInfo(_currentMoves, _currentAttacks);
        }
        
        public void SubMoveNumber() {
            _currentMoves--;
            if (_currentMoves < 0) _currentMoves = 0;
            UIManager.Instance.UpdateUITurnInfo(_currentMoves, _currentAttacks);
        }

        public void SkipTurn() {
            
            switch (gameState) {
                case GameState.AngelsTurn:
                    ClearMove();
                    ChangeState(GameState.OrcsTurn);
                    break;
                case GameState.OrcsTurn:
                    ClearMove();
                    ChangeState(GameState.AngelsTurn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        public static void ClearMove() {
            UnitManager.Instance.SetSelectedUnit(null);
            GridManager.Instance.HideMoves();
        }

        public void SetStatusPause(bool pauseObjectActiveSelf)
        {
            if (pauseObjectActiveSelf){
                _orcTower.StopAnimations();    
                _angelTower.StopAnimations();    
            }
            else
            {
                _orcTower.ContinueAnimations();    
                _angelTower.ContinueAnimations();    
            }
            
            isPaused = pauseObjectActiveSelf;
        }
    }
   
    public enum GameState
    {
        GenerateGrid,
        SpawnAngels,
        SpawnOrcs,
        AngelsTurn,
        OrcsTurn
    }
}