using System;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Cube;
using CMCore.Managers;
using CMCore.Util;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.UI.UIs
{
    public class InGameUI : UIBase
    {
        public Button restartButton;

        [SerializeField] private Text currentLevelText;

        [SerializeField] private Slider levelProgressSlider;


        [SerializeField] private Text timerText;
        [SerializeField] private GameObject timerPanel;

        protected override void OnShow()
        {
            base.OnShow();

            ToggleTimerPanel(GameManager.Instance.LevelManager.CurrentLevelInfo.duration > 0);
        }

        private void ToggleTimerPanel(bool enable)
        {
            timerPanel.SetActive(enable);
        }

        protected override void OnHide()
        {
            base.OnHide();
            UpdateLevelProgressBar(0, 1);
        }

        public void Initialize()
        {
            restartButton.onClick.AddListener(RestartButton_OnClick);
            EventManager.OnCubeCollectedBy += OnCubeCollectedBy;
        }

        private void OnCubeCollectedBy(CubeBehavior arg1, CharacterBase arg2)
        {
            
            UpdateLevelProgressBar(GameManager.Instance.LevelManager.PlayerScore,
                GameManager.Instance.LevelManager.LevelGoal);
        }

        public void UpdateTimerText(float timeLeft)
        {
            var msm = timeLeft.GetMinutesSecondsMilliseconds();

            timerText.text = $"{msm.Item1:00}:{msm.Item2:00}:{msm.Item3/10:00}";
        }

        private void UpdateLevelProgressBar(CubeBehavior obj)
        {
            UpdateLevelProgressBar(GameManager.Instance.LevelManager.PlayerScore,
                GameManager.Instance.LevelManager.LevelGoal);
        }

        private void UpdateLevelProgressBar(int current, int max)
        {
            levelProgressSlider.value = (float)current / max;
        }

        public void UpdateCurrentLevelText(int level)
        {
            currentLevelText.text = "LEVEL " + level;
        }

        private void RestartButton_OnClick()
        {
            restartButton.interactable = false;
            CoroutineHelper.DelayedAction(GameManager.Instance, () => { restartButton.interactable = true; }, 0.5f);
            EventManager.OnGameStateChanged?.Invoke(GameState.Menu);
            EventManager.OnGameStateChanged?.Invoke(GameState.InGame);
        }
    }
}