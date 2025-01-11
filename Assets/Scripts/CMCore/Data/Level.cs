using System.Collections.Generic;
using CMCore.Data.Object;
using CMCore.Gameplay.Character.AI;
using UnityEngine;

namespace CMCore.Data
{
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "CMCore/Level")]
    public class Level : ScriptableObject // A ScriptableObject that stores level data such as object data(s), level id.
    {
        public AIBehavior aiBehavior;
        public CubeSpawnerProperties cubeSpawnerProperties;
        public float duration;
        public int levelID;
        public List<ObjectData> levelData;
        public ColorPalette[] palettes;
    }


    [System.Serializable]
    public class ObjectData
    {
        public string Name => prefabID.ToString();
        
        public int prefabID;
        public TransformData transformData;
        public CubeData cubeData;
        public CollectorData collectorData;
    }
}