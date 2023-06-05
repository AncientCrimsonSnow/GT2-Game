using UnityEngine;

namespace Features.TileSystem
{
    public class PointerResourceGeneratorTileComponent : ItemTileComponent
    {
        private readonly bool _isMovable;
        private readonly Tile _itemTilePointer;
        private readonly Item _itemLoot;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileComponent(Tile tile, bool isMovable, Tile itemTilePointer, Item itemLoot, int itemAmountCost) : base(tile)
        {
            _isMovable = isMovable;
            _itemTilePointer = itemTilePointer;
            _itemLoot = itemLoot;
            _itemAmountCost = itemAmountCost;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!_itemTilePointer.ItemContainer.CanAddItemCount(_itemAmountCost) && !Tile.ItemContainer.ContainsItem()) return false;
            
            RemovePointerTileItem();
            InitializeSelfTileItem();
            return true;
        }

        private void RemovePointerTileItem()
        {
            _itemTilePointer.ItemContainer.AddItemCount(_itemAmountCost);
        }

        private void InitializeSelfTileItem()
        {
            Tile.ItemContainer.InitializeItem(_itemLoot);
        }
        
        public override bool IsMovable()
        {
            return _isMovable;
        }
    }
}