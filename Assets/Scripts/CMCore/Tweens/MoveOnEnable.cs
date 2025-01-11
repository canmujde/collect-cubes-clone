using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CMCore.Tweens
{
    public class MoveOnEnable : MonoBehaviour
    {
        public float duration;
        public Ease ease;
        [Header("Parameters")] 
        public Vector3 from;
        private Vector3 _to;
        public UnityEvent OnComplete;

        private Transform _thisTransform;

        private void Awake()
        {
            if (_thisTransform) return;
        
        
            _thisTransform = transform;
            _to = _thisTransform.localPosition;

        }

        private void OnEnable()
        {
            DoMove();
        }

        private void DoMove()
        {

            var id = gameObject.name + "Move";

            DOTween.Kill(id);
            _thisTransform.localPosition = from;
            _thisTransform.DOLocalMove(_to, duration).SetId(id).SetEase(ease).OnComplete(() => OnComplete?.Invoke());
        }
    }
}
