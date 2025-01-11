using CMCore.Managers;
using CMCore.Util;
using UnityEngine;

namespace CMCore.Data
{
    public class PlayerSettings : MonoBehaviour
    {

        private GameManager _gameManager;
        
        public bool VibrationsEnabled { get; private set; }
        public bool MusicEnabled { get; private set; }
        public bool SfxEnabled { get; private set; }

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            GetData();
        }

        /// <summary>
        /// Gets game settings from PlayerPrefs and sets it's variables.
        /// </summary>
        private void GetData()
        {
            VibrationsEnabled = GameManager.Instance.DataManager.GetData(Constants.SettingsData.VibrationsEnabledPref, 1) == 1;
            MusicEnabled = GameManager.Instance.DataManager.GetData(Constants.SettingsData.MusicEnabledPref, 1) == 1;
            SfxEnabled = GameManager.Instance.DataManager.GetData(Constants.SettingsData.SfxEnabledPref, 1) == 1;

            
            GameManager.Instance.AudioManager.musicSource.volume = MusicEnabled ? 1 : 0;
        }



        /// <summary>
        /// Toggles vibration and saves it's value.
        /// </summary>
        public void ToggleVibration()
        {
            VibrationsEnabled = !VibrationsEnabled;
            GameManager.Instance.DataManager.SetData(Constants.SettingsData.VibrationsEnabledPref, VibrationsEnabled ? 1 : 0);
        }
        
        /// <summary>
        /// Toggles music and saves it's value.
        /// </summary>
        public void ToggleMusic()
        {
            MusicEnabled = !MusicEnabled;
            GameManager.Instance.DataManager.SetData(Constants.SettingsData.MusicEnabledPref, MusicEnabled ? 1 : 0);
            GameManager.Instance.AudioManager.musicSource.volume = MusicEnabled ? 1 : 0;
        }
        
        /// <summary>
        /// Toggles sfx and saves it's value.
        /// </summary>
        public void ToggleSfx()
        {
            SfxEnabled = !SfxEnabled;
            GameManager.Instance.DataManager.SetData(Constants.SettingsData.SfxEnabledPref, SfxEnabled ? 1 : 0);
        }
    }
}
