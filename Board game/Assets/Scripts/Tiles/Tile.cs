using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private bool _isWalkable;
    [SerializeField] private GameObject _highlight;
    

    [FormerlySerializedAs("OccupieUnit")] public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null; 

    public virtual void Init(int x, int y) {
        
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        
        //Subtract 0.5 to get the unit centered in vertical
        var transformPosition = transform.position;
        transformPosition.y -= 0.5f;

        unit.transform.position = transformPosition;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
    
    //EVENTS

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GameState != GameState.AngelsTurn) return;

        if (OccupiedUnit != null) {
            if (OccupiedUnit.Faction == Faction.Angels) UnitManager.Instance.SetSelectedAngel((BaseAngel) OccupiedUnit);
             else if (UnitManager.Instance.selectedAngel != null) {
                var orc = (BaseOrcs) OccupiedUnit;
                Destroy(orc.gameObject);
                UnitManager.Instance.SetSelectedAngel(null);
            }
        }
        else if (UnitManager.Instance.selectedAngel != null) {
            SetUnit(UnitManager.Instance.selectedAngel);
            UnitManager.Instance.SetSelectedAngel(null);
        }
    }
}