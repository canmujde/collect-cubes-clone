using System;
using System.Collections.Generic;
using System.Linq;
using CMCore.Data.Object;
using CMCore.Gameplay.Character;
using CMCore.Gameplay.Character.AI;
using CMCore.Gameplay.Character.Player;
using CMCore.Gameplay.Cube;
using CMCore.Interfaces;
using CMCore.Managers;
using CMCore.Util;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMCore.Gameplay.Collector
{
    public class CollectorBehavior : MonoBehaviour, IResetable
    {
        [SerializeField] private List<CubeBehavior> collectedCubes;
        [SerializeField] private Material playerBelongingMaterial;
        [SerializeField] private Material aiBelongingMaterial;


        [ShowNativeProperty] public CharacterBase BelongsTo { get; internal set; }

        [SerializeField] private Renderer mainRenderer;
        [SerializeField] private Renderer particle;

        public void ResetBehaviour()
        {
            collectedCubes = new List<CubeBehavior>();
            BelongsTo = null;
        }

        public void SetBelonging(BelongsTo belongsTo)
        {
            this.DelayedAction(() => FindCharacters(belongsTo), 0.2f);

            SetMaterialByBelonging(belongsTo);
        }

        private void FindCharacters(BelongsTo belongsTo)
        {
            if (belongsTo == Data.Object.BelongsTo.Player)
            {
                var character = FindObjectOfType<PlayerMovementController>();
                character.ConnectedCollector = this;
                BelongsTo = character;
            }
            else
            {
                var character = FindObjectOfType<AIBehaviorController>();
                character.ConnectedCollector = this;
                BelongsTo = character;
            }
        }

        private void SetMaterialByBelonging(BelongsTo belongsTo)
        {
            switch (belongsTo)
            {
                case Data.Object.BelongsTo.Player:
                    mainRenderer.material = playerBelongingMaterial;
                    particle.material = playerBelongingMaterial;
                    break;
                case Data.Object.BelongsTo.AI:
                    mainRenderer.material = aiBelongingMaterial;
                    particle.material = aiBelongingMaterial;
                    break;
            }
        }

        private void Awake()
        {
            EventManager.OnCubeCollectedBy += OnCubeCollectedBy;
        }


        private void OnDestroy()
        {
            EventManager.OnCubeCollectedBy -= OnCubeCollectedBy;
        }


        private void OnCubeCollectedBy(CubeBehavior cube, CharacterBase character)
        {
            if (character.ConnectedCollector != this) return;
            collectedCubes.Add(cube);

            if (character.CarryingCubes.Contains(cube)) character.CarryingCubes.Remove(cube);

            var topCubeY = collectedCubes.Any()
                ? collectedCubes.Max(x => x.transform.position.y)
                : transform.position.y;
            var vector = transform.position;
            
            vector.y = topCubeY;
            
            // Rastgele bir açı üret (0 ile 180 derece arasında, radyan cinsinden)
            float randomAngle = Random.Range(0f, Mathf.PI);

            // Yarım dairenin içinde rastgele bir nokta için yarıçapı ölçekle
            float randomRadius = Random.Range(0f, 1f);

            // X ve Z koordinatlarını hesapla
            float x = Mathf.Cos(randomAngle) * randomRadius;
            float z = Mathf.Sin(randomAngle) * randomRadius;

            // Forward yönünü kullanarak doğru rotasyonu uygula
            Vector3 localPosition = new Vector3(x, Random.Range(0f,0.5f), z);
            
            Vector3 worldPosition = transform.TransformPoint(localPosition);
            
            // cube.ChangeLayerAll(Constants.Layers.Cube);

   

            cube.Pull(transform, () => cube.Reposition(worldPosition));
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckCube(other);
        }

        private void CheckCube(Collider other)
        {
            var cubeBehavior = other.GetComponentInParent<CubeBehavior>();
            if (!cubeBehavior) return;
            if (cubeBehavior.HasCollected) return;
            if (GameManager.Instance.LevelManager.CurrentLevelInfo.aiBehavior != null &&
                cubeBehavior.BelongsTo != BelongsTo) return;

            EventManager.OnCubeCollectedBy?.Invoke(cubeBehavior, BelongsTo);
        }
    }
}