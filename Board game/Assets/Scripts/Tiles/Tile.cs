using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupieUnit;
    public bool Walkable => _isWalkable && OccupieUnit == null; 

    public virtual void Init(int x, int y) {
        
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupieUnit = null;
        
        //Subtract 0.5 to get the unit centered in vertical
        var transformPosition = transform.position;
        transformPosition.y -= 0.5f;

        unit.transform.position = transformPosition;
        OccupieUnit = unit;
        unit.OccupiedTile = this;
    }
}