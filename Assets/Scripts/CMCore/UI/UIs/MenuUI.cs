using CMCore.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CMCore.UI.UIs
{
    public class MenuUI : UIBase
    {

        public SettingsUI settingsUI;

        [SerializeField] private EventTrigger startButtonEventTrigger;
        [SerializeField] private Button settingsButton;
        
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
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener( ( data ) => { PlayButton_OnClick(); } );
            startButtonEventTrigger.triggers.Add( entry );

            settingsButton.onClick.AddListener(EnableSettingsPanel);

            foreach (var closeButton in settingsUI.closeButtons)
            {
                closeButton.onClick.AddListener(DisableSettingsPanel);
            }
            
            settingsUI.vibrationButton.onClick.AddListener(settingsUI.ToggleVibration);
            settingsUI.sfxButton.onClick.AddListener(settingsUI.ToggleSfx);
            settingsUI.musicButton.onClick.AddListener(settingsUI.ToggleMusic);

            settingsUI.Initialize();

        }

        
        /// <summary>
        /// Play Button OnClick.
        /// </summary>
        private static void PlayButton_OnClick()
        {
            EventManager.OnGameStateChanged?.Invoke(GameState.InGame);
        }
        
        [Button]
        private void EnableSettingsPanel()
        {
            settingsUI.Show();
            
        }

        [Button]
        public void DisableSettingsPanel()
        {
            settingsUI.Hide();
            
        }
    }
}
