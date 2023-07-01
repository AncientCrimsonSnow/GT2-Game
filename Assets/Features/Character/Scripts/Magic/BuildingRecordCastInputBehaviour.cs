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
    public class BuildingRecordCastInputBehaviour : BaseCastInput
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private BaseItem_SO droppedItemOnDestroy;
        [SerializeField] private GameObject originVisualisationPrefab;
        
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
    
        [Header("Input Focus")]
        [SerializeField] private DirectionInputFocus directionInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private CastInputFocus castInputFocus;

        private Vector3Int _originPosition;
        private GameObject _instantiatedOriginVisualizationPrefab;
        
        private void Awake()
        {
            _originPosition = TileHelper.TransformPositionToVector3Int(transform);

            _instantiatedOriginVisualizationPrefab = Instantiate(originVisualisationPrefab, transform.position, Quaternion.identity);
            InitializeFocus(gameObject);
            InitializeRecording(gameObject);
        }

        public override void OnCastInput(InputAction.CallbackContext context)
        {
            Destroy(_instantiatedOriginVisualizationPrefab);
            SetLoop();
        }

        public override void OnInterruptCast(InputAction.CallbackContext context)
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
            directionInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseMovementInput>());
            castInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseCastInput>());
            interactionInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseInteractionInput>());
        }
        
        private void InitializeRecording(GameObject instantiatedPrefab)
        {
            var onReplayCompleteAction = new Action(() =>
            {
                //destroy, if a record gets interrupted
                if (skeletonFocus.ContainsFocus())
                {
                    ResetFocus();
                }

                if (_instantiatedOriginVisualizationPrefab != null)
                {
                    Destroy(_instantiatedOriginVisualizationPrefab);
                }

                DropItem(instantiatedPrefab.transform);
                ReplayManager.Instance.UnregisterReplayable(instantiatedPrefab);
                Destroy(instantiatedPrefab);
            });
            
            //the newestInstantiatedSkeleton must be buffered, because it will be null during replay
            ReplayManager.Instance.InitializeRecording(skeletonFocus.GetFocus(), ResetFocus, onReplayCompleteAction);
        }

        private void ResetFocus()
        {
            directionInputFocus.Restore();
            interactionInputFocus.Restore();
            castInputFocus.Restore();
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
