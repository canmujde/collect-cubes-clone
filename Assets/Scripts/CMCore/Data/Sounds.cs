using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "CMCore/Sound Library")]
    public class Sounds : ScriptableObject
    {
        public Audio[] audios;
    }
    

    [System.Serializable]
    public class Audio
    {
        
        public AudioClip clip;
        public string soundName;
        public SoundType soundType;
    }
    public enum SoundType{Sfx,Music}
}