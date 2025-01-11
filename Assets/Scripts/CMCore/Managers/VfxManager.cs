using CMCore.Data.Object;
using CMCore.Util;
using UnityEngine;

namespace CMCore.Managers
{
    public class VfxManager : MonoBehaviour
    {
        public ParticleSystem confettiFx;

        private GameManager _gameManager;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            EventManager.OnGameStateChanged += OnStateChanged;
        }

        /// <summary>
        /// Plays given particle.
        /// </summary>
        /// <param name="particle"></param>
        public void Play(ParticleSystem particle)
        {
            particle.Play();
        }

        public void ResetBehaviour()
        {
            confettiFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }



        public OPrefabInfo GetParticleById(int id, float repoolAfter)
        {
            var particle = GameManager.Instance.PoolManager.GetFromPool(id);

            CoroutineHelper.DelayedAction(this, () =>
            {
                GameManager.Instance.PoolManager.RePoolObject(particle);
            },repoolAfter);
            return particle;
        }

        

        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    ResetBehaviour();
                    break;
                case GameState.InGame:
                    ResetBehaviour();
                    break;
                case GameState.Fail:
                    OnGameEndState(GameResult.Fail);
                    break;
                case GameState.Win:
                    OnGameEndState(GameResult.Win);
                    break;
            }
        }

        private void OnGameEndState(GameResult result)
        {
            switch (result)
            {
                case GameResult.Win:
                    Play(confettiFx);
                    break;
                case GameResult.Fail:

                    break;
            }
        }
    }
}