using Features.TileSystem.Scripts;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class StackableItemTileInteractable : ItemTileInteractable
    {
        private int _itemCount;

        public StackableItemTileInteractable(Tile tile, bool isMovable, BaseItem_SO baseItemType, 
            int maxContainedItemCount, int containedItemAmountOnSpawn, GameObject useThisGameObject = null) : base(tile, isMovable)
        {
            if (useThisGameObject)
            {
                Tile.ItemContainer.InitializeItem(baseItemType, useThisGameObject, maxContainedItemCount, containedItemAmountOnSpawn);
            }
            else
            {
                Tile.ItemContainer.InitializeItem(baseItemType, maxContainedItemCount, containedItemAmountOnSpawn);
            }
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out BaseItemCarryBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor cant't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (!Tile.ItemContainer.CanAddItemCount(heldItemBehaviour.CarriedBaseItem, 1)) return false;

                Tile.ItemContainer.AddItemCount(heldItemBehaviour.CarriedBaseItem, 1);
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (!Tile.ItemContainer.CanAddItemCount(Tile.ItemContainer.ContainedBaseItem, -1)) return false;
                
                Tile.ItemContainer.AddItemCount(Tile.ItemContainer.ContainedBaseItem, -1);
                heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedBaseItem);
            }
            
            return true;
        }

        public override bool TryCastMagic(GameObject caster)
        {
            if (!Tile.ItemContainer.CanAddItemCount(Tile.ItemContainer.ContainedBaseItem, -1)) return false;
            if (!Tile.ItemContainer.ContainedBaseItem.TryCastMagic(caster)) return false;
            
            Tile.ItemContainer.AddItemCount(Tile.ItemContainer.ContainedBaseItem, -1);
            return true;
        }
    }
}