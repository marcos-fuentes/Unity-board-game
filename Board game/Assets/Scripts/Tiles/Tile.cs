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
         
        transform.position = transform.position;
        OccupieUnit = unit;
        unit.OccupiedTile = this;
    }
}