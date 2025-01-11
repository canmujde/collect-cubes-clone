using CMCore.Data;
using CMCore.Interfaces;
using CMCore.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CMCore.Gameplay.Character.Player
{
    public class PlayerMovementController : CharacterBase
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerMovementProperties movementProperties;

        [ShowNativeProperty] private Vector3 Direction { get; set; }
        
        public override void ResetBehaviour()
        {
            base.ResetBehaviour();
            rb.mass = movementProperties.RigidbodyMass;
            rb.drag = movementProperties.RigidbodyDrag;
            Direction = Vector3.zero;
            
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        private void Awake()
        {
            InputManager.PointerDrag += OnPointerDrag;
            InputManager.PointerUp += OnPointerUp;
        }
        
        private void OnPointerUp(PointerEventData obj)
        {
            Direction = Vector3.zero;
        }

        private void OnPointerDrag(PointerEventData obj)
        {
            Direction = new Vector3(obj.delta.x, 0, obj.delta.y);
        }
        
        private void Update()
        {
            Rotate();
        }
        
        private void FixedUpdate()
        {
            Move();
            
        }

        
        private void Move()
        {
            rb.velocity = Direction.magnitude <= movementProperties.MinimumInputMagnitude || Direction == Vector3.zero
                ? Friction()
                : Direction * movementProperties.MovementSpeedFactor;
        }

        private void Rotate()
        {
            if (Mathf.Abs(Direction.magnitude) <= 1) return;
            
            var angleDifference = Vector3.Angle(Direction, transform.forward);

            if (angleDifference > movementProperties.InstantRotationAfterAngle)
            {
                RotateTowards(Direction);
            }
            else
            {
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direction), movementProperties.RotationSpeedFactor * Time.deltaTime);
            }
        }

        private void RotateTowards(Vector3 dir)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        private Vector3 Friction()
        {
            return Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * movementProperties.Friction);
        }



        
    }
}