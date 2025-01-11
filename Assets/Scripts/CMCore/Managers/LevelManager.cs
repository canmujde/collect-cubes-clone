using System;
using System.Collections.Generic;
using System.Linq;
using CMCore.Data;
using CMCore.Data.Object;
using CMCore.Interfaces;
using CMCore.Util;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMCore.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private GameManager _gameManager;

        [field: SerializeField] public Level[] Levels { get; private set; }
        [SerializeField] private Transform levelParent;
        [field: SerializeField] public MaterialLibrary MaterialLibrary { get; private set; }
        [field: SerializeField] public Timer Timer { get; private set; }

        [Tooltip(Constants.Messages.RepeatLastLevelTooltip)] [SerializeField]
        private int repeatLastLevels;

        [SerializeField] private List<OPrefabInfo> currentObjects;

        [SerializeField] private bool manualCreation = false;
        [SerializeField] private int manualLevelID = 0;

        [ShowNativeProperty]
        public int CurrentLevelId
        {
            get => Application.isPlaying
                ? GameManager.Instance.DataManager.GetData(Constants.Data.CurrentLevelIDPref,
                    Constants.Data.CurrentLevelIDPrefDefault)
                : 0;
            private set => GameManager.Instance.DataManager.SetData(Constants.Data.CurrentLevelIDPref, value);
        }

        [ShowNativeProperty]
        public int CurrentLevel
        {
            get => Application.isPlaying
                ? GameManager.Instance.DataManager.GetData(Constants.Data.CurrentLevelPref,
                    Constants.Data.CurrentLevelPrefDefault)
                : 0;
            private set => GameManager.Instance.DataManager.SetData(Constants.Data.CurrentLevelPref, value);
        }

        [ShowNativeProperty] public Level CurrentLevelInfo { get; private set; }

        [ShowNativeProperty] public int LevelGoal { get; set; }

        [ShowNativeProperty]
        public int PlayerScore
        {
            get => _playerScore;
            set => _playerScore = value;
        }

        public int AIScore
        {
            get => _aiScore;
            set => _aiScore = value;
        }

        private void CheckPlayerReachedGoal()
        {
            if (PlayerScore >= LevelGoal)
                this.DelayedAction(() => EventManager.OnGameStateChanged?.Invoke(GameState.Win), 1);
        }


        private int _playerScore;
        private int _aiScore;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="gameManager"></param>
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;

            EventManager.OnGameStateChanged += OnStateChanged;
            EventManager.OnPool += OnPool;
            EventManager.OnPullFromPool += OnPullFromPool;
        }

        private void OnPullFromPool(OPrefabInfo prefab)
        {
            currentObjects.Add(prefab);
            prefab.transform.SetParent(levelParent);
        }

        private void OnPool(OPrefabInfo prefab)
        {
            if (currentObjects.Contains(prefab))
                currentObjects.Remove(prefab);
        }


        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    CreateLevel();
                    break;
                case GameState.InGame:
                    StartTimerIfNeeded();
                    break;
                case GameState.Fail:
                    GameManager.Instance.UIManager.failUI.UpdateLevelCompletedText(CurrentLevel);
                    break;
                case GameState.Win:
                    GameManager.Instance.UIManager.winUI.UpdateLevelCompletedText(CurrentLevel);
                    LevelSucceeded();
                    break;
            }
        }

        private void StartTimerIfNeeded()
        {
            if (CurrentLevelInfo.duration <= 0)
            {
                Timer = null;
                return;
            }


            Timer = new Timer(CurrentLevelInfo.duration, OnTimeElapsed);
            Timer.Start();
        }

        private void OnTimeElapsed()
        {
            Timer = null;
            if (CurrentLevelInfo.aiBehavior != null)
            {
                if (_playerScore > _aiScore)
                    EventManager.OnGameStateChanged?.Invoke(GameState.Win);
                else
                {
                    EventManager.OnGameStateChanged?.Invoke(GameState.Fail);
                }
            }
            else
            {
                EventManager.OnGameStateChanged?.Invoke(GameState.Win);
            }
        }

        private void Update()
        {
            if (_gameManager.CurrentState != GameState.InGame) return;
            if (Timer == null)
            {
                _gameManager.UIManager.inGameUI.UpdateTimerText(0);
                return;
            }

            TickTimer();
        }

        private void TickTimer()
        {
            Timer.Tick(Time.deltaTime);
            if (Timer == null) return;
            _gameManager.UIManager.inGameUI.UpdateTimerText(Timer.Time - Timer.ElapsedTime);
        }

        /// <summary>
        /// If level exists clears that level and prepares for new level creation with CurrentLevelID or ManualLevelID parameters.
        /// </summary>
        private void CreateLevel()
        {
            ClearLevel();
            var current = manualCreation ? GetLevelById(manualLevelID) : GetLevelByOrder(CurrentLevelId);
            if (current)
            {
                SetLevel(current);
            }
        }

        /// <summary>
        /// Sets the data by pulling the prepared level from the objects in the pool.
        /// </summary>
        /// <param name="current"></param>
        private void SetLevel(Level current)
        {
            CurrentLevelInfo = current;

            PlayerScore = 0;
            AIScore = 0;
            LevelGoal = CurrentLevelInfo.duration > 0 ? int.MaxValue : 0;

            foreach (var t in CurrentLevelInfo.levelData)
            {
                var pooledObject = GameManager.Instance.PoolManager.GetFromPool(t.prefabID);

                //lets first reset it's runtime data.
                var resettable = pooledObject.GetComponent<IResetable>();
                resettable.ResetBehaviour();

                #region Data Load

                //Load & Apply Transform data
                var transformData = pooledObject.GetComponent<OData<TransformData>>();
                if (transformData)
                    transformData.SetData(t.transformData);

                //Load & Apply cube data
                var cubeData = pooledObject.GetComponent<OData<CubeData>>();
                if (cubeData)
                    cubeData.SetData(t.cubeData);

                //Load & Apply collector data
                var collectorData = pooledObject.GetComponent<OData<CollectorData>>();
                if (collectorData)
                    collectorData.SetData(t.collectorData);

                #endregion
            }
        }


        /// <summary>
        /// Returns level by given parameter in Level Array.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Level by given parameter</returns>
        private Level GetLevelByOrder(int order)
        {
            if (order >= Levels.Length) goto restart;

            var level = Levels[order];
            if (level) return level;

            restart:
            CurrentLevelId = repeatLastLevels > 0 && repeatLastLevels < Levels.Length
                ? Levels.Length - repeatLastLevels
                : 0;
            level = Levels[CurrentLevelId];

            return level;
        }

        private Level GetLevelById(int levelId)
        {
            var level = Array.Find(Levels, matchLevel => matchLevel.levelID == levelId);

            return level;
        }

        /// <summary>
        /// Clears level completely.
        /// </summary>
        private void ClearLevel()
        {
            foreach (var t in currentObjects.ToArray())
            {
                GameManager.Instance.PoolManager.RePoolObject(t);
            }

            currentObjects.Clear();
        }

        /// <summary>
        /// Increases and sets level data to DataManager on Level Succeeded.
        /// </summary>
        private void LevelSucceeded()
        {
            manualCreation = false;
            CurrentLevel++;
            CurrentLevelId++;
        }

        public void PlaySpecificLevel(int id)
        {
            var level = Array.Find(Levels, x => id == x.levelID);

            if (level == null) return;


            manualLevelID = id;
            manualCreation = true;


            EventManager.OnGameStateChanged?.Invoke(GameState.Menu);
            EventManager.OnGameStateChanged?.Invoke(GameState.InGame);
        }

        public void PlayNormal()
        {
            manualLevelID = -1;
            manualCreation = false;
        }

        public void IncreasePlayerScore()
        {
            PlayerScore++;
            CheckPlayerReachedGoal();
        }

        public void IncreaseAIScore()
        {
            AIScore++;
        }
    }
}