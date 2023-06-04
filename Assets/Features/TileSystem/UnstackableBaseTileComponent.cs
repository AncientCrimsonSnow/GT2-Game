using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableBaseTileComponent : BaseTileComponent, IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; private set; }
        public Item ContainedItem { get; }

        public UnstackableBaseTileComponent(Tile tile, Item newItem, GameObject instantiatedObject) : base(tile)
        {
            ContainedItem = newItem;
            InstantiatedGameObject = instantiatedObject;
        }

        public override bool IsExchangeable(BaseTileComponent newBaseTileComponent)
        {
            switch (newBaseTileComponent)
            {
                case StackableBaseTileComponent stackableItemTileComponent when ContainedItem != stackableItemTileComponent.ContainedItem:
                case UnstackableBaseTileComponent:
                    return false;
                default:
                    Object.Destroy(InstantiatedGameObject);
                    InstantiatedGameObject = null;
                    return true;
            }
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) && heldItemBehaviour.IsCarrying()) return false;
            
            heldItemBehaviour.PickupItem(ContainedItem);
            return Tile.TryRegisterTileComponent(new EmptyBaseTileComponent(Tile));
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}