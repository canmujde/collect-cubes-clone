using System.Collections.Generic;
using CMCore.Data.Object;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "GamePrefabsData", menuName = "CMCore/Game Prefabs Data")]
    public class GamePrefabsData : ScriptableObject
    {
        [ReorderableList] public List<PrefabData> PrefabDataList; //List of prefabs.
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GamePrefabsData))]
    public class GamePrefabsDataEditor : Editor
    {
        private GamePrefabsData _target;

        public override void OnInspectorGUI()
        {
            if (_target == null)
                _target = (GamePrefabsData) target;
            base.OnInspectorGUI();

            foreach (var t in _target.PrefabDataList)
            {
                if (!t.prefab) continue;
                t.id = t.prefab.prefabID;
            }
        }
    }

#endif
    [System.Serializable]
    public class PrefabData
    {
        public int id;
        public OPrefabInfo prefab;
        public int size;
        public bool showInLevelEditor;

    }
}