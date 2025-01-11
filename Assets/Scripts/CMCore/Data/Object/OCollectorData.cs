using System;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Collector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CMCore.Data.Object
{
    public enum BelongsTo
    {
        Player,
        AI
    }
    
    public class OCollectorData :  OData<CollectorData>
    {
        [SerializeField] private CollectorBehavior collectorBehavior;
        
        public override void SetData(CollectorData newData)
        {
            base.SetData(newData);
            collectorBehavior.SetBelonging(Data.belongsTo);
            ;
        }
    }
    
    
    [Serializable]
    public struct CollectorData
    {
        public BelongsTo belongsTo;

        public CollectorData(BelongsTo belongs)
        {
            belongsTo = belongs;
        }
    }
}
