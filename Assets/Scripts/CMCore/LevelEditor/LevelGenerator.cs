
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CMCore.Data;
using CMCore.Data.Object;
using CMCore.Util;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;


namespace CMCore.LevelEditor
{
    public class LevelGenerator : MonoBehaviour
    {

        public MaterialLibrary MaterialLibrary => materialLibrary;

        [Header("Level Generation (Cubes)")] [HorizontalLine()] 
        [SerializeField] private Texture2D levelMap;
        [SerializeField] private MaterialLibrary materialLibrary;
        [SerializeField] private GameObject cubePrefab;
        
        [SerializeField] [Range(-3, 3)] private float xPosition = 0;
        [SerializeField] [Range(-5, 5)] private float zPosition = 0;
        [SerializeField] [Range(0.05f, 1)] private float scaleFactor = 1;
        
        [SerializeField] private Transform cubesParent;

        // [Header("Player")][HorizontalLine()] 
        // [SerializeField] private G
        
        [Header("Level Load & Save Operations")][HorizontalLine()]
        [SerializeField] private int levelId;
        [SerializeField] private Level[] levels;
        [SerializeField] private List<OPrefabInfo> currentObjects;
       [SerializeField] private GamePrefabsData PrefabDataList; 
        
        private const string MaterialPathInAssets = "Assets/Art/Materials/LevelMaterials/";
        private const float YPosition = 0.29f;

        private void OnValidate()
        {
            UpdateParentPositionAndScale();
        }

        
        
        private void UpdateParentPositionAndScale()
        {
            if (cubesParent == null) return;
            cubesParent.localScale = scaleFactor * Vector3.one;
            cubesParent.localPosition = Vector3.right * xPosition + Vector3.forward * zPosition + Vector3.up * YPosition;
        }


        [Button()]
        public void GenerateCubesFromPixelTexture()
        {
            if (levelMap == null)
            {
                Debug.LogWarning("Please insert texture to Level Map field.");
                return;
            }
            
            if (cubesParent.childCount>0) ClearGeneratedCubes();
            xPosition = 0;
            zPosition = 0;
            scaleFactor = 1;
            UpdateParentPositionAndScale();

            for (int x = 0; x < levelMap.width; x++)
            {
                for (int y = 0; y < levelMap.height; y++)
                {
                    GenerateCube(x, y);
                }
            }
            
            cubesParent.CenterOnChildren();
            UpdateParentPositionAndScale();
        }
        
        [Button()]
        public void ClearGeneratedCubes()
        {
            if (cubesParent == null) return;

            for (int p = cubesParent.childCount-1; p >= 0; p--)
            {
                var opr = cubesParent.GetChild(p).GetComponent<OPrefabInfo>();
                if (opr && currentObjects.Contains(opr))
                    currentObjects.Remove(opr);
                
                DestroyImmediate(cubesParent.GetChild(p).gameObject);
            }
        }

        [Button()]
        void SaveLevel()
        {
            var saveObjects = FindObjectsOfType<OPrefabInfo>();
            if (saveObjects.Length<=0)
            {
                Debug.LogWarning(Constants.Messages.NoPrefabInSceneMessage);
                return;
            }
            var asset = levels.FirstOrDefault(lstLevel => lstLevel.levelID == levelId);
        
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
                    // customData = t.GetComponent<OCustomData>() ? t.GetComponent<OCustomData>().GetData() : null
                     
                };
                asset.levelData.Add(levelData);
            }
        
            asset.levelID = levelId;
        
            if (newObject)
            {
                AssetDatabase.CreateAsset(asset, $"Assets/Data/Static/Levels/Level{levelId}.asset");
        
                var ls = levels.ToList();
        
                ls.Add(asset);
        
                levels = ls.ToArray();
        
            }
        
            EditorUtility.SetDirty(asset);
        
        
        
            foreach (var item in saveObjects)
            {
                DestroyImmediate(item.gameObject);
            }
        
            currentObjects.Clear();
            
            xPosition = 0;
            zPosition = 0;
            scaleFactor = 1;
            UpdateParentPositionAndScale();
            Debug.Log("Saved level as Level ID: " + asset.levelID); 
        }
        
        [Button()]
        public void LoadLevel()
        {
            CreateLevel(levelId);
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
            var level = Array.Find(levels, lev => lev.levelID == id);
        
            if (level != null) return level;
             
             
            Debug.Log(Constants.Messages.NoLevelIDInTheListMessage);
        
            return null;
        }
        
        private void SetLevel(Level current)
        {
            foreach (var t in current.levelData)
            {
                var instance = PrefabDataList.PrefabDataList.Find(x => x.prefab.prefabID == t.prefabID).prefab;
        
                var instantiate = Instantiate(instance);

                
                //set data of level
                 
                var transformData = instantiate.GetComponent<OData<TransformData>>();
                if (transformData)
                    transformData.SetData(t.transformData);
               
                var cubeData = instantiate.GetComponent<OData<CubeData>>();
                if (cubeData)
                {
                    cubeData.SetData(t.cubeData);
                    instantiate.transform.parent = cubesParent;
                }
                    
        
                currentObjects.Add(instantiate);
            }
        }

        private void GenerateCube(int x, int y) 
        {
            var color = levelMap.GetPixel(x, y);

            if (color.a == 0) return; //pixel is transparent, do not generate anything.

            var material = materialLibrary.cubeMaterials.Find(material => material.color == color);


            if (!material) material = CreateMaterial(color);

            var position = new Vector3(x - (levelMap.width / 2), 0, y - (levelMap.height / 2));

            var cube = Instantiate(cubePrefab, position, Quaternion.identity, cubesParent);
            cube.GetComponent<OData<CubeData>>().SetData(new CubeData
            {
                colorHex = material.name,
            });
            cube.GetComponentInChildren<Renderer>().sharedMaterial = material;
        }

        private Material CreateMaterial(Color color)
        {
            var material = new Material(Shader.Find("Standard"))
            {
                color = color
            };
            material.SetFloat("_Glossiness", 0);
            materialLibrary.cubeMaterials.Add(material);

            EditorUtility.SetDirty(materialLibrary);
            AssetDatabase.CreateAsset(material,
                MaterialPathInAssets + ColorUtility.ToHtmlStringRGB(color) + Constants.Extension.Material);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return material;
        }
        
        
        
        
        

        // bool ColorsAreClose(Color a, Color z, float threshold = 0.7f)
        // {
        //     float r = a.r - z.r,
        //         g = a.g - z.g,
        //         b = a.b - z.b;
        //     return (r*r + g*g + b*b) <= threshold*threshold;
        // }
    }
}

#endif