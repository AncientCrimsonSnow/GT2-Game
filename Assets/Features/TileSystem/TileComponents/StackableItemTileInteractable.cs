using Features.TileSystem.CharacterBehaviours;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class StackableItemTileInteractable : ItemTileInteractable
    {
        private int _itemCount;

        public StackableItemTileInteractable(Tile tile) : base(tile) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour))
            {
                Debug.LogWarning("The interactor cant't pickup Items, because CarriedItemBaseBehaviour is missing!");
                return false;
            }
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (!Tile.ItemContainer.CanAddItemCount(heldItemBehaviour.CarriedItem, 1)) return false;

                Tile.ItemContainer.AddItemCount(heldItemBehaviour.CarriedItem, 1);
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (!Tile.ItemContainer.CanAddItemCount(Tile.ItemContainer.ContainedItem, -1)) return false;
                
                Tile.ItemContainer.AddItemCount(Tile.ItemContainer.ContainedItem, -1);
                heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedItem);
            }
            
            return true;
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}