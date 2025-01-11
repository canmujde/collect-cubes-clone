using System.Collections.Generic;
using CMCore.Data;
using CMCore.Util;
using UnityEngine;

namespace CMCore.Managers
{
    public class EconomyManager : MonoBehaviour
    {
        public Currency[] currencies;

        public readonly Dictionary<CurrencyType, int> CurrencyAmounts = new Dictionary<CurrencyType, int>();

        private GameManager _gameManager;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            
            foreach (var t in currencies)
            {
                var value = GameManager.Instance.DataManager.GetData(t.currencyType + Constants.Words.AmountWord, 0);
                
                CurrencyAmounts.Add(t.currencyType, value);
                GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(t.currencyType,value, value);
            }
        }

        /// <summary>
        /// Adds currency by given parameters like CurrencyType, Amount.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        /// <param name="updateDisplay"></param>
        public void EarnCurrency(CurrencyType currencyType, int amount,bool updateDisplay = true)
        {
            var previousAmount = CurrencyAmounts[currencyType];
            CurrencyAmounts[currencyType] += amount;
            GameManager.Instance.DataManager.SetData(currencyType + Constants.Words.AmountWord, CurrencyAmounts[currencyType]);
            if (!updateDisplay) return;
            GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(currencyType,previousAmount, CurrencyAmounts[currencyType]);
        }
        /// <summary>
        /// Removes currency by given parameters like CurrencyType, Amount.
        /// </summary>
        /// <param name="currencyType"></param>
        /// <param name="amount"></param>
        public void ForegoCurrency(CurrencyType currencyType, int amount)
        {
            var previousAmount = CurrencyAmounts[currencyType];
            CurrencyAmounts[currencyType] -= CurrencyAmounts[currencyType] - amount >= 0 ? amount : 0;
            GameManager.Instance.DataManager.SetData(currencyType + Constants.Words.AmountWord, CurrencyAmounts[currencyType]);
            GameManager.Instance.UIManager.economyUI.UpdateCurrencyText(currencyType,previousAmount, CurrencyAmounts[currencyType]);
        }

        public bool CheckForCurrency(CurrencyType currencyType, int amount)
        {
            return CurrencyAmounts[currencyType] >= amount;
        }
    }
}