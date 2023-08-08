using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class EmptyItemTileInteractable : ItemTileInteractable
    {
        public EmptyItemTileInteractable(Tile tile) : base(tile, true)
        {
            Tile.ItemContainer.DestroyItem();
        }

        public override bool CanInteract(GameObject interactor, out string interactionText)
        {
            interactionText = "";
            
            if (!interactor.TryGetComponent(out IItemCarryBehaviour heldItemBehaviour)) return false;
            if (!heldItemBehaviour.IsCarrying()) return false;

            interactionText = "Drop";
            return true;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IItemCarryBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor cant't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }

            if (!heldItemBehaviour.IsCarrying())
            {
                //Debug.LogWarning("The Interactor isn't carrying an item!");
                return false;
            }

            var heldItem = heldItemBehaviour.GetNextCarried();
            TileHelper.ReuseOnTile(Tile, heldItem.prefab, Quaternion.identity);
            heldItemBehaviour.DropItem(heldItem);
            return true;
        }

        public override bool CanCast(GameObject caster, out string interactionText)
        {
            interactionText = "";
            return false;
        }

        public override bool TryCast(GameObject caster)
        {
            return false;
        }
    }
}