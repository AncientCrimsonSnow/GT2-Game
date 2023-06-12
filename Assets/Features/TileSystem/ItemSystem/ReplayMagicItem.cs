using Features.Input;
using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileComponents;
using Features.TileSystem.TileSystem;
using NewReplaySystem;
using UnityEngine;

namespace Features.TileSystem.ItemSystem
{
    [CreateAssetMenu]
    public class ReplayMagicItem : BaseItem
    {
        [SerializeField] private TileManager tileManager;
        [SerializeField] private SkeletonFocus skeletonFocus;
        [SerializeField] private CinemachineVirtualCameraFocus cinemachineVirtualCameraFocus;
        [SerializeField] private GameObject magicInstantiationPrefab;
        
        [SerializeField] private MovementInputFocus movementInputFocus;
        [SerializeField] private InteractionInputFocus interactionInputFocus;
        [SerializeField] private MagicInputFocus magicInputFocus;
        
        private Transform _entryCameraFollow;
        
        public override bool TryCastMagic(GameObject caster)
        {
            if (skeletonFocus.ContainsFocus()) return false;
            
            FocusSkeleton(caster);
            return true;
        }
        
        private void FocusSkeleton(GameObject caster)
        {
            movementInputFocus.SetCurrentAsRestore();
            magicInputFocus.SetCurrentAsRestore();
            var vector3Int = TileHelper.TransformPositionToVector3Int(caster.transform);
            skeletonFocus.SetOriginPosition(vector3Int);
            skeletonFocus.SetFocus(Instantiate(magicInstantiationPrefab, vector3Int, Quaternion.identity));

            //the newestInstantiatedSkeleton must be buffered, because it will be null during replay
            var bufferInstantiation = skeletonFocus.GetFocus();
            ReplayManager.Instance.InitializeRecording(skeletonFocus.GetFocus(), () =>
            {
                //destroy, if a record gets interrupted
                if (skeletonFocus.ContainsFocus())
                {
                    ResetFocus();
                }

                var worldPositionInt2 = TileHelper.TransformPositionToInt2(bufferInstantiation.transform);
                var foundTile = tileManager.SearchPlaceableTile(worldPositionInt2,
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
            
                ReplayManager.Instance.UnregisterReplayable(bufferInstantiation);
                Destroy(bufferInstantiation);
            });

            cinemachineVirtualCameraFocus.SetFollow(skeletonFocus.GetFocus().transform);

            if (skeletonFocus.GetFocus().TryGetComponent(out BaseMovementInput baseMovementInput))
            {
                movementInputFocus.SetFocus(baseMovementInput);
            }
            
            if (skeletonFocus.GetFocus().TryGetComponent(out BaseMagicInput baseMagicInput))
            {
                magicInputFocus.SetFocus(baseMagicInput);
            }

            if (skeletonFocus.GetFocus().TryGetComponent(out BaseInteractionInput baseInteractionInput))
            {
                interactionInputFocus.SetFocus(baseInteractionInput);
            }
        }

        private void ResetFocus()
        {
            movementInputFocus.Restore();
            interactionInputFocus.Restore();
            magicInputFocus.Restore();
            
            cinemachineVirtualCameraFocus.SetFollow(cinemachineVirtualCameraFocus.GetRestoreFollow());
            
            skeletonFocus.Restore();
        }
    }
}
