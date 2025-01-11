using CMCore.Data.Object;
using CMCore.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CMCore.Gameplay.Text
{
    public class DynamicText : MonoBehaviour
    {
        public OPrefabInfo prefab;
        public TextMeshPro textMeshPro;

        public Color initialTextColor;
        public Color initialOutlineBGColor;
        public Color initialBGColor;


        public SpriteRenderer outline;
        public SpriteRenderer bg;

        public void Play(string context, Color startColor, Vector3 startPos, Vector3 lookAt, float fontSize = 13)
        {
            textMeshPro.DOKill();
            transform.DOKill();

            transform.localScale = Vector3.one;

            textMeshPro.text = context;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = startColor;
            transform.position = startPos;

            var endColor = startColor;
            endColor.a = 0;

            if (lookAt != Vector3.zero)
                transform.LookAt(lookAt);

            textMeshPro.DOColor(endColor, 1).SetDelay(0.3f);
            transform.DOPunchScale(Vector3.one * 0.3f, 1, 7, 1).OnUpdate(() =>
            {
                transform.Translate(Vector3.up * Time.deltaTime * 4);
            }).OnComplete(() => GameManager.Instance.PoolManager.RePoolObject(prefab));
        }
        
        public void Play(string context, Vector3 startPos)
        {
            textMeshPro.DOKill();
            outline.DOKill();
            bg.DOKill();
            transform.DOKill();

            transform.localScale = Vector3.one*.6f;

            textMeshPro.text = context;
            transform.position = startPos;

            outline.color = initialOutlineBGColor;
            bg.color = initialBGColor;
            textMeshPro.color = initialTextColor;


            var endColorOutline = outline.color;
            endColorOutline.a = 0;
            
            var endColorBG = bg.color;
            endColorBG.a = 0;
            
            var endColorText = textMeshPro.color;
            endColorText.a = 0;


            outline.DOColor(endColorOutline, 1).SetDelay(0.3f);
            bg.DOColor(endColorBG, 1).SetDelay(0.3f);
            textMeshPro.DOColor(endColorText, 1).SetDelay(0.3f);
            
            transform.DOPunchScale(Vector3.one * 0.05f, 1, 4, 1).OnUpdate(() =>
            {
                transform.Translate(Vector3.up * Time.deltaTime * 1.5f);
            }).OnComplete(() => GameManager.Instance.PoolManager.RePoolObject(prefab));
        }
    }
}