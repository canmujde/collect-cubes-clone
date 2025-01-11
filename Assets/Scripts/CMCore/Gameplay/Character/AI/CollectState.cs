using System.Collections.Generic;
using System.Linq;
using CMCore.Gameplay.Cube;
using UnityEngine;

namespace CMCore.Gameplay.Character.AI
{
    public class CollectState : AIState
    {

        private Transform _target;

        public override void ResetBehaviour()
        {
            base.ResetBehaviour();
            
            _target = null;
            FindTargetAsTransform();
        }

        protected internal override void ProcessState()
        {
            // Debug.Log("move to base");
            if (!_target)
            {
                var target = FindTargetAsTransform();
                if (target)
                    _target = target;
            }
            else
            {
                if (Controller.CarryingCubes.Any() && Vector3.Distance(transform.position, Controller.ConnectedCollector.transform.position) > 2)
                {
                    MoveToTarget();
                    LookTowardsTarget();
                }
                else
                {
                    foreach (var vCube in Controller.CarryingCubes)
                    {
                        vCube.BelongsTo = null;
                    }
                    Controller.CarryingCubes = new List<CubeBehavior>();
                    Controller.CurrentState = Controller.GetState;
                }
                
            }
        }


        public override void LookTowardsTarget()
        {
            
            var lookPos = _target.transform.position - transform.position;
            lookPos.y = YPos;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (int)Controller.Behavior.AIProfessionalism*2);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        }

        public override void MoveToTarget()
        {
            var position = _target.transform.position;
            var finalPosition = new Vector3(position.x, YPos, position.z);

            transform.position = Vector3.MoveTowards(transform.position, finalPosition,
                Time.deltaTime * (int)Controller.Behavior.AIProfessionalism);
            // Debug.Log("move to base");
        }


        protected override CubeBehavior FindTarget()
        {
            return null;
        }

        protected override Transform FindTargetAsTransform()
        {
            return Controller.ConnectedCollector.transform;
            // throw new System.NotImplementedException();
        }
    }
}
