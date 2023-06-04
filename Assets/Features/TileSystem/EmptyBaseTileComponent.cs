using UnityEngine;

namespace Features.TileSystem
{
    public class EmptyBaseTileComponent : BaseTileComponent
    {
        public EmptyBaseTileComponent(Tile tile) : base(tile) { }

        public override bool IsExchangeable(BaseTileComponent newBaseTileComponent)
        {
            return true;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) 
                && !heldItemBehaviour.IsCarrying()) return false;

            var heldItem = heldItemBehaviour.HeldItem;
            var instantiatedObject = TileHelper.InstantiateOnTile(Tile, heldItem.prefab, Quaternion.identity);
            var tileObjectComponent = new UnstackableBaseTileComponent(Tile, heldItem, instantiatedObject);
            return Tile.TryRegisterTileComponent(tileObjectComponent);
        }

        public override bool IsMovable() => true;
    }
}