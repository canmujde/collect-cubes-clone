using System.Collections.Generic;
using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "CMCore/Color Palette", order = 1)]
    public class ColorPalette : ScriptableObject
    {
        public ColorData ColorData;
    }

    [System.Serializable]
    public class ColorData
    {
        public List<Color> colors;
    }
}