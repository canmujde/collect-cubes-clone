using System.Collections.Generic;
using CMCore.Data;
using CMCore.Managers;
using CMCore.Util;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sprite = UnityEngine.Sprite;

namespace CMCore.UI.UIs
{
    public class WinUI : UIBase
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Text levelCompletedText;

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
            nextButton.onClick.AddListener(NextButton_OnClick);
        }

        public void UpdateLevelCompletedText(int level)
        {
            levelCompletedText.text = "LEVEL " + level +"\nCOMPLETE!";
        }

        private void NextButton_OnClick()
        {
            EventManager.OnGameStateChanged?.Invoke(GameState.Menu);
        }
    }
}