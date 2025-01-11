#if UNITY_EDITOR
using CMCore.LevelEditor;
#endif
using System;
using CMCore.Gameplay.Character.AI;
using CMCore.Gameplay.Character.Player;
using CMCore.Gameplay.Cube;
using CMCore.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMCore.Data.Object
{
    [AddComponentMenu("Cube Data")]
    public class OCubeData : OData<CubeData>
    {
        [ShowNativeProperty] private Material CurrentMaterial { get; set; }
        [field: SerializeField] private Material CollectedMaterialPlayer { get; set; }
        [field: SerializeField] private Material CollectedMaterialAI { get; set; }
        [field: SerializeField] private CubeBehavior Behavior { get; set; }
        [SerializeField] private Renderer rend;
        

        public override void SetData(CubeData newData)
        {
            Data = newData;

            if (SceneManager.GetActiveScene().name == "LevelEditor")
            {
                #if UNITY_EDITOR
                var levelGenerator = FindObjectOfType<LevelGenerator>();
                var findMaterial = levelGenerator.MaterialLibrary.cubeMaterials.Find(material => material.name == Data.colorHex);

                if (!findMaterial) return;
                SetMaterial(findMaterial);
                #endif
            }
            else
            {
                var findMaterial = GameManager.Instance.LevelManager.MaterialLibrary.cubeMaterials.Find(material => material.name == Data.colorHex);

                if (!findMaterial) return;
                SetMaterial(findMaterial);
            }
                
            
            
        }

        public void SetMaterial(Material material)
        {
            CurrentMaterial = material;
            rend.material = CurrentMaterial;
        }


        public void SetCollected()
        {
            var player = FindObjectOfType<PlayerMovementController>();
            var aiExist = GameManager.Instance.LevelManager.CurrentLevelInfo.aiBehavior != null;
            if (aiExist)
            {
                SetMaterial(Behavior.BelongsTo == player ? CollectedMaterialPlayer : CollectedMaterialAI);
                return;
            }
            SetMaterial(CollectedMaterialPlayer);
            
        }
    }


    [Serializable]
    public struct CubeData
    {
        public string colorHex;

        public CubeData(string colorHex)
        {
            this.colorHex = colorHex;
        }
    }
}
