using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "PlayerMovementProperty", menuName = "CMCore/Player Movement Property")]
    public class PlayerMovementProperties : ScriptableObject
    {
        [field: SerializeField] public float Friction { get; private set; }
        [field: SerializeField] public float MovementSpeedFactor { get; private set;}
        [field: SerializeField] public float RotationSpeedFactor { get; private set;}
        [field: SerializeField] public float InstantRotationAfterAngle { get; private set;}
        [field: SerializeField] public float MinimumInputMagnitude { get;private set; }
        [field: SerializeField] public float RigidbodyMass { get;private set; }
        [field: SerializeField] public float RigidbodyDrag { get; private set;}
    }
}
