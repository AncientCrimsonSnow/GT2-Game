using Features.Camera;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Movement;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    public class RecordCastInputBehaviour : BaseCastInput
    {
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    
        [Header("Input Focus")]
        [SerializeField] private DirectionInputFocus movementInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private CastInputFocus magicInputFocus;

        private Vector3Int _originPosition;
    
        private void Awake()
        {
            _originPosition = TileHelper.TransformPositionToVector3Int(transform);
        }

        public override void OnCastInput(InputAction.CallbackContext context)
        {
            SetLoop();
        }

        public override void OnInterruptCast(InputAction.CallbackContext context)
        {
            ReplayManager.Instance.StopReplayable(skeletonFocus.GetFocus());
        }
    
        private void SetLoop()
        {
            var isLoop = _originPosition == TileHelper.TransformPositionToVector3Int(transform);
            ReplayManager.Instance.StartReplay(skeletonFocus.GetFocus(), isLoop);
        }

        private void OnMouseDown()
        {
            ReplayManager.Instance.StopReplayable(gameObject);
        }
    }
}
