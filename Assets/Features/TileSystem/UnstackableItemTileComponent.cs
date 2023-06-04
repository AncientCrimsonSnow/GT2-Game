using UnityEngine;

namespace Features.TileSystem
{
    public class UnstackableItemTileComponent : ExchangeableBaseTileComponent, IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; private set; }
        public Item ContainedItem { get; }

        public UnstackableItemTileComponent(Tile tile, Item newItem, GameObject instantiatedObject) : base(tile)
        {
            ContainedItem = newItem;
            InstantiatedGameObject = instantiatedObject;
        }

        public override bool IsExchangeable(BaseTileComponent newBaseTileComponent)
        {
            switch (newBaseTileComponent)
            {
                case StackableItemTileComponent stackableItemTileComponent when ContainedItem != stackableItemTileComponent.ContainedItem:
                case UnstackableItemTileComponent:
                    return false;
                default:
                    return true;
            }
        }

        public override void OnExchange(BaseTileComponent newBaseTileComponent)
        {
            Object.Destroy(InstantiatedGameObject);
            InstantiatedGameObject = null;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour) && heldItemBehaviour.IsCarrying()) return false;
            
            heldItemBehaviour.PickupItem(ContainedItem);
            return Tile.TryRegisterTileComponent(new EmptyItemTileComponent(Tile));
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}