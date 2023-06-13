using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.ItemSystem
{
    public class ItemContainer
    {
        public BaseItem_SO ContainedBaseItem { get; private set; }
        
        private readonly Tile _tile;
        
        private GameObject _instantiatedGameObject;
        private int _itemCount;
        private int _maxContainedItemCount;
        private int _registratorStack;

        public ItemContainer(Tile tile)
        {
            _tile = tile;
        }
        
        public bool ContainsItem()
        {
            return _instantiatedGameObject != null && ContainedBaseItem != null;
        }

        public void InitializeItem(BaseItem_SO newBaseItem, GameObject instantiatedObject, int maxContainedItemCount = 1, int itemCount = 1)
        {
            if (ContainsItem()) return;

            _itemCount = itemCount;
            _maxContainedItemCount = maxContainedItemCount;
            ContainedBaseItem = newBaseItem;
            _instantiatedGameObject = instantiatedObject;
        }

        public void InitializeItem(BaseItem_SO newBaseItem, int maxItemCount = 1, int itemCount = 1)
        {
            if (ContainsItem()) return;

            _itemCount = itemCount;
            _maxContainedItemCount = maxItemCount;
            ContainedBaseItem = newBaseItem;
            _instantiatedGameObject = TileHelper.InstantiateOnTile(_tile, newBaseItem.prefab, Quaternion.identity);
        }
        
        public bool CanDestroyItem(int maxItemDestructionCount)
        {
            if (_itemCount > maxItemDestructionCount)
            {
                Debug.LogWarning($"You can only destroy the Item, when there are at most {maxItemDestructionCount} items on it! There are/is currently {_itemCount}");
                return false;
            }

            if (_registratorStack > 0)
            {
                Debug.LogWarning($"There are still registrators linked with this item!");
                return false;
            }

            return true;
        }

        public void DestroyItem(int maxItemDestructionCount)
        {
            if (!CanDestroyItem(maxItemDestructionCount)) return;
            
            Object.Destroy(_instantiatedGameObject);
            ContainedBaseItem = null;
            _maxContainedItemCount = 0;
        }

        public void AddItemCount(BaseItem_SO baseItem, int change)
        {
            if (CanAddItemCount(baseItem, change))
            {
                _itemCount += change;
            }
        }

        public bool CanAddItemCount(BaseItem_SO baseItem, int change)
        {
            if (ContainedBaseItem != baseItem)
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