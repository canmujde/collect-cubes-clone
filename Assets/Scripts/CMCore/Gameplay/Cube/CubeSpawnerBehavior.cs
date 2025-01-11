using System.Collections;
using CMCore.Data;
using CMCore.Data.Object;
using CMCore.Interfaces;
using CMCore.Managers;
using UnityEngine;

namespace CMCore.Gameplay.Cube
{
    public class CubeSpawnerBehavior : MonoBehaviour, IResetable
    {
        [SerializeField] private CubeSpawnerProperties spawnerProperties;

        [SerializeField] private Transform spawnTransform;

        private WaitForSeconds _initWait;
        private WaitForSeconds _cycleWait;
        private WaitUntil _waitUntil;
        
        
        public void ResetBehaviour()
        {
            spawnerProperties = GameManager.Instance.LevelManager.CurrentLevelInfo.cubeSpawnerProperties;

            _initWait = new WaitForSeconds(spawnerProperties.InitSpawnDurationBetween);
            _cycleWait = new WaitForSeconds(spawnerProperties.SpawnDurationBetween);
            _waitUntil = new WaitUntil(() => GameManager.Instance.CurrentState == GameState.InGame);
            
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            for (int i = 0; i < spawnerProperties.InitSpawnCount; i++)
            {
                Spawn();
                yield return _initWait;
            }
            
            yield return _waitUntil;
            
            while (GameManager.Instance.CurrentState == GameState.InGame)
            {
                Spawn();
                yield return _cycleWait;
            }
        }

        private void Spawn()
        {
            var cubePrefabFromPool = GameManager.Instance.PoolManager.GetFromPool(0);
                
            var cubeData = cubePrefabFromPool.GetComponent<OCubeData>();
            var transformData = cubePrefabFromPool.GetComponent<OTransformData>();
            var cubeBehavior = cubePrefabFromPool.GetComponent<CubeBehavior>();
            
            
            cubeBehavior.ResetBehaviour();
            cubeData.SetData(spawnerProperties.SpawnCubeCubeData);
            transformData.SetDataAroundPos(spawnerProperties.SpawnCubeTransformData);
        }
    }
}
