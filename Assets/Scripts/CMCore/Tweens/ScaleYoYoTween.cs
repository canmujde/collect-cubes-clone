using DG.Tweening;
using UnityEngine;

namespace CMCore.Tweens
{
    public class ScaleYoYoTween : MonoBehaviour
    {
        public Ease ease;

        private void OnEnable()
        {
            var t = transform;
            t.DOKill();
            t.localScale = Vector3.one;
            StartTween();
        }

        private void StartTween()
        {
            transform.DOScale(Vector3.one * 1.2f, 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        }
    }
}