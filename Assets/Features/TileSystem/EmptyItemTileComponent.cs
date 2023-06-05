using UnityEngine;

namespace Features.TileSystem
{
    public class EmptyItemTileComponent : ItemTileComponent
    {
        public EmptyItemTileComponent(Tile tile) : base(tile) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) 
                && !heldItemBehaviour.IsCarrying()) return false;

            var heldItem = heldItemBehaviour.HeldItem;
            Tile.ItemContainer.InitializeItem(heldItem);
            Tile.ExchangeFirstTileComponentOfType<ItemTileComponent>(new UnstackableItemTileComponent(Tile));
            heldItemBehaviour.DropItem();
            return true;
        }

        public override bool IsMovable() => true;
    }
}