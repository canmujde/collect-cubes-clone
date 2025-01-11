using System.Collections.Generic;
using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "Sprites", menuName = "CMCore/Sprite Library")]
    public class Sprites : ScriptableObject
    {
        public List<Sprite> sprites;
    }
    
    [System.Serializable]
    public class Sprite
    {
        public UnityEngine.Sprite  sprite;
        public string spriteName;
    }
}
