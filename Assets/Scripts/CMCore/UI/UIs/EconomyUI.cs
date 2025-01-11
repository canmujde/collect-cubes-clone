using System;
using System.Collections;
using System.Collections.Generic;
using CMCore.Data;
using CMCore.Managers;
using CMCore.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Sprite = UnityEngine.Sprite;

namespace CMCore.UI.UIs
{
    public class EconomyUI : UIBase
    {
        [SerializeField] private Text[] currencyTexts;
        [SerializeField] private Image[] currenyImages;
        private readonly Dictionary<CurrencyType, Text> _currencyTypesTexts = new Dictionary<CurrencyType, Text>();
        public Dictionary<CurrencyType, Image> CurrencyTypesImages = new Dictionary<CurrencyType, Image>();


        public Image[] floatingImages;

        protected override void OnShow()
        {
            base.OnShow();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        public void Initialize()
        {

            for (var i = 0; i < GameManager.Instance.EconomyManager.currencies.Length; i++)
            {
                var economyManager = GameManager.Instance.EconomyManager;
                var currencyType = GameManager.Instance.EconomyManager.currencies[i].currencyType;
                CurrencyTypesImages.Add(currencyType, currenyImages[i]);
                _currencyTypesTexts.Add(currencyType, currencyTexts[i]);

                UpdateCurrencyText(currencyType, economyManager.CurrencyAmounts[currencyType],
                    economyManager.CurrencyAmounts[currencyType]);
                UpdateCurrencyImage(currenyImages[i], GameManager.Instance.EconomyManager.currencies[i].currencySprite);
            }
        }

        private void UpdateCurrencyImage(Image image, Sprite currencySprite)
        {
            image.sprite = currencySprite;
        }

        /// <summary>
        /// Updates currency text by given currency type.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="previousAmount"></param>
        /// <param name="currencyAmount"></param>
        public void UpdateCurrencyText(CurrencyType currencyType, int previousAmount, int currencyAmount)
        {
            var displayAmount = previousAmount;


            DOTween.To(() => displayAmount, x => displayAmount = x, currencyAmount, 1)
                .OnUpdate(() => { _currencyTypesTexts[currencyType].text = displayAmount.Abbreviate(); });
        }


        private static void DisableFloatingImage(Image image)
        {
            image.gameObject.SetActive(false);
        }

        public void AnimateEarnByUI(CurrencyType currencyType, float amount, Image sellIcon)
        {
            var countOfImages = (int)amount / 100;

            if (countOfImages == 0) countOfImages = 1;

            var willGo = CurrencyTypesImages[currencyType];
            for (int i = 0; i < countOfImages; i++)
            {
                var image = Array.Find(floatingImages, image1 => image1.gameObject.activeSelf == false);

                if (image == null) continue;

                image.rectTransform.sizeDelta = willGo.rectTransform.sizeDelta;
                image.sprite = CurrencyTypesImages[currencyType].sprite;
                image.gameObject.SetActive(true);


                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    GameManager.Instance.UIManager.canvas.transform as RectTransform,
                    Input.mousePosition, GameManager.Instance.UIManager.canvas.worldCamera,
                    out var startPos);

                var randomOffset = new Vector2(Random.Range(-50, 50), +Random.Range(-50, 50));
                image.transform.position =
                    GameManager.Instance.UIManager.canvas.transform.TransformPoint(startPos + randomOffset);


                // image.rectTransform.position = Input.mousePosition;

                var i1 = i;
                image.rectTransform.DOMove(willGo.transform.position, 1f).OnComplete(() =>
                {
                    var willGoTransform = willGo.transform;
                    
                    willGoTransform.DOKill();
                    willGoTransform.localScale = Vector3.one;

                    willGo.transform.DOPunchScale(Vector3.one * 0.75f, 0.4f, 4);
                    DisableFloatingImage(image);
                    if (i1 == countOfImages - 1)
                    {
                        GameManager.Instance.AudioManager.Play("MoneyEarn");
                    }
                }).SetDelay(i * 0.05f);
            }
        }
    }
}