using UnityEngine;

namespace Features.TileSystem
{
    public class EmptyItemTileComponent : ExchangeableBaseTileComponent
    {
        public EmptyItemTileComponent(Tile tile) : base(tile) { }

        public override bool IsExchangeable(BaseTileComponent newBaseTileComponent)
        {
            return true;
        }

        public override void OnExchange(BaseTileComponent newBaseTileComponent) { }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) 
                && !heldItemBehaviour.IsCarrying()) return false;

            var heldItem = heldItemBehaviour.HeldItem;
            var instantiatedObject = TileHelper.InstantiateOnTile(Tile, heldItem.prefab, Quaternion.identity);
            var tileObjectComponent = new UnstackableItemTileComponent(Tile, heldItem, instantiatedObject);
            heldItemBehaviour.DropItem();
            return Tile.TryRegisterTileComponent(tileObjectComponent);
        }

        public override bool IsMovable() => true;
    }
}