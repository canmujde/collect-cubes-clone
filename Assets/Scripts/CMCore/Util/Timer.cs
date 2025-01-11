using System;
using UnityEngine;

namespace CMCore.Util
{
    
    [Serializable]
    public class Timer
    {
        [field:SerializeField]public float Time { get; private set; }
        [field:SerializeField]public float ElapsedTime { get; private set; }
        private bool _isRunning;
        private Action _onTimeElapsed;

        public Timer(float time, Action onTimeElapsed)
        {
            this.Time = time;
            this._onTimeElapsed = onTimeElapsed;
            ElapsedTime = 0;
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Tick(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }

            ElapsedTime += deltaTime;
            if (ElapsedTime >= Time)
            {
                _onTimeElapsed?.Invoke();
                ElapsedTime = 0;
            }
        }
    }
}