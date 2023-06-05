using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableItemTileComponent : ItemTileComponent
    {
        public UnstackableItemTileComponent(Tile tile) : base(tile) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) 
                && heldItemBehaviour.IsCarrying() && Tile.ItemContainer.CanDestroyItem()) return false;
            
            heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedItem);
            Tile.ItemContainer.AddItemCount(-1);
            Tile.ItemContainer.DestroyItem();
            Tile.ExchangeFirstTileComponentOfType<ItemTileComponent>(new EmptyItemTileComponent(Tile));
            return true;
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}