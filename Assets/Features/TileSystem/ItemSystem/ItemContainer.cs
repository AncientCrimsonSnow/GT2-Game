using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.ItemSystem
{
    public class ItemContainer
    {
        public Item ContainedItem { get; private set; }
        
        private readonly Tile _tile;
        
        private GameObject _instantiatedGameObject;
        private int _itemCount;
        private int _maxContainedItemCount;

        public ItemContainer(Tile tile)
        {
            _tile = tile;
        }

        public bool ContainsItem()
        {
            return _instantiatedGameObject != null && ContainedItem != null;
        }
        
        public void InitializeItem(Item newItem, GameObject instantiatedObject, int maxContainedItemCount = 1, int itemCount = 1)
        {
            if (ContainsItem()) return;

            _itemCount = itemCount;
            _maxContainedItemCount = maxContainedItemCount;
            ContainedItem = newItem;
            _instantiatedGameObject = instantiatedObject;
        }

        public void InitializeItem(Item newItem, int maxItemCount = 1, int itemCount = 1)
        {
            if (ContainsItem()) return;

            _itemCount = itemCount;
            _maxContainedItemCount = maxItemCount;
            ContainedItem = newItem;
            _instantiatedGameObject = TileHelper.InstantiateOnTile(_tile, newItem.prefab, Quaternion.identity);
        }
        
        public bool CanDestroyItem(int maxItemDestructionCount)
        {
            if (_itemCount <= maxItemDestructionCount) return true;
            
            Debug.LogWarning($"You can only destroy the Item, when there are at most {maxItemDestructionCount} items on it! There are/is currently {_itemCount}");
            return false;
        }

        public void DestroyItem(int maxItemDestructionCount)
        {
            if (!CanDestroyItem(maxItemDestructionCount)) return;
            
            Object.Destroy(_instantiatedGameObject);
            ContainedItem = null;
            _maxContainedItemCount = 0;
        }

        public void AddItemCount(Item item, int change)
        {
            if (CanAddItemCount(item, change))
            {
                _itemCount += change;
            }
        }

        public bool CanAddItemCount(Item item, int change)
        {
            if (ContainedItem != item)
            {
                Debug.LogWarning("You cant add a different item to this ItemContainer!");
                return false;
            }
            
            if (_itemCount + change < 0)
            {
                Debug.LogWarning("You can't remove more items than there are available on this Tile!");
                return false;
            }

            if (_itemCount + change > _maxContainedItemCount)
            {
                Debug.LogWarning("You can't add more items on this Tile!");
                return false;
            }
            
            return true;
        }
    }
}