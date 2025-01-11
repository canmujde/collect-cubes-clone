using System;
using System.Linq;
using CMCore.Data.Object;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Character.Player;
using CMCore.Interfaces;
using CMCore.Managers;
using CMCore.Util;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMCore.Gameplay.Cube
{
    public class CubeBehavior : MonoBehaviour, IResetable
    {
        [SerializeField] private float maxVelocity;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject childVisual;
        [SerializeField] private GameObject childCollider;

        private OCubeData _cubeData;
        private const float MagnetDuration = 0.2f;
        public bool HasCollected { get; private set; }

        [ShowNativeProperty] public CharacterBase BelongsTo { get; internal set; }

        private void Awake()
        {
            _cubeData = GetComponent<OCubeData>();
            EventManager.OnCubeCollectedBy += CollectedBy;
        }

        private void CollectedBy(CubeBehavior behavior, CharacterBase character)
        {
            if (behavior != this) return;
            
            
            HasCollected = true;
            rb.isKinematic = true;
            rb.drag = 0;
            var randomLayer = Constants.Layers.CubeLayers.GetRandomElement();

            ChangeLayerAll(randomLayer);

            var player = FindObjectOfType<PlayerMovementController>();
            if (GameManager.Instance.LevelManager.CurrentLevelInfo.aiBehavior != null)
            {
                if (BelongsTo && BelongsTo.Equals(player))
                    GameManager.Instance.LevelManager.IncreasePlayerScore();
                else
                    GameManager.Instance.LevelManager.IncreaseAIScore();

                
            }

            else
            {
                GameManager.Instance.LevelManager.IncreasePlayerScore();
            }
            
            _cubeData.SetCollected();
            
            
        }

     
        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponentInParent<CharacterBase>();
            if (!character) return;

            character.ACubeEnteredCarryArea(this);
        }
        private void OnTriggerExit(Collider other)
        {
            var character = other.GetComponentInParent<CharacterBase>();
            if (!character) return;

            character.ACubeExitedCarryArea(this);
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        private void Update()
        {
            if (BelongsTo)
            {
                if (Vector3.Distance(transform.position, BelongsTo.transform.position)>3)
                {
                    if (BelongsTo.CarryingCubes.Contains(this)) BelongsTo.CarryingCubes.Remove(this);
                    BelongsTo = null;
                }
            }
        }

        private void FixedUpdate()
        {
            if (HasCollected)
            {
                // rb.velocity = Vector3.down * .5f;
                return;
            }

            ClampVelocity();
        }
        public void ResetBehaviour()
        {
            BelongsTo = null;
            HasCollected = false;
            rb.drag = 4;
            rb.isKinematic = false;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            ChangeLayerAll(Constants.Layers.Cube);
            AnimateVisual();
            if (GameManager.Instance.LevelManager.CurrentLevelInfo.duration <= 0)
                GameManager.Instance.LevelManager.LevelGoal++;
        }

        private void AnimateVisual()
        {
            childVisual.transform.DOKill();
            childVisual.transform.localScale = Vector3.one;
            childVisual.transform.DOPunchScale(Vector3.one * -0.5f, 0.6f, 0, 6).SetEase(Ease.OutElastic).SetDelay(0.2f);
        }

        private void ClampVelocity()
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }

        private void OnCollected(CubeBehavior behavior)
        {
            if (behavior != this) return;
            HasCollected = true;
            rb.isKinematic = true;
            rb.drag = 0;
            var randomLayer = Constants.Layers.CubeLayers.GetRandomElement();

            ChangeLayerAll(randomLayer);
            GameManager.Instance.LevelManager.IncreasePlayerScore();
            _cubeData.SetCollected();
        }

        public void Reposition(Vector3 worldPos)
        {
            transform.position = worldPos;
        }

        public void Pull(Transform collector, Action action)
        {


       
           
            
            transform.DOMove(collector.transform.position, MagnetDuration).OnComplete(() => action?.Invoke());
        }

        public void ChangeLayerAll(string layer)
        {
            gameObject.ChangeLayer(layer);
            childVisual.ChangeLayer(layer);
            childCollider.ChangeLayer(layer);
        }
    }
}