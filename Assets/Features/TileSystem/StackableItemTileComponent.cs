using UnityEngine;

namespace Features.TileSystem
{
    public class StackableItemTileComponent : ExchangeableBaseTileComponent, IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; private set; }
        public Item ContainedItem { get; }
        
        private int _itemCount;

        public StackableItemTileComponent(Tile tile, Item containedItem, GameObject instantiatedObject) : base(tile)
        {
            ContainedItem = containedItem;
            InstantiatedGameObject = instantiatedObject;
        }

        public override bool IsExchangeable(BaseTileComponent newBaseTileComponent)
        {
            switch (newBaseTileComponent)
            {
                case UnstackableItemTileComponent unstackableItemTileComponent when _itemCount <= 1 && ContainedItem != unstackableItemTileComponent.ContainedItem:
                case StackableItemTileComponent:
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
            if (!interactor.TryGetComponent(out CarriedItemBaseBehaviour heldItemBehaviour)) return false;
            
            if (heldItemBehaviour.IsCarrying())
            {
                if (_itemCount >= ContainedItem.maxStack)
                {
                    Debug.LogWarning($"Cant add {heldItemBehaviour.HeldItem}! {GetType()} has reached it's max item count!");
                    return false;
                }
                _itemCount++;
                
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (IsSuccessfulItemRemove(-1))
                {
                    heldItemBehaviour.PickupItem(ContainedItem);
                }
            }
            
            return true;
        }
        
        public bool IsSuccessfulItemRemove(int amount)
        {
            if (_itemCount <= 0) return false;
                
            _itemCount -= amount;
            return true;
        }

        public override bool IsMovable()
        {
            return true;
        }
    }
}