using Features.TileSystem.Scripts;
using Uilities.Pool;
using UnityEngine;

namespace Features.Items.Scripts
{
    public class ItemContainer
    {
        public BaseItem_SO ContainedBaseItem { get; private set; }

        private readonly Tile _tile;

        public Poolable PooledGameObject => _pooledGameObject;
        private Poolable _pooledGameObject;
        private int _itemCount;
        private int _maxContainedItemCount;
        private int _registratorStack;

        public ItemContainer(Tile tile)
        {
            _tile = tile;
        }
        
        public bool ContainsItem()
        {
            return _pooledGameObject != null && ContainedBaseItem != null;
        }

        public void InitializeItem(BaseItem_SO newBaseItem, Poolable pooledGameObject, int maxContainedItemCount = 1, int itemCount = 1)
        {
            if (ContainsItem()) return;

            _itemCount = itemCount;
            _maxContainedItemCount = maxContainedItemCount;
            ContainedBaseItem = newBaseItem;
            _pooledGameObject = pooledGameObject;
        }
        
        public bool CanDestroyItem()
        {
            if (!ContainsItem()) return false;
            
            if (_registratorStack > 0)
            {
                Debug.LogWarning($"There are still registrators linked with this item!");
                return false;
            }

            return true;
        }

        public void DestroyItem()
        {
            if (!CanDestroyItem()) return;
            
            _pooledGameObject.Release();
            ContainedBaseItem = null;
            _maxContainedItemCount = 0;
        }

        public void AddItemCount(BaseItem_SO baseItem, int change)
        {
            if (CanAddItemCount(change) && IsItemFit(baseItem))
            {
                _itemCount += change;
            }
        }

        public bool IsItemFit(BaseItem_SO baseItem)
        {
            return ContainedBaseItem == baseItem;
        }

        public bool CanAddItemCount(int change)
        {
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

        public void AddRegistratorStack()
        {
            _registratorStack++;
        }
        
        public void RemoveRegistratorStack()
        {
            _registratorStack--;
        }
    }
}