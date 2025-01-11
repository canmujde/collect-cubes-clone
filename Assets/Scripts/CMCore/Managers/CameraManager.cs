using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CMCore.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Camera gameCamera;
        public Camera uiCamera;

        private Coroutine _showRoutine;
        [SerializeField] private Transform gameCameraTransform;
        [SerializeField] private Transform uiCameraTransform;
        private Transform _targetTransform;
        private Vector3 _currentOffset;

        [SerializeField] private float smoothFactor;
        [SerializeField] private bool moveX;
        public List<QueuedShow> queuedShows;

        private void Awake()
        {
            queuedShows = new List<QueuedShow>();
            gameCameraTransform = gameCamera.gameObject.transform;
            uiCameraTransform = uiCamera.gameObject.transform;
        }

        public void SetTarget(Transform target, Vector3 offset, bool hard)
        {
            _targetTransform = target;
            _currentOffset = offset;
            if (hard)
                gameCameraTransform.position = _targetTransform.position + _currentOffset;
        }

        public void SetPosition(Vector3 pos)
        {
            _targetTransform = null;


            gameCameraTransform.position = pos;
        }

        private void LateUpdate()
        {
            if (!_targetTransform) return;

            Move();
        }

        private void Move()
        {
            var to = _targetTransform.position + _currentOffset;
            if (!moveX)
                to.x = 0;
            gameCameraTransform.position =
                Vector3.Lerp(gameCameraTransform.position, to, smoothFactor * Time.deltaTime);
        }

        public void Show()
        {
            if (_showRoutine != null)
                StopCoroutine(_showRoutine);

            _showRoutine = StartCoroutine(ShowSomething());
        }

        private IEnumerator ShowSomething()
        {
            Show:
            var queuedShow = queuedShows[0];

            queuedShows.Remove(queuedShow);

            SetTarget(queuedShow.t, queuedShow.offset, false);
            smoothFactor = queuedShow.smoothSpeed;
            yield return new WaitForSeconds(queuedShow.duration);
            if (queuedShows.Any())
                goto Show;


            yield return null;
            //SetTarget(GameManager.Instance.LevelManager.CurrentPlayer.transform, false);
        }
    }

    public static class Offsets
    {
        public static Vector3 RunOffset = new Vector3(0, 25, -15);
        public static Vector3 ObjectFocusOffset = new Vector3(0, 3.59f, -5f);
    }

    [System.Serializable]
    public struct QueuedShow
    {
        public Transform t;
        public Vector3 offset;
        public float duration;
        public float smoothSpeed;

        public QueuedShow(Transform t, Vector3 offset, float duration, float smoothSpeed)
        {
            this.t = t;
            this.offset = offset;
            this.duration = duration;
            this.smoothSpeed = smoothSpeed;
        }
    }
}