using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private Button _skipTurnButton;
        [SerializeField] private GameObject _tileObject, _tileUnitObject;
        [SerializeField] private Image _turnImage, selectedUnitImage;
        [SerializeField] private Text _turnText, _selectedUnitNameText, _movementAreaStatText, _attackDamageStat, _movementLeftStat, _attackLeftStat;
        [SerializeField] private Sprite orcImage, ogreImage, goblinImage, angelImage, magicianImage, warriorImage;
        [SerializeField] private GameObject _countDownObject,_pauseObject;
        [SerializeField] private GameObject _selectedUnitContainer;
        
        public float timePerTurn = 30.5f;
        public float timeRemaining = 30.5f;
        public bool timerIsRunning = true;
        public Text timeText;




        void DisplayTime(float timeToDisplay) => timeText.text = string.Format("{0:00}", (int) timeToDisplay % 60);
        

        private void Awake() => Instance = this;
        
        
        void Start() {
            Button btn = _skipTurnButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            Button pauseButton = _pauseButton.GetComponent<Button>();
            pauseButton.onClick.AddListener(OnPauseClick);
        }

        private void OnPauseClick() {
            _pauseObject.SetActive(!_pauseObject.activeSelf);
            GameManager.Instance.SetStatusPause(_pauseObject.activeSelf);
        }
        
        void TaskOnClick() {
            GameManager.Instance.SkipTurn();
        }

        public void ChangeTurn(GameState gameState) {
            switch (gameState) {
                case GameState.AngelsTurn:
                    timerIsRunning = true;
                    timeRemaining = timePerTurn;
                    _turnText.text = "TURN: ANGEL";
                    _turnImage.sprite = angelImage;
                    break;
                case GameState.OrcsTurn:
                    timerIsRunning = true;
                    timeRemaining = timePerTurn;
                    _turnText.text = "TURN: ORC";
                    _turnImage.sprite = orcImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }
        }

        public void UpdateUITurnInfo(int movementLeft, int attackLeft) {
            _movementLeftStat.text = $"MOV LEFT: {movementLeft}";
            _attackLeftStat.text = $"ATTACK LEFT: {attackLeft}"; 
        }
        
        public void ShowSelectedUnit(BaseUnit unit) {
            if (unit != null) {
                var unitNameClean = unit.name.Replace("(Clone)", "");
                _selectedUnitNameText.text = $"UNIT: {unitNameClean}";
                _movementAreaStatText.text = $"MOV AREA: {unit.movementArea}"; 
                _attackDamageStat.text = $"ATTACK DMG: {unit.attackDamage}";

                switch (unitNameClean.ToLower()) {
                    case "assassin":
                        selectedUnitImage.sprite = angelImage;
                        break;
                    case "magician":
                        selectedUnitImage.sprite = magicianImage;
                        break;
                    case "warrior":
                        selectedUnitImage.sprite = warriorImage;
                        break;
                    case "goblin":
                        selectedUnitImage.sprite = goblinImage;
                        break;
                    case "ogre":
                        selectedUnitImage.sprite = ogreImage;
                        break;
                    case "orc":
                        selectedUnitImage.sprite = orcImage;
                        break;
                    default:
                        selectedUnitImage.sprite = angelImage;
                        break;
                }
                _selectedUnitContainer.SetActive(true);
                
            }
            else {
                _selectedUnitContainer.SetActive(false);
            }
            //( _selectedUnitObject.GetComponentInChildren<Text>().text = unit.unitName;
        }
        
        private void Update() {
            if (timerIsRunning) {
                if (timeRemaining > 0) {
                    timeRemaining -= (Time.deltaTime);
                    DisplayTime(timeRemaining);
                }
                else {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    GameManager.Instance.SkipTurn();
                }
            }
        }
        
    }
}