using System;
using System.Linq;
using CMCore.Gameplay.Cube;
using CMCore.Util;
using UnityEngine;

namespace CMCore.Gameplay.Character.AI
{
    public class GetState : AIState
    {
        [SerializeField]
        private int _smartAttempt;

        protected internal override void ProcessState()
        {
            if (!Target)
            {
                var target = FindTarget();
                if (target)
                    Target = target;
                else
                {
                    //there is no cube to get. 
                    // if collected cubes any ?  go to collect state;

                    if (Controller.CarryingCubes.Any()) Controller.CurrentState = Controller.CollectState;
                    else Target = FindTarget();
                }
            }
            else // found target cube, go towards it ? or go collect
            {
                var decideToGetMore = DecideToGet();

                if (decideToGetMore)
                {
                    if (Target.BelongsTo && Target.BelongsTo != Controller) Target = null;
                    else
                    {
                        MoveToTarget();
                        LookTowardsTarget();

                        var diff = Target.transform.position - transform.position;
                        if (diff.magnitude <= Controller.Behavior.MinDistanceToCarry && !Controller.CarryingCubes.Contains(Target))
                        {
                            Target.BelongsTo = Controller;
                            Controller.CarryingCubes.Add(Target);
                            Target = null;
                        }
                    }
                }
                else
                {
                    Controller.CurrentState = Controller.CollectState;
                    // go to collect state;
                }
            }
        }


        protected override CubeBehavior FindTarget()
        {
            var availableCubes = FindObjectsOfType<CubeBehavior>()
                .Where(behavior => !behavior.HasCollected && !behavior.BelongsTo &&
                                   !Controller.CarryingCubes.Contains(behavior) &&
                                   behavior.transform.position.z > -2).ToArray();
            return SelectTarget(availableCubes);
        }

        protected override Transform FindTargetAsTransform()
        {
            return null;
        }

        private bool DecideToGet()
        {
            var cubeCountMax = (int)Controller.Behavior.AICubeGetStateBehavior;
            var carryingCount = Controller.CarryingCubes.Count;

            return carryingCount < cubeCountMax;
        }

        private CubeBehavior SelectTarget(CubeBehavior[] availableCubes)
        {
            var selectionBehavior = Controller.Behavior.AICubeSelectionBehavior;
            
            if (Controller.Behavior.AICubeSelectionBehavior == AICubeSelectionBehavior.Smart &&
                _smartAttempt >= Controller.Behavior.MaxSmartAttempt)
                selectionBehavior = AICubeSelectionBehavior.Near;
                
            
            switch (selectionBehavior)
            {
                case AICubeSelectionBehavior.Far:
                    return availableCubes != null && availableCubes.Length > 0
                        ? availableCubes.FindFurthestCube(transform.position)
                        : null;
                case AICubeSelectionBehavior.Near:
                    return availableCubes != null && availableCubes.Length > 0
                        ? availableCubes.FindClosestCube(transform.position)
                        : null;
                case AICubeSelectionBehavior.Random:
                    return availableCubes != null && availableCubes.Length > 0
                        ? availableCubes.GetRandomElement()
                        : null;
                case AICubeSelectionBehavior.Smart:
                    var smartFind = availableCubes != null && availableCubes.Length > 0
                        ? availableCubes.FindAtLeastNCloseOnes(1f, 3)
                        : null;

                    if (!smartFind)
                        _smartAttempt++;
                    
                    else _smartAttempt--;

                    _smartAttempt = Mathf.Clamp(_smartAttempt, 0, 100);

                    return smartFind;
                    
            }

            return null;
        }
    }
}