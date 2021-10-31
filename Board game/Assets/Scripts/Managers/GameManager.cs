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

        private void Awake() => Instance = this;
    

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
                    break;
                case GameState.OrcsTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
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