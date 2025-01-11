using CMCore.Data.Object;
using CMCore.Gameplay.Cube;
using CMCore.Interfaces;
using NaughtyAttributes;
using UnityEngine;

namespace CMCore.Gameplay.Character.AI
{
    public abstract class AIState : MonoBehaviour, IResetable
    {
        protected const float YPos = 0.126f;
        [ShowNativeProperty]
        [field: SerializeField] public CubeBehavior Target { get; protected internal set; }
        [field: SerializeField] public AIBehaviorController Controller { get; protected set; }
        protected internal abstract void ProcessState();
        protected abstract CubeBehavior FindTarget();
        protected abstract Transform FindTargetAsTransform();
        
        
        public virtual void LookTowardsTarget()
        {
            
            var lookPos = Target.transform.position - transform.position;
            lookPos.y = YPos;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (int)Controller.Behavior.AIProfessionalism*2);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        }

        public virtual void MoveToTarget()
        {
            var position = Target.transform.position;
            var finalPosition = new Vector3(position.x, YPos, position.z);
            
           

            transform.position = Vector3.MoveTowards(transform.position, finalPosition,
                Time.deltaTime * (int)Controller.Behavior.AIProfessionalism); 
            
           

            

          
        }

        public virtual void ResetBehaviour()
        {
            Target = null;
        }
    }
}
