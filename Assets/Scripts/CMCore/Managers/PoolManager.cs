using System.Collections.Generic;
using CMCore.Data;
using CMCore.Data.Object;
using UnityEngine;

namespace CMCore.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public GamePrefabsData prefabData;

        [SerializeField] private Transform poolParent;
        [SerializeField] private int extendSize;
        public List<OPrefabInfo> pooledObjects;

        private GameManager _gameManager;
        
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="gameManager"></param>
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            
            EventManager.OnPool += OnPool;
            EventManager.OnPullFromPool += OnPullFromPool;
            
            CreatePool();
        }

        private void OnPullFromPool(OPrefabInfo prefab)
        {
            pooledObjects.Remove(prefab);
            prefab.gameObject.SetActive(true);
            
        }

        private void OnPool(OPrefabInfo prefab)
        {
            pooledObjects.Add(prefab);
            prefab.transform.SetParent(poolParent);
            prefab.gameObject.SetActive(false);
        }


        /// <summary>
        /// First pool creation.
        /// </summary>
        private void CreatePool()
        {
            foreach (var pData in prefabData.PrefabDataList)
            {
                for (int i = 0; i < pData.size; i++)
                {
                    var obj = Instantiate(pData.prefab.gameObject, poolParent);
                    var prefabInfo = obj.GetComponent<OPrefabInfo>();
                    obj.SetActive(false);
                    obj.name = pData.prefab.name;
                    pooledObjects.Add(prefabInfo);
                }
            }
        }

        /// <summary>
        /// Repools given GameObject(OPrefabInfo).
        /// </summary>
        /// <param name="prefab"></param>
        public void RePoolObject(OPrefabInfo prefab)
        {
            EventManager.OnPool?.Invoke(prefab);
        }
        
        /// <summary>
        /// Returns GameObject(OPrefabInfo) from pool if exists. Otherwise, extends pool with given id and returns first element.
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns>OPrefabInfo</returns>
        public OPrefabInfo GetFromPool(int objectID)
        {
            var oPrefabInfo = pooledObjects.Find(obj => obj.prefabID == objectID);

            if (oPrefabInfo != null)
            {
                EventManager.OnPullFromPool?.Invoke(oPrefabInfo);
                return oPrefabInfo;
            }
            //increase pool size & return instantiated first Object.
            // Debug.Log($"No object in pool with id : {objectID}. Extending pool by {extendSize}.");

            var data = prefabData.PrefabDataList.Find(pData => pData.id == objectID);

            for (int i = 0; i < extendSize; i++)
            {
                var obj = Instantiate(data.prefab, poolParent);
                var prefabInfo = obj.GetComponent<OPrefabInfo>();
                obj.gameObject.name = data.prefab.name;
                obj.gameObject.SetActive(false);
                pooledObjects.Add(prefabInfo);
            }

            return GetFromPool(objectID);
        }
    }
}