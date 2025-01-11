using System;
using System.Collections.Generic;
using CMCore.Gameplay.Cube;
using CMCore.Interfaces;
using CMCore.Managers;
using UnityEngine;

namespace CMCore.Gameplay.Character.AI
{
    public class AIBehaviorController : CharacterBase
    {
        
        [field: SerializeField] public AIBehavior Behavior { get; protected set; }
        
        [field: SerializeField] public GetState GetState { get; private set; }
        [field: SerializeField] public CollectState CollectState { get; private set; }
        [field: SerializeField] public  AIState CurrentState { get; internal set; }
        
        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.InGame) return;
            
            CurrentState.ProcessState();
        }
        public override void ResetBehaviour()
        {
            base.ResetBehaviour();
            Behavior = GameManager.Instance.LevelManager.CurrentLevelInfo.aiBehavior;
            CurrentState = GetState;
        }

        public override void ACubeEnteredCarryArea(CubeBehavior behavior)
        {
            base.ACubeEnteredCarryArea(behavior);
            GetState.Target = null;
        }

        public override void ACubeExitedCarryArea(CubeBehavior behavior)
        {
            base.ACubeExitedCarryArea(behavior);
        }


        // private void OnTriggerEnter(Collider other)
        // {
        //     var cube = other.GetComponentInParent<CubeBehavior>();
        //     
        //     
        //     if (!cube) return;
        //     if (cube.BelongsTo) return;
        //     if (CarryingCubes.Contains(cube)) return;
        //     Debug.Log("?");
        //     CarryingCubes.Add(cube);
        //
        //
        //
        // }
    }
}
