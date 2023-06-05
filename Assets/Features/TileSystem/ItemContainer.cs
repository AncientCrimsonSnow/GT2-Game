using System.Collections.Generic;
using UnityEngine;

namespace Features.TileSystem
{
    public class ItemContainer
    {
        public Item ContainedItem { get; private set; }
        
        private readonly Tile _tile;
        
        private GameObject _instantiatedGameObject;
        private int _itemCount;
        private int _maxItemCount;

        private List<TileComponentRegistrator> _registrators;

        public ItemContainer(Tile tile)
        {
            _tile = tile;
            _registrators = new List<TileComponentRegistrator>();
        }

        public bool ContainsItem()
        {
            return _instantiatedGameObject != null && ContainedItem != null;
        }

        public void InitializeItem(Item newItem, int maxItemCount = 1, int itemCount = 1)
        {
            if (!ContainsItem()) return;

            _itemCount = itemCount;
            _maxItemCount = maxItemCount;
            ContainedItem = newItem;
            _instantiatedGameObject = TileHelper.InstantiateOnTile(_tile, newItem.prefab, Quaternion.identity);
        }
        
        public bool CanDestroyItem()
        {
            return ContainsItem() && _itemCount == 0 && _registrators.Count == 1;
        }

        public void DestroyItem()
        {
            if (!CanDestroyItem()) return;
            
            Object.Destroy(_instantiatedGameObject);
            ContainedItem = null;
            _maxItemCount = 0;
        }

        public void AddItemCount(int change)
        {
            if (CanAddItemCount(change))
            {
                _itemCount += change;
            }
        }

        public bool CanAddItemCount(int change)
        {
            return _itemCount + change >= 0 && _itemCount + change <= _maxItemCount;
        }
        
        public void AddRegistrator(TileComponentRegistrator tileComponentRegistrator)
        {
            _registrators.Add(tileComponentRegistrator);
        }
        
        public void RemoveRegistrator(TileComponentRegistrator tileComponentRegistrator)
        {
            _registrators.Remove(tileComponentRegistrator);
        }
    }
}