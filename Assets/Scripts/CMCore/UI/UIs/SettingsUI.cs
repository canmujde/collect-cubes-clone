using CMCore.Data;
using CMCore.Managers;
using CMCore.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.UI.UIs
{
    public class SettingsUI : UIBase
    {
        [SerializeField] private Sprites spriteList;
        [SerializeField] private Image vibrationToggleButtonImage;
        [SerializeField] private Image sfxToggleButtonImage;
        [SerializeField] private Image musicToggleButtonImage;
        [SerializeField] private Image bgMask;

        [SerializeField] private bool isEnabled;

        public Button vibrationButton;
        public Button sfxButton;
        public Button musicButton;
        public Button[] closeButtons;

        protected override void OnShow()
        {
            base.OnShow();
            isEnabled = !isEnabled;

            vibrationToggleButtonImage.raycastTarget = isEnabled;
            sfxToggleButtonImage.raycastTarget = isEnabled;
            musicToggleButtonImage.raycastTarget = isEnabled;

            SetVibrationButton();
            SetMusicButton();
            SetSfxButton();
            Animate();
        }
        protected override void OnHide()
        {
            base.OnHide();
        }
        public void Initialize()
        {
            vibrationToggleButtonImage.raycastTarget = isEnabled;
            sfxToggleButtonImage.raycastTarget = isEnabled;
            musicToggleButtonImage.raycastTarget = isEnabled;
            SetVibrationButton();
            SetMusicButton();
            SetSfxButton();
        }
        
        
        
        public void Animate()
        {
            bgMask.DOKill();
            bgMask.DOFillAmount(isEnabled ? 1 : 0, isEnabled ? 0.5f : 0).SetEase(isEnabled ? Ease.OutBack : Ease.OutExpo);
        }

        private void SetSfxButton()
        {
            var sfxEnabled = GameManager.Instance.PlayerSettings.SfxEnabled
                ? Constants.Data.SfxEnabledStr
                : Constants.Data.SfxDisabledStr;
            sfxToggleButtonImage.transform.localScale = Vector3.one;
            sfxToggleButtonImage.sprite = spriteList.sprites.Find(x => x.spriteName == sfxEnabled).sprite;

            // sfxToggleButtonImage.SetNativeSize();
        }

        private void SetMusicButton()
        {
            var musicEnabled = GameManager.Instance.PlayerSettings.MusicEnabled
                ? Constants.Data.MusicEnabledStr
                : Constants.Data.MusicDisabledStr;
            musicToggleButtonImage.transform.localScale = Vector3.one;
            musicToggleButtonImage.sprite = spriteList.sprites.Find(x => x.spriteName == musicEnabled).sprite;
            // musicToggleButtonImage.SetNativeSize();
        }

        private void SetVibrationButton()
        {
            var vibrationsEnabled = GameManager.Instance.PlayerSettings.VibrationsEnabled
                ? Constants.Data.VibrationsEnabledStr
                : Constants.Data.VibrationsDisabledStr;
            vibrationToggleButtonImage.transform.localScale = Vector3.one;
            vibrationToggleButtonImage.sprite = spriteList.sprites.Find(x => x.spriteName == vibrationsEnabled).sprite;

            // vibrationToggleButtonImage.SetNativeSize();
        }

        public void ToggleVibration()
        {
            GameManager.Instance.PlayerSettings.ToggleVibration();
            var vibrationToggleButtonTransform = vibrationToggleButtonImage.transform;

            vibrationToggleButtonTransform.DOKill();
            vibrationToggleButtonTransform.localScale = Vector3.one;
            vibrationToggleButtonTransform.DOPunchScale(Vector3.one * 0.4f, 0.2f, 0, 1);
            SetVibrationButton();
        }

        public void ToggleMusic()
        {
            GameManager.Instance.PlayerSettings.ToggleMusic();
            var musicToggleButtonTransform = musicToggleButtonImage.transform;

            musicToggleButtonTransform.DOKill();
            musicToggleButtonTransform.localScale = Vector3.one;
            musicToggleButtonImage.transform.DOPunchScale(Vector3.one * 0.4f, 0.2f, 0, 1);
            SetMusicButton();
        }

        public void ToggleSfx()
        {
            GameManager.Instance.PlayerSettings.ToggleSfx();
            var sfxToggleButtonTransform = sfxToggleButtonImage.transform;

            sfxToggleButtonTransform.DOKill();
            sfxToggleButtonTransform.localScale = Vector3.one;
            sfxToggleButtonImage.transform.DOPunchScale(Vector3.one * 0.4f, 0.2f, 0, 1);
            SetSfxButton();
        }
    }
}