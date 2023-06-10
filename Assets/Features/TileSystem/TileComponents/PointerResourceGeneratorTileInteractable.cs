using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class PointerResourceGeneratorTileInteractable : ItemTileInteractable
    {
        private readonly bool _isMovable;
        private readonly Tile _itemTilePointer;
        private readonly Item _itemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileInteractable(Tile tile, bool isMovable, Tile itemTilePointer, Item itemLoot, int itemAmountCost) : base(tile)
        {
            _isMovable = isMovable;
            _itemTilePointer = itemTilePointer;
            _itemLoot = itemLoot;
            _itemAmountCost = itemAmountCost;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointer.ItemContainer.CanAddItemCount(_itemTilePointer.ItemContainer.ContainedItem, -_itemAmountCost) || Tile.ItemContainer.ContainsItem()) return false;

            RemovePointerTileItem();
            InitializeSelfTileItem();
            return true;
        }

        private void RemovePointerTileItem()
        {
            _itemTilePointer.ItemContainer.AddItemCount(_itemTilePointer.ItemContainer.ContainedItem, -_itemAmountCost);
            Debug.Log("Removed item from pointer.");
        }

        private void InitializeSelfTileItem()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(Tile, _itemLoot));
            Debug.Log("Dropped item by crafting.");
        }
        
        public override bool IsMovable()
        {
            return _isMovable;
        }
    }
}