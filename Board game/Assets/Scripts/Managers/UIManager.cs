using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject _selectedUnitObject;
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
                _selectedUnitObject.GetComponentInChildren<Text>().text = unit.unitName;
                _selectedUnitObject.SetActive(true);
            } 
        }
    }
}