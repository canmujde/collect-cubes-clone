using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CMCore.Util
{
    public static class AbbreviationUtility
    {
        private static readonly SortedDictionary<int, string> Abbreviations = new SortedDictionary<int, string>()
        {
            { 1000, "K" },
            { 1000000, "M" },
            { 1000000000, "B" },
            // { 1000000000000, "T" },
            // { 1000000000000000, "Q" },
            // { 1000000000000000000, "QT" },
        };

        public static string Abbreviate(this int value)
        {
            for (int i = Abbreviations.Count - 1; i >= 0; i--)
            {
                var pair = Abbreviations.ElementAt(i);
                if (value < pair.Key) continue;


                var roundedNumber = (float)value / (float)pair.Key;
            
                if (roundedNumber - Mathf.Floor(roundedNumber)!=0)
                    return roundedNumber.ToString("0.0") + pair.Value;
            
            
                return roundedNumber.ToString("F0") + pair.Value;
            }

            return value.ToString();
        }
    }
}