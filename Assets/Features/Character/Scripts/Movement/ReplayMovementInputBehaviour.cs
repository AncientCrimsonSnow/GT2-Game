using System;
using DG.Tweening;
using Features.ReplaySystem;
using Features.ReplaySystem.Record;
using Features.TileSystem.Scripts;
using Uilities.Attributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Movement
{
    public class ReplayMovementInputBehaviour : BaseMovementInput, IReplayOriginator
    {
        public Action<IInputSnapshot> PushNewTick { get; set; }

        [SerializeField] private TileManager tileManager;
        [SerializeField, Layer] private int editorLayer;
        [SerializeField] private Ease easeType;
    
        private Vector2 _storedInputVector;

        private void Start()
        {
            ReplayManager.Instance.RegisterOriginator(gameObject, this);
        }

        public override void OnDirectionInputFocusChanges()
        {
            _storedInputVector = Vector2.zero;
        }

        public override void OnDirectionInput(InputAction.CallbackContext context)
        {
            _storedInputVector = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (ReplayManager.Instance.IsTickPerformed || _storedInputVector == Vector2.zero) return;

            var inputInt2 = new int2(Mathf.RoundToInt(_storedInputVector.x), Mathf.RoundToInt(_storedInputVector.y));
            var targetTile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform) + inputInt2);

            if (targetTile.IsMovable())
            {
                PushNewTick.Invoke(new MovementInputSnapshot(transform, tileManager, _storedInputVector, easeType));
            }
            else
            {
                PushNewTick.Invoke(new RotationInputSnapshot(transform, _storedInputVector));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (editorLayer != other.gameObject.layer) return;
        
            ReplayManager.Instance.StopReplayable(gameObject);
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }

    public class OriginatorExample : MonoBehaviour, IReplayOriginator
    {
        public Action<IInputSnapshot> PushNewTick { get; set; }

        private void Awake()
        {
            ReplayManager.Instance.RegisterOriginator(gameObject, this);
        }

        public void PushTick()
        {
            PushNewTick.Invoke(new ExampleInputSnapshot(transform, Vector3.forward));
        }
    }

    public class ExampleInputSnapshot : IInputSnapshot
    {
        private readonly Transform transform;
        private readonly Vector3 movementVector;

        public ExampleInputSnapshot(Transform transform, Vector3 movementVector)
        {
            this.transform = transform;
            this.movementVector = movementVector;
        }
        
        public void Tick(float tickDurationInSeconds)
        {
            transform.DOMove(transform.position + movementVector, tickDurationInSeconds);
        }
    }
}
