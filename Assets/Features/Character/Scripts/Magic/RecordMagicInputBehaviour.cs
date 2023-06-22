using System;
using Features.Camera;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Movement;
using Features.Items.Scripts;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts.Magic
{
    public class RecordMagicInputBehaviour : BaseMagicInput
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private BaseItem_SO droppedItemOnDestroy;
        
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    
        [Header("Input Focus")]
        [SerializeField] private MovementInputFocus movementInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private MagicInputFocus magicInputFocus;

        private Vector3Int _originPosition;
    
        private void Awake()
        {
            _originPosition = TileHelper.TransformPositionToVector3Int(transform);
            
            InitializeFocus(gameObject);
            InitializeRecording(gameObject);
        }

        public override void OnMagicInput(InputAction.CallbackContext context)
        {
            SetLoop();
        }

        public override void OnInterruptMagic(InputAction.CallbackContext context)
        {
            ReplayManager.Instance.StopReplayable(skeletonFocus.GetFocus());
        }
        
        private void OnMouseDown()
        {
            ReplayManager.Instance.StopReplayable(gameObject);
        }
        
        private void InitializeFocus(GameObject instantiatedPrefab)
        {
            skeletonFocus.SetFocus(instantiatedPrefab);
            cinemachineVirtualCameraFocus.SetFollow(instantiatedPrefab.transform).ApplyFollow();
            movementInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseMovementInput>());
            magicInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseMagicInput>());
            interactionInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseInteractionInput>());
        }
        
        private void InitializeRecording(GameObject instantiatedPrefab)
        {
            //the newestInstantiatedSkeleton must be buffered, because it will be null during replay
            ReplayManager.Instance.InitializeRecording(skeletonFocus.GetFocus(), () =>
            {
                //destroy, if a record gets interrupted
                if (skeletonFocus.ContainsFocus())
                {
                    ResetFocus();
                }

                DropItem(instantiatedPrefab.transform);
                ReplayManager.Instance.UnregisterReplayable(instantiatedPrefab);
                Destroy(instantiatedPrefab);
            });
        }

        private void ResetFocus()
        {
            movementInputFocus.Restore();
            interactionInputFocus.Restore();
            magicInputFocus.Restore();
            cinemachineVirtualCameraFocus.RestoreFollow();
            skeletonFocus.Restore();
        }
        
        private void DropItem(Transform transform)
        {
            var worldPositionInt2 = TileHelper.TransformPositionToInt2(transform);
            var foundTile = tileManager.SearchNearestTileByCondition(worldPositionInt2,
                tile => tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() ||
                        (tile.TryGetFirstTileInteractableOfType(out StackableItemTileInteractable _) &&
                         tile.ItemContainer.ContainedBaseItem == droppedItemOnDestroy && tile.ItemContainer.CanAddItemCount(droppedItemOnDestroy, 1)));

            if (foundTile.ContainsTileInteractableOfType<EmptyItemTileInteractable>())
            {
                foundTile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(foundTile, true, droppedItemOnDestroy));
            }
            else
            {
                foundTile.ItemContainer.AddItemCount(droppedItemOnDestroy, 1);
            }
        }
    
        private void SetLoop()
        {
            var isLoop = _originPosition == TileHelper.TransformPositionToVector3Int(transform);
            ReplayManager.Instance.StartReplay(skeletonFocus.GetFocus(), isLoop);
        }
    }
}
