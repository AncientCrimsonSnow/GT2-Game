using UnityEngine;

namespace Features.TileSystem
{
    public class StackableItemTileComponent : ItemTileComponent
    {
        private int _itemCount;

        public StackableItemTileComponent(Tile tile) : base(tile) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour)) return false;
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (!Tile.ItemContainer.CanAddItemCount(1)) return false;

                Tile.ItemContainer.AddItemCount(1);
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (!Tile.ItemContainer.CanAddItemCount(-1)) return false;
                
                Tile.ItemContainer.AddItemCount(-1);
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