using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "Currency", menuName = "CMCore/Currency")]
    public class Currency : ScriptableObject
    {
        public CurrencyType currencyType;
        public UnityEngine.Sprite currencySprite;
    }

    public enum CurrencyType
    {
        Money,
        Diamond
    }
}