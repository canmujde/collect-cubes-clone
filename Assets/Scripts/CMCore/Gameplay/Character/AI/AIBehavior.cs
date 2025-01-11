using UnityEngine;

namespace CMCore.Gameplay.Character.AI
{
    [CreateAssetMenu(fileName = "AIBehavior", menuName = "CMCore/AIBehavior")]
    public class AIBehavior : ScriptableObject
    {
        [field: SerializeField] public AICubeSelectionBehavior AICubeSelectionBehavior { get; private set; }
        [field: SerializeField] public AICubeGetStateBehavior AICubeGetStateBehavior { get; private set; }
        [field: SerializeField] public AIProfessionalism AIProfessionalism { get; private set; }
        [field: SerializeField] public int MaxSmartAttempt { get; set; }
        [field: SerializeField] public float MinDistanceToCarry { get; set; }
    }


    public enum AICubeSelectionBehavior
    {
        Far,
        Near,
        Random,
        Smart
    }
    
    public enum AICubeGetStateBehavior
    {
        Hasty = 4,
        Stacker = 7,
        Filler = 12,
        Reckless = 2,
    }
    
    public enum AIProfessionalism
    {
        Hasty = 4,
        Relaxed = 2,
        Noob = 1,
        Pro = 7
    }
}
