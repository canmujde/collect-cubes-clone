using System;
using CMCore.Data.Object;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Cube;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CMCore.Managers
{
    public static class EventManager
    {
        public static Action<GameState> OnGameStateChanged;
        public static Action<OPrefabInfo> OnPool;
        public static Action<OPrefabInfo> OnPullFromPool;
        public static Action<CubeBehavior, CharacterBase> OnCubeCollectedBy;

    }
}