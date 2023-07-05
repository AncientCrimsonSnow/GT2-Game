using System;
using Features.Items.Scripts;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    public class RecordCastInputBehaviour : BaseCastInput
    {
        [SerializeField] private TileManager tileManager;
        
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;

        private Vector3Int _originPosition;
    
        private void Awake()
        {
            _originPosition = TileHelper.TransformPositionToVector3Int(transform);
        }

        public override void OnCastInput(InputAction.CallbackContext context)
        {
            var tile = tileManager.GetTileAt(TileHelper.TransformPositionToInt2(transform));

            if (tile.ContainsTileInteractableOfType<UnstackableItemTileInteractable>())
            {
                tile.TryCast(gameObject);
            }
            else
            {
                SetLoop();
            }
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
