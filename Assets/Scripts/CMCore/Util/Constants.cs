
namespace CMCore.Util
{
    public static class Constants //Constant values
    {
        public static class Messages
        {
            public const string RepeatLastLevelTooltip = "'0' means the game will loop all levels. \nOtherwise the game will loop last 'n' levels.";
            public const string NoPrefabInSceneMessage = "There is no prefab to save to level data in the current scene.";
            public const string NoLevelIDInTheListMessage = "LevelId is not in the list.";
            public const string StateBecomeNullMessage = "State became null! Setting state to menu state.";
            public const string OnLevelLoadedMessage = "OnLevelLoaded";
            public const string OnLevelStartedMessage = "OnLevelStarted";
            public const string OnLevelEndedMessage = "OnLevelEnded | Result: ";

            public const string OnStateChangedMessage = "StateChangedEvent => ";
        }
        public static class SettingsData
        {
            
            public const string VibrationsEnabledPref = "vibrationsEnabled";
            public const string MusicEnabledPref = "musicEnabled";
            public const string SfxEnabledPref = "sfxEnabled";
        }

        public static class Words
        {
            public const string AmountWord = "Amount";
            public const string LevelWord = "Level ";
            public const string DayWord = "Day ";
            public const string TargetWord = "Target ";
            public const string PlayerScoreText = "PlayerScoreText";
            public const string AIScoreText = "AIScoreText";
            

        }
        

        public static class Data
        {
            public const string CurrentLevelPref = "currentLevel";
            public const string CurrentLevelIDPref = "currentLevelId";

            public const string VibrationsEnabledStr = "VibrationsEnabled";
            public const string VibrationsDisabledStr = "VibrationsDisabled";

            public const string SfxEnabledStr = "SFXEnabled";
            public const string SfxDisabledStr = "SFXDisabled";

            public const string MusicEnabledStr = "MusicEnabled";
            public const string MusicDisabledStr = "MusicDisabled";

            public const int CurrentLevelPrefDefault = 1;
            public const int CurrentLevelIDPrefDefault = 0;

        }

        public static class Extension
        {
            public const string Material = ".mat";
        }
        
        public static class AnimationKeys
        {
            
        }
        
        public static class Tags
        {
            public const string Player = "Player";
        }

        public static class Layers
        {
            public const string Player = "Player";
            public const string IgnoreRaycast = "IgnoreRaycast";
            public const string Cube1 = "Cube1";
            public const string Cube2 = "Cube2";
            public const string Cube3 = "Cube3";
            public const string Cube = "Cube";

            public static string[] CubeLayers = { Cube1, Cube2, Cube3 };
        }
        


    }
}
