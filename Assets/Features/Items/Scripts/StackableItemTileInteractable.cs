using Features.TileSystem.Scripts;
using Uilities.Pool;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class StackableItemTileInteractable : ItemTileInteractable
    {
        private int _itemCount;

        public StackableItemTileInteractable(Tile tile, bool isMovable, BaseItem_SO baseItemType, 
            int maxContainedItemCount, int containedItemAmountOnSpawn, Poolable pooledGameObject) : base(tile, isMovable)
        {
            Tile.ItemContainer.InitializeItem(baseItemType, pooledGameObject, maxContainedItemCount, containedItemAmountOnSpawn);
        }

        public override bool CanInteract(GameObject interactor, out string interactionText)
        {
            interactionText = "";
            if (!interactor.TryGetComponent(out IItemCarryBehaviour heldItemBehaviour)) return false;
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (!Tile.ItemContainer.IsItemFit(heldItemBehaviour.GetNextCarried()) || !Tile.ItemContainer.CanAddItemCount(1)) return false;

                interactionText = "Drop";
            }
            else
            {
                if (!heldItemBehaviour.CanCarryMore() || !Tile.ItemContainer.CanAddItemCount(-1)) return false;
                
                interactionText = "Pickup";
            }
            
            return true;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IItemCarryBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor cant't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (!Tile.ItemContainer.IsItemFit(heldItemBehaviour.GetNextCarried()) || !Tile.ItemContainer.CanAddItemCount(1)) return false;

                Tile.ItemContainer.AddItemCount(heldItemBehaviour.GetNextCarried(), 1);
                heldItemBehaviour.DropItem(heldItemBehaviour.GetNextCarried());
            }
            else
            {
                if (!heldItemBehaviour.CanCarryMore() || !Tile.ItemContainer.CanAddItemCount(-1)) return false;
                
                Tile.ItemContainer.AddItemCount(Tile.ItemContainer.ContainedBaseItem, -1);
                heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedBaseItem);
            }
            
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