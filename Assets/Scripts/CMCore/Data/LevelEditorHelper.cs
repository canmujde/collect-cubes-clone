using System;
using System.Collections.Generic;
using System.Linq;
using CMCore.Data.Object;
using CMCore.Managers;
using CMCore.Util;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace CMCore.Data
{
    public class LevelEditorHelper : MonoBehaviour
    {

        public int levelID;
        public LevelManager levelManager;
        public PoolManager poolManager;
        public List<Transform> currentObjects;
        public bool playLevelOnEnteredPlayMode;

        
        
        [Button()]
          void SaveLevel()
         {
             var saveObjects = FindObjectsOfType<OPrefabInfo>();
             if (saveObjects.Length<=0)
             {
                 Debug.LogWarning(Constants.Messages.NoPrefabInSceneMessage);
                 return;
             }
             var asset = levelManager.Levels.FirstOrDefault(lstLevel => lstLevel.levelID == levelID);
        
             var newObject = false;
             if (asset == null)
             {
                 newObject = true;
                 asset = ScriptableObject.CreateInstance<Level>();
             }
        
             
             asset.levelData = new List<ObjectData>();
        
             foreach (var t in saveObjects)
             {
                 var levelData = new ObjectData
                 {
                     prefabID = t.prefabID,
                     transformData = t.GetComponent<OTransformData>() ? t.GetComponent<OData<TransformData>>().GetData() : new TransformData(),
                     cubeData = t.GetComponent<OCubeData>() ? t.GetComponent<OData<CubeData>>().GetData() : new CubeData(),
                     collectorData = t.GetComponent<OCollectorData>() ? t.GetComponent<OData<CollectorData>>().GetData() : new CollectorData(),
                     
                 };
                 asset.levelData.Add(levelData);
             }
        
             asset.levelID = levelID;
        
             if (newObject)
             {
                 AssetDatabase.CreateAsset(asset, $"Assets/Data/Levels/Level{levelID}.asset");
        
                 // var levels = levelManager.Levels.ToList();
                 //
                 // levels.Add(asset);
                 //
                 // levelManager.Levels = levels.ToArray();
        
             }
        
             EditorUtility.SetDirty(asset);
        
        
        
             foreach (var item in saveObjects)
             {
                 DestroyImmediate(item.gameObject);
             }
        
             currentObjects.Clear();
             Debug.Log("Saved level as Level ID: " + asset.levelID); 
         }
        
          [Button()]
         public void LoadLevel()
         {
             CreateLevel(levelID);
         }
         [Button()]
         private void ClearLevel()
         {
             for (int i = currentObjects.Count - 1; i >= 0; i--)
             {
                 DestroyImmediate(currentObjects[i].gameObject);
             }
             currentObjects.Clear();
         }
        
         private void CreateLevel(int id)
         {
             if (currentObjects.Count > 0)
                 ClearLevel();
             Level current = GetLevel(id);
             if (current)
             {
                 SetLevel(current);
             }
         }
        
         private Level GetLevel(int id)
         {
             var level = Array.Find(levelManager.Levels, lev => lev.levelID == id);
        
             if (level != null) return level;
             
             
             Debug.Log(Constants.Messages.NoLevelIDInTheListMessage);
        
             return null;
         }
        
         private void SetLevel(Level current)
         {
             foreach (var t in current.levelData)
             {
                 var instance = poolManager.prefabData.PrefabDataList.Find(x => x.prefab.prefabID == t.prefabID).prefab.transform;
        
                 var instantiate = Instantiate(instance.gameObject,transform);

                 //set data of level
                 
                 var transformData = instantiate.GetComponent<OData<TransformData>>();
                 if (transformData)
                     transformData.SetData(t.transformData);
               
        
                 currentObjects.Add(instantiate.transform);
             }
         }
        
         [CustomEditor(typeof(LevelEditorHelper))]
         public class LevelEditorHelper_Inspector : Editor
         {
             public override void OnInspectorGUI()
             {
                 base.OnInspectorGUI();
                 LevelEditorHelper helper = (LevelEditorHelper) target;
        
                 if (GUILayout.Button("Save"))
                 {
                     helper.SaveLevel();
                 }
                 if (GUILayout.Button("Load"))
                 {
                     helper.LoadLevel();
                 }
                 if (GUILayout.Button("Clear"))
                 {
                     helper.ClearLevel();
                 }
        
             }
         }
        




    }
}

#endif
