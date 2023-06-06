using UnityEngine;

namespace Features.TileSystem.TileComponents
{
    public class PointerResourceGeneratorTileInteractable : ItemTileInteractable
    {
        private readonly bool _isMovable;
        private readonly Tile.Tile _itemTilePointer;
        private readonly Item.Item _itemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileInteractable(Tile.Tile tile, bool isMovable, Tile.Tile itemTilePointer, Item.Item itemLoot, int itemAmountCost) : base(tile)
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
        }

        private void InitializeSelfTileItem()
        {
            Tile.ItemContainer.InitializeItem(_itemLoot);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new UnstackableItemTileInteractable(Tile));
        }
        
        public override bool IsMovable()
        {
            return _isMovable;
        }
    }
}