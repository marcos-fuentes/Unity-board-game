using Tiles;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseUnit : MonoBehaviour {
    [FormerlySerializedAs("OccupiedTile")] public BaseTile occupiedBaseTile;
    public Faction faction;
    public string unitName;
    public int movementArea = 3;
}