using CMCore.Managers;
using CMCore.Util;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.UI.UIs
{
    public class FailUI : UIBase
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Text levelFailedText;

        protected override void OnShow()
        {
            base.OnShow();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        public void Initialize()
        {
            retryButton.onClick.AddListener(RetryButton_OnClick);
        }

        public void UpdateLevelCompletedText(int level)
        {
            levelFailedText.text = "LEVEL " + level +"\n YOU FAILED";
        }

        private void RetryButton_OnClick()
        {
            EventManager.OnGameStateChanged?.Invoke(GameState.Menu);
        }
    }
}