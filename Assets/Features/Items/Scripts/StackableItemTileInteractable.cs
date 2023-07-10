using Features.TileSystem.Scripts;
using Uilities.Pool;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class StackableItemTileInteractable : ItemTileInteractable
    {
        private readonly Poolable _pooledGameObject;
        private int _itemCount;

        public StackableItemTileInteractable(Tile tile, bool isMovable, BaseItem_SO baseItemType, 
            int maxContainedItemCount, int containedItemAmountOnSpawn, Poolable pooledGameObject) : base(tile, isMovable)
        {
            _pooledGameObject = pooledGameObject;
            Tile.ItemContainer.InitializeItem(baseItemType, pooledGameObject, maxContainedItemCount, containedItemAmountOnSpawn);
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
                if (!Tile.ItemContainer.IsItemFit(heldItemBehaviour.CarriedBaseItem) || !Tile.ItemContainer.CanAddItemCount(1)) return false;

                Tile.ItemContainer.AddItemCount(heldItemBehaviour.CarriedBaseItem, 1);
                heldItemBehaviour.DropItem();
            }
            else
            {
                if (!Tile.ItemContainer.CanAddItemCount(-1)) return false;
                
                Tile.ItemContainer.AddItemCount(Tile.ItemContainer.ContainedBaseItem, -1);
                heldItemBehaviour.PickupItem(Tile.ItemContainer.ContainedBaseItem);
            }
            
            return true;
        }

        public override bool TryCast(GameObject caster)
        {
            return false;
        }
    }
}