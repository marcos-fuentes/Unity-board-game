using System;
using UI;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private Button _closePauseButton;
        [SerializeField] private Button _resumePauseButton;
        [SerializeField] private Button _helpPauseButton;
        [SerializeField] private Button _quitPuuseButton;
        [SerializeField] private GameObject _helpMenuObject;
        [SerializeField] private GameObject _optionsMenuObject;
        [SerializeField] private Button _helpBackButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _optionsBackButton;
        [SerializeField] private Button _showFpsEnabled;

        [SerializeField] Sprite onShowFpsSprite;
        [SerializeField] Sprite offShowFpsSprite;


        [SerializeField] private Button _skipTurnButton, _endButtonExit, _endButtonRestart;
        [SerializeField] private GameObject _tileObject, _tileUnitObject;
        [SerializeField] private Image _turnImage, selectedUnitImage;
        public GameObject _theEnd, _angelsWon, _orcsWon;

        [SerializeField] private Text _turnText,
            _selectedUnitNameText,
            _movementAreaStatText,
            _attackDamageStat,
            _movementLeftStat,
            _attackLeftStat;

        [SerializeField] private Sprite orcImage, ogreImage, goblinImage, angelImage, magicianImage, warriorImage;
        [SerializeField] private GameObject _countDownObject, _pauseObject;
        [SerializeField] private GameObject _selectedUnitContainer;


        public float timePerTurn = 30.5f;
        public float timeRemaining = 30.5f;
        public bool timerIsRunning = true;
        public Text timeText;
        private FPSCounter _fpsCounter;


        void DisplayTime(float timeToDisplay) => timeText.text = string.Format("{0:00}", (int) timeToDisplay % 60);


        private void Awake() => Instance = this;


        void Start()
        {
            Button btn = _skipTurnButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            Button pauseButton = _pauseButton.GetComponent<Button>();
            pauseButton.onClick.AddListener(OnPauseClick);

            _closePauseButton.onClick.AddListener(OnPauseClick);
            _resumePauseButton.onClick.AddListener(OnPauseClick);
            _helpPauseButton.onClick.AddListener(OnHelpButttonClicked);
            _quitPuuseButton.onClick.AddListener(OnButtonExitPressed);
            _helpBackButton.onClick.AddListener(OnBackHelpButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonPressed);
            _optionsBackButton.onClick.AddListener(OnOptionsBackButtonPressed);
            _showFpsEnabled.onClick.AddListener(OnShowFpsClicked);
            _endButtonRestart.onClick.AddListener(OnButtonRestartPressed);
            _endButtonExit.onClick.AddListener(OnButtonExitPressed);
            _fpsCounter = gameObject.AddComponent<FPSCounter>();
        }

        private bool isAnyMenuOpen() => _optionsMenuObject.activeSelf || _helpMenuObject.activeSelf;

        private void OnHelpButttonClicked()
        {
            if (!isAnyMenuOpen())
            {
                _helpMenuObject.SetActive(true);
            }
        }

        private void OnBackHelpButtonClicked()
        {
            _helpMenuObject.SetActive(false);
        }

        private void OnOptionsBackButtonPressed() => _optionsMenuObject.SetActive(false);

        private void OnOptionsButtonPressed()
        {
            if (!isAnyMenuOpen())
            {
                _optionsMenuObject.SetActive(true);
            }
        }

        private void OnButtonExitPressed()
        {
            if (!isAnyMenuOpen())
            {
                GameManager.Instance.GoToMainMenu();
            }
        }

        private void OnButtonRestartPressed() => GameManager.Instance.GoToGameScene();

        private void OnPauseClick()
        {
            if (!isAnyMenuOpen())
            {
                closeOptionsMenu();
                _pauseObject.SetActive(!_pauseObject.activeSelf);
                timerIsRunning = !timerIsRunning;
                GameManager.Instance.SetStatusPause(_pauseObject.activeSelf);
            }
        }

        private void closeOptionsMenu()
        {
            _helpMenuObject.SetActive(false);
            _optionsMenuObject.SetActive(false);
        }

        private void OnShowFpsClicked()
        {
            if (_fpsCounter.isRunning)
            {
                _showFpsEnabled.GetComponent<Image>().sprite = offShowFpsSprite;
                _fpsCounter.isRunning = false;
            }
            else
            {
                _showFpsEnabled.GetComponent<Image>().sprite = onShowFpsSprite;
                _fpsCounter.isRunning = true;
            }
        }

        void TaskOnClick()
        {
            GameManager.Instance.SkipTurn();
        }

        public void ChangeTurn(GameState gameState)
        {
            switch (gameState)
            {
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

        public void UpdateUITurnInfo(int movementLeft, int attackLeft)
        {
            _movementLeftStat.text = $"MOV LEFT: {movementLeft}";
            _attackLeftStat.text = $"ATTACK LEFT: {attackLeft}";
        }

        public void ShowSelectedUnit(BaseUnit unit)
        {
            if (unit != null)
            {
                var unitNameClean = unit.unitClass;
                _selectedUnitNameText.text = $"CLASS: {unitNameClean}";
                _movementAreaStatText.text = $"MOV AREA: {unit.movementArea}";
                _attackDamageStat.text = $"ATTACK DMG: {unit.attackDamage}";

                switch (unitNameClean)
                {
                    case Class.Assassin:
                        selectedUnitImage.sprite = angelImage;
                        break;
                    case Class.Magician:
                        selectedUnitImage.sprite = magicianImage;
                        break;
                    case Class.Warrior:
                        selectedUnitImage.sprite = warriorImage;
                        break;
                    case Class.Rogue:
                        selectedUnitImage.sprite = goblinImage;
                        break;
                    case Class.Warlock:
                        selectedUnitImage.sprite = ogreImage;
                        break;
                    case Class.Tank:
                        selectedUnitImage.sprite = orcImage;
                        break;
                    default:
                        selectedUnitImage.sprite = angelImage;
                        break;
                }

                _selectedUnitContainer.SetActive(true);
            }
            else
            {
                _selectedUnitContainer.SetActive(false);
            }
            
        }

        public void FinishGame(Faction faction)
        {
            _orcsWon.SetActive(faction == Faction.Orcs);
            _angelsWon.SetActive(faction == Faction.Angels);
            GameManager.Instance.SetStatusPause(true);
            SoundManager.Instance.PlayVictoryMusic();

            _theEnd.SetActive(true);
        }

        private void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= (Time.deltaTime);
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    GameManager.Instance.SkipTurn();
                }
            }
        }

        static public string getSystemInfo()
        {
            string str = "<color=red>SYSTEM INFO</color>";

            #if UNITY_IOS
                    str += "\n[iphone generation]"+UnityEngine.iOS.Device.generation.ToString();
            #endif

            #if UNITY_ANDROID
                        str += "\n[system info]" + SystemInfo.deviceModel;
            #endif

            str += "\n[type]" + SystemInfo.deviceType;
            str += "\n[os version]" + SystemInfo.operatingSystem;
            str += "\n[system memory size]" + SystemInfo.systemMemorySize;
            str += "\n[graphic device name]" + SystemInfo.graphicsDeviceName + " (version " +
                   SystemInfo.graphicsDeviceVersion + ")";
            str += "\n[graphic memory size]" + SystemInfo.graphicsMemorySize;
            str += "\n[platform] " + Application.platform;
            
            return str;
        }
    }
}