using System.Collections.Generic;
using CMCore.Gameplay.Collector;
using CMCore.Gameplay.Cube;
using CMCore.Interfaces;
using CMCore.Util;
using UnityEngine;

namespace CMCore.Gameplay.Character
{
    public class CharacterBase : MonoBehaviour, IResetable
    {
        [SerializeField] private string selfCollidedCubeLayer;
        [field: SerializeField] public List<CubeBehavior> CarryingCubes { get; protected internal set; }
        [field: SerializeField] public CollectorBehavior ConnectedCollector { get; set; }

        public virtual void ResetBehaviour()
        {
            CarryingCubes = new List<CubeBehavior>();
        }

        public virtual void ACubeEnteredCarryArea(CubeBehavior behavior)
        {
            // if (behavior.BelongsTo != null &&
            //     Vector3.Distance(behavior.transform.position, behavior.BelongsTo.transform.position) < 0.2f) return;
            // if (CarryingCubes.Contains(behavior)) return;

            behavior.BelongsTo = this;
            behavior.ChangeLayerAll(selfCollidedCubeLayer);

            if (!CarryingCubes.Contains(behavior))
                CarryingCubes.Add(behavior);
        }

        public virtual void ACubeExitedCarryArea(CubeBehavior behavior)
        {
            // if (behavior.BelongsTo!= this) return;
            // if (!CarryingCubes.Contains(behavior)) return;


            behavior.BelongsTo = null;
            this.DelayedAction(() =>
            {
                if (behavior.HasCollected) return;
                behavior.ChangeLayerAll(Constants.Layers.Cube);
            },0.5f);
            

            if (CarryingCubes.Contains(behavior))
                CarryingCubes.Remove(behavior);
        }
    }
}