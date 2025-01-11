using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CMCore.Tweens
{
    public class ScaleOnEnable : MonoBehaviour
    {
        public float duration;
        public float delay;
        public Ease ease;
        [Header("Parameters")] public Vector3 from;
        public Vector3 to = Vector3.one;
        public UnityEvent OnComplete;

        private Transform _thisTransform;


        private void Awake()
        {
            if (!_thisTransform)
                _thisTransform = transform;
        }

        private void OnEnable()
        {
            DoScale();
        }

        private void DoScale()
        {
            var id = gameObject.name + "Scale" + Random.state;

            DOTween.Kill(id);
            _thisTransform.localScale = from;
            _thisTransform.DOScale(to, duration).SetId(id).SetEase(ease).SetDelay(delay)
                .OnComplete(() => OnComplete?.Invoke());
        }
    }
}