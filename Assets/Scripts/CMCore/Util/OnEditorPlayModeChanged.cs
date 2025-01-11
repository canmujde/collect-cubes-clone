using CMCore.Data;
using CMCore.Managers;
using UnityEditor;
using OBJ = UnityEngine.Object;

#if UNITY_EDITOR
namespace CMCore.Util
{
    [InitializeOnLoad]
    public static class OnEditorPlayModeChanged
    {
        static OnEditorPlayModeChanged()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            var levelManager = OBJ.FindObjectOfType<LevelManager>();
            var helper = OBJ.FindObjectOfType<LevelEditorHelper>();

        

            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (helper.playLevelOnEnteredPlayMode)
                {
                    levelManager.PlaySpecificLevel(helper.levelID);
                   // levelManager.manualCreation = true;
                   // levelManager.manualLevelID = helper.levelID;
                }
                else
                {
                    levelManager.PlayNormal();
                    // levelManager.manualCreation = false;
                    // levelManager.manualLevelID = 0; 
                }
            }
        
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                // levelManager.manualCreation = false;
                // levelManager.manualLevelID = 0; 
                levelManager.PlayNormal();
            }

            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                // levelManager.manualCreation = false;
                // levelManager.manualLevelID = 0; 
                levelManager.PlayNormal();
            }
                
        

        }
    }
}
#endif