using CMCore.Data.Object;
using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "Cube Spawner Property", menuName = "CMCore/Cube Spawner Property")]
    public class CubeSpawnerProperties : ScriptableObject
    {
     
        
         [field: SerializeField] public int InitSpawnCount { get; private set; }
         [field: SerializeField] public float InitSpawnDurationBetween { get; private set;}
         
         
         
         [field: SerializeField] public float SpawnDurationBetween { get; private set;}
         [field: SerializeField] public TransformData SpawnCubeTransformData { get; private set;}
         [field: SerializeField] public CubeData SpawnCubeCubeData { get; private set;}
         

    }
}
