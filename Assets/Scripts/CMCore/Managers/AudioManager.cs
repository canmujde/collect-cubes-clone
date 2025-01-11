using System;
using CMCore.Data;
using UnityEngine;

namespace CMCore.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sounds sounds;

        public AudioSource musicSource;
        public AudioSource[] sfxSources;

        private GameManager _gameManager;

        /// <summary>
        /// Plays sound by given parameter as Sound name.
        /// </summary>
        /// <param name="soundName"></param>
        public void Play(string soundName)
        {
            var audioP = Array.Find(sounds.audios, x => x.soundName == soundName );

            if (audioP == null) return;

            if (audioP.soundType == SoundType.Music)
            {
                musicSource.clip = audioP.clip;
                musicSource.Play();
            }
            else
            {
                if (!GameManager.Instance.PlayerSettings.SfxEnabled) return;
                foreach (var t in sfxSources)
                {
                    if (t.isPlaying) continue;
                    
                    t.clip = audioP.clip;
                    t.Play();
                    break;
                }
            }
        }

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            Play("bg");

        }
    }
}