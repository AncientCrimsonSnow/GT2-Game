using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using Features.TileSystem.Scripts.Registrator;
using UnityEngine;

namespace Features.TileSystem.Scripts
{
    public class PointerResourceGeneratorTileInteractable : ITileInteractable
    {
        private readonly Tile _tile;
        private readonly bool _isMovable;
        private readonly List<BaseTileRegistrator> _itemTilePointers;
        private readonly BaseItem_SO _baseItemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileInteractable(Tile tile, bool isMovable, List<BaseTileRegistrator> itemTilePointers, BaseItem_SO baseItemLoot, int itemAmountCost)
        {
            _tile = tile;
            _isMovable = isMovable;
            _itemTilePointers = itemTilePointers;
            _baseItemLoot = baseItemLoot;
            _itemAmountCost = itemAmountCost;
        }

        public bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointers.All(x => x.Tile.ItemContainer.CanAddItemCount(x.Tile.ItemContainer.ContainedBaseItem, -_itemAmountCost)) 
                || !_tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return false;

            RemoveItemFromPointer();
            DropItemOnTile();
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
        
        private void RemoveItemFromPointer()
        {
            foreach (var baseTileRegistrator in _itemTilePointers)
            {
                baseTileRegistrator.Tile.ItemContainer.AddItemCount(baseTileRegistrator.Tile.ItemContainer.ContainedBaseItem, -_itemAmountCost);
            }
            Debug.Log("Removed item from pointer.");
        }

        private void DropItemOnTile()
        {
            _tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(_tile));
            TileHelper.InstantiateOnTile(_tile, _baseItemLoot.prefab, Quaternion.identity);
            Debug.Log("Dropped item by crafting.");
        }
    }
}