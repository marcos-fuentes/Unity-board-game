using UnityEngine;
using static Faction;
using static GameState;

public class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] internal bool _isWalkable;
    [SerializeField] private GameObject _highlight;
    [SerializeField] internal BaseUnit _tileUnit;
    
    public bool isWalkable => _isWalkable && _tileUnit == null;
    public bool isAngelSpawnable;
    public bool isOrcSpawnable;

    public virtual void Init(int x, int y)
    {
    
    }

    public void SetUnitToTile(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile._tileUnit = null;

        //Subtract 0.5 to get the unit centered in vertical
        var transformPosition = transform.position;
        transformPosition.y -= 0.5f;

        unit.transform.position = transformPosition;
        _tileUnit = unit;
        unit.OccupiedTile = this;
    }

    private void MoveUnit(BaseUnit unit)
    {
        if (unit != null && isWalkable) {
            SetUnitToTile(unit);
            GameManager.Instance.ChangeState(unit.Faction == Angels ? OrcsTurn : AngelsTurn);
            UnitManager.Instance.SetSelectedUnit(null);
        }
    }

    /**
     * Manages the turn of the unit
     * unit: pass the unit that you want to manage
     * factionTurn: the faction of the turn you are using
     */
    private void ManageUnitTurn(BaseUnit unit, Faction factionTurn)
    {
        if (unit != null && unit.Faction == factionTurn) {
            if (_tileUnit == null) {
                MoveUnit(UnitManager.Instance.selectedUnit);
            } 
            else if (_tileUnit.Faction != factionTurn) {
                Destroy(_tileUnit.gameObject);
                MoveUnit(unit);
            }
        } else {
            if (_tileUnit != null && _tileUnit.Faction == factionTurn) {
                UnitManager.Instance.SetSelectedUnit(_tileUnit);
            }
        }
    }


    //EVENTS
    
    /**
     * Enables a highlight resource to each tile when mouse is over the tile
     */
    private void OnMouseEnter() {
        _highlight.SetActive(this is GrassTile);
    }
    /**
     * Disables a highlight resource to each tile when mouse leaves the tile
     */
    private void OnMouseExit() {
        _highlight.SetActive(false);
    }
    
    /**
     * When a Tile is clicked Units turns are managed to move or destroy another unit.
     */
    private void OnMouseDown()
    {
        Debug.Log("Mouse clicked: " + TileName);
        switch (GameManager.Instance.GameState)
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