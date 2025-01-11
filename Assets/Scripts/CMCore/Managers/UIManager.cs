using System;
using System.Collections;
using System.Collections.Generic;
using CMCore.Gameplay;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Character.AI;
using CMCore.Gameplay.Character.Player;
using CMCore.Gameplay.Cube;
using CMCore.Gameplay.Text;
using CMCore.UI;
using CMCore.UI.UIs;
using CMCore.Util;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.Managers
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// UI Fields
        /// </summary>
        public UIBase[] stateUis;

        public MenuUI menuUI;
        public InGameUI inGameUI;
        public WinUI winUI;
        public FailUI failUI;
        public EconomyUI economyUI;

        public Canvas canvas;

        public List<ScoreTextBehavior> scoreTexts;

        private GameManager _gameManager;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="gameManager"></param>
        public void Initialize(GameManager gameManager)
        {
               
            _gameManager = gameManager;
            
            failUI.Initialize();
            menuUI.Initialize();
            winUI.Initialize();
            economyUI.Initialize();
            inGameUI.Initialize();

            EventManager.OnCubeCollectedBy += OnCubeCollectedBy;
            EventManager.OnGameStateChanged += ChangeStateUI;
        }

        private void OnCubeCollectedBy(CubeBehavior cube, CharacterBase character)
        {

            if (_gameManager.LevelManager.CurrentLevelInfo.duration <= 0) return;
            var player = FindObjectOfType<PlayerMovementController>();
            var ai = FindObjectOfType<AIBehaviorController>();

            if (player == character)
            {
                var playerBelongText = scoreTexts.Find(behavior => behavior.gameObject.name == Constants.Words.PlayerScoreText);
                playerBelongText.UpdateText(_gameManager.LevelManager.PlayerScore);
            }
            else
            {
                var aiBelongText = scoreTexts.Find(behavior => behavior.gameObject.name == Constants.Words.AIScoreText);
                aiBelongText.UpdateText(_gameManager.LevelManager.AIScore);
            }
           
        }

        private void OnScoreMaded(CubeBehavior obj)
        {
            // ScoreTextBehavior.UpdateText(_gameManager.LevelManager.PlayerScore);
        }


        /// <summary>
        /// Changes UI by given GameState.
        /// </summary>
        /// <param name="state"></param>
        private void ChangeStateUI(GameState state)
        {
            if (state == GameState.InGame)
            {
                inGameUI.UpdateCurrentLevelText(GameManager.Instance.LevelManager.CurrentLevel);
                
            }
                


            if (state == GameState.Fail || state == GameState.Win)
            {
                StartCoroutine(ChangeStateUIRoutine(state));
            }
            else
            {
                
                Change(state);
            }
        }

        private IEnumerator ChangeStateUIRoutine(GameState state, float delay = 1)
        {
            yield return new WaitForSeconds(delay);
            scoreTexts = new List<ScoreTextBehavior>();
            Change(state);
        }

        private void Change(GameState state)
        {
            Array.ForEach(stateUis, x =>
            {
                if (x.state == state) x.Show();
                else x.Hide();
            });
        }
        
    }
}