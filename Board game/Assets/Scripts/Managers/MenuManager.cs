using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [FormerlySerializedAs("_selectedAngelObject")] [SerializeField] private GameObject _selectedUnitObject;
    [SerializeField] private GameObject _tileObject, _tileUnitObject;

    private void Awake() 
    {
        Instance = this;
    }
    
    public void ShowSelectedUnit(BaseUnit unit)
    {
        if (unit == null) {
            _selectedUnitObject.SetActive(false);
        } 
        else {
            _selectedUnitObject.GetComponentInChildren<Text>().text = unit.UnitName;
            _selectedUnitObject.SetActive(true);
        } 
    }
}