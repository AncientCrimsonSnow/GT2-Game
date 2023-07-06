using Features.Items.Scripts;
using UnityEngine;

namespace Features.TileSystem.Scripts
{
    public class PointerResourceGeneratorTileInteractable : ITileInteractable
    {
        private readonly Tile _tile;
        private readonly bool _isMovable;
        private readonly Tile _itemTilePointer;
        private readonly BaseItem_SO _baseItemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileInteractable(Tile tile, bool isMovable, Tile itemTilePointer, BaseItem_SO baseItemLoot, int itemAmountCost)
        {
            _tile = tile;
            _isMovable = isMovable;
            _itemTilePointer = itemTilePointer;
            _baseItemLoot = baseItemLoot;
            _itemAmountCost = itemAmountCost;
        }

        public bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointer.ItemContainer.CanAddItemCount(_itemTilePointer.ItemContainer.ContainedBaseItem, -_itemAmountCost) 
                || !_tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return false;

            RemovePointerTileItem();
            InitializeSelfTileItem();
            return true;
        }

        public bool TryCast(GameObject caster)
        {
            return false;
        }

        public bool IsMovable()
        {
            return _isMovable;
        }
        
        private void RemovePointerTileItem()
        {
            _itemTilePointer.ItemContainer.AddItemCount(_itemTilePointer.ItemContainer.ContainedBaseItem, -_itemAmountCost);
            Debug.Log("Removed item from pointer.");
        }

        private void InitializeSelfTileItem()
        {
            _tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(_tile));
            TileHelper.InstantiateOnTile(_tile, _baseItemLoot.prefab, Quaternion.identity);
            Debug.Log("Dropped item by crafting.");
        }
    }
}