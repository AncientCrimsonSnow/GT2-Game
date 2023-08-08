using System.Collections.Generic;
using System.Linq;
using Features.Items.Scripts;
using Features.TileSystem.Scripts.Registrator;
using Uilities.Pool;
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
        private readonly Poolable _poolable;
        private readonly bool _destroyPointerIfEmpty;

        public PointerResourceGeneratorTileInteractable(Tile tile, bool isMovable, List<BaseTileRegistrator> itemTilePointers, 
            BaseItem_SO baseItemLoot, int itemAmountCost, Poolable poolable, bool destroyPointerIfEmpty)
        {
            _tile = tile;
            _isMovable = isMovable;
            _itemTilePointers = itemTilePointers;
            _baseItemLoot = baseItemLoot;
            _itemAmountCost = itemAmountCost;
            _poolable = poolable;
            _destroyPointerIfEmpty = destroyPointerIfEmpty;
        }

        public bool CanInteract(GameObject interactor, out string interactionText)
        {
            interactionText = "";
            if (!_itemTilePointers.All(x => x.Tile.ItemContainer.CanAddItemCount(-_itemAmountCost)
                                            || !_tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>())) return false;

            interactionText = "Generate";
            Debug.Log(interactionText);
            return true;
        }

        public bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointers.All(x => x.Tile.ItemContainer.CanAddItemCount(-_itemAmountCost)
                                            || !_tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>())) return false;

            RemoveItemFromPointer();
            DropItemOnTile();
            return true;
        }

        public bool CanCast(GameObject caster, out string interactionText)
        {
            interactionText = "";
            return false;
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
                
                if (!baseTileRegistrator.Tile.ItemContainer.CanAddItemCount(-_itemAmountCost) && _destroyPointerIfEmpty)
                {
                    _poolable.Release();
                }
            }
            //Debug.Log("Removed item from pointer.");
        }

        private void DropItemOnTile()
        {
            TileHelper.ReuseOnTile(_tile, _baseItemLoot.prefab, Quaternion.identity);
            
            //Debug.Log("Dropped item by crafting.");
        }
    }
}