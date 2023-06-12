using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class PointerResourceGeneratorTileInteractable : ItemTileInteractable
    {
        private readonly Tile _itemTilePointer;
        private readonly BaseItem _baseItemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileInteractable(Tile tile, bool isMovable, Tile itemTilePointer, BaseItem baseItemLoot, int itemAmountCost) : base(tile, isMovable)
        {
            _itemTilePointer = itemTilePointer;
            _baseItemLoot = baseItemLoot;
            _itemAmountCost = itemAmountCost;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointer.ItemContainer.CanAddItemCount(_itemTilePointer.ItemContainer.ContainedBaseItem, -_itemAmountCost) 
                || !Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return false;

            RemovePointerTileItem();
            InitializeSelfTileItem();
            return true;
        }

        public override bool TryCastMagic(GameObject caster)
        {
            return false;
        }

        private void RemovePointerTileItem()
        {
            _itemTilePointer.ItemContainer.AddItemCount(_itemTilePointer.ItemContainer.ContainedBaseItem, -_itemAmountCost);
            Debug.Log("Removed item from pointer.");
        }

        private void InitializeSelfTileItem()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(Tile, IsMovable(), _baseItemLoot));
            Debug.Log("Dropped item by crafting.");
        }
    }
}