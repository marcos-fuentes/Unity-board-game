using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChamgeState(GameState.GenerateGrid);
    }

    internal void ChamgeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnAngels:
                UnitManager.Instance.SpawnAngels(); 
                break;
            case GameState.SpawnOrcs:
                UnitManager.Instance.SpawnOrcs();
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