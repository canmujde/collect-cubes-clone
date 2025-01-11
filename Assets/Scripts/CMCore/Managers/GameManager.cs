using System.Threading.Tasks;
using CMCore.Data;
using CMCore.Util;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using Constants = CMCore.Util.Constants;

namespace CMCore.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //Storage of current state of game
        public GameState CurrentState { get; private set; }

        #region References

        public UIManager UIManager { get; private set; }
        public LevelManager LevelManager { get; private set; }
        public PoolManager PoolManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public PlayerSettings PlayerSettings { get; private set; }
        public EconomyManager EconomyManager { get; private set; }
        public DataManager DataManager { get; private set; }
        public VfxManager VfxManager { get; private set; }
        public CameraManager CameraManager { get; private set; }
        public InputManager InputManager { get; private set; }

        [SerializeField] private bool logStateChanges;

        #endregion
        
        #region Unity

        private void Awake()
        {
            Instance = this;
            EventManager.OnGameStateChanged += OnStateChanged;

            DOTween.SetTweensCapacity(1000, 1000);
           
            InputManager = GetComponent<InputManager>();
            UIManager = GetComponent<UIManager>();
            LevelManager = GetComponent<LevelManager>();
            PoolManager = GetComponent<PoolManager>();
            AudioManager = GetComponent<AudioManager>();
            PlayerSettings = GetComponent<PlayerSettings>();
            EconomyManager = GetComponent<EconomyManager>();
            DataManager = GetComponent<DataManager>();
            VfxManager = GetComponent<VfxManager>();
            CameraManager = GetComponent<CameraManager>();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            Input.multiTouchEnabled = false;

            Initialize();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Manipulation();
#endif
        }

        #endregion

        /// <summary>
        /// Initialization
        /// </summary>
        private void Initialize()
        {

            
            DataManager.Initialize(this);
            EconomyManager.Initialize(this);
            PlayerSettings.Initialize(this);
            AudioManager.Initialize(this);
            
            PoolManager.Initialize(this);
            LevelManager.Initialize(this);
            VfxManager.Initialize(this);
            InputManager.Initialize(this);
            UIManager.Initialize(this);

            
                
            EventManager.OnGameStateChanged?.Invoke(GameState.Menu);
            // EventManager.OnGameStateChanged?.Invoke(GameState.InGame);
        }

        /// <summary>
        /// Manipulates game such as Winning/Failing Game, changing TimeScale, increasing/decreasing currency for testing purposes.
        /// </summary>
        private void Manipulation()
        {
            #region State Manipulation

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (CurrentState == GameState.InGame)
                    EventManager.OnGameStateChanged?.Invoke(GameState.Win);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (CurrentState == GameState.InGame)
                    EventManager.OnGameStateChanged?.Invoke(GameState.Fail);
                
            }

            #endregion

            #region Time Manipulation

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3;
            }

            #endregion

            #region Economy Manipulation

            if (Input.GetKeyDown(KeyCode.P))
            {
                EconomyManager.EarnCurrency(CurrencyType.Money, 100);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                EconomyManager.ForegoCurrency(CurrencyType.Money, 100);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                EconomyManager.EarnCurrency(CurrencyType.Diamond, 100);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                EconomyManager.ForegoCurrency(CurrencyType.Diamond, 100);
            }
            

            #endregion
        }
        
        /// <summary>
        /// State change event
        /// </summary>
        /// <param name="state"></param>
        private void OnStateChanged(GameState state)
        {
            CurrentState = state;

            if (logStateChanges)
                Debug.Log(Constants.Messages.OnStateChangedMessage + "<color=#26803e>" + state + "</color>");
        }
    }


    public enum GameState
    {
        Menu,
        InGame,
        Fail,
        Win
    }

    public enum GameResult
    {
        Win,
        Fail
    }
}