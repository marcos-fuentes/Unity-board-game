using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedAngelObject, _tileObject, _tileUnitObject;

    private void Awake() 
    {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile)
    {
        if (tile == null) {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            _tileObject.SetActive(true);
        }
    }

    public void ShowSelectedAngel(BaseAngel angel)
    {
        if (angel == null) {
            _selectedAngelObject.SetActive(false);
            return;
        }
        _selectedAngelObject.GetComponentInChildren<Text>().text = angel.UnitName;
        _selectedAngelObject.SetActive(true);
    }
}