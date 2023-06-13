using Features.Camera;
using Features.Character.Scripts;
using Features.Character.Scripts.Interaction;
using Features.Character.Scripts.Magic;
using Features.Character.Scripts.Movement;
using Features.ReplaySystem;
using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    [CreateAssetMenu]
    public class ReplayMagicItem_SO : BaseItem_SO
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private GameObject magicInstantiationPrefab;
        
        [Header("Character Focus")]
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
        
        [Header("Input Focus")]
        [SerializeField] private MovementInputFocus movementInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private MagicInputFocus magicInputFocus;

        public override bool TryCastMagic(GameObject caster)
        {
            if (skeletonFocus.ContainsFocus()) return false;
            
            var vector3Int = TileHelper.TransformPositionToVector3Int(caster.transform);
            var instantiatedPrefab = Instantiate(magicInstantiationPrefab, vector3Int, Quaternion.identity);
            InitializeFocus(instantiatedPrefab);
            InitializeRecording(instantiatedPrefab);
            
            return true;
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
        
        private void InitializeFocus(GameObject instantiatedPrefab)
        {
            movementInputFocus.SetCurrentAsRestore();
            magicInputFocus.SetCurrentAsRestore();
            skeletonFocus.SetFocus(instantiatedPrefab);
            cinemachineVirtualCameraFocus.SetFollow(instantiatedPrefab.transform).ApplyFollow();
            movementInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseMovementInput>());
            magicInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseMagicInput>());
            interactionInputFocus.SetFocus(instantiatedPrefab.GetComponent<BaseInteractionInput>());
        }

        //TODO: duplicate code
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
                         tile.ItemContainer.ContainedBaseItem == this && tile.ItemContainer.CanAddItemCount(this, 1)));

            if (foundTile.ContainsTileInteractableOfType<EmptyItemTileInteractable>())
            {
                foundTile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(foundTile, true, this));
            }
            else
            {
                foundTile.ItemContainer.AddItemCount(this, 1);
            }
        }
    }
}
