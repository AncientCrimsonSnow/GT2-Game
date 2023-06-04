using UnityEngine;

namespace Features.TileSystem
{
    public class PointerResourceGeneratorTileComponent : BaseTileComponent, IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; }
        
        private readonly bool _isMovable;
        private readonly StackableItemTileComponent _stackableItemTileComponentPointer;
        private readonly Item _item;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileComponent(Tile tile, GameObject instantiatedGameObject, bool isMovable,
            StackableItemTileComponent stackableItemTileComponentPointer, Item item, int itemAmountCost) : base(tile)
        {
            _isMovable = isMovable;
            InstantiatedGameObject = instantiatedGameObject;
            _stackableItemTileComponentPointer = stackableItemTileComponentPointer;
            _item = item;
            _itemAmountCost = itemAmountCost;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!_stackableItemTileComponentPointer.IsSuccessfulItemRemove(_itemAmountCost)) return false;

            var instantiatedObject = TileHelper.InstantiateOnTile(Tile, _item.prefab, Quaternion.identity);
            return Tile.TryRegisterTileComponent(new UnstackableItemTileComponent(Tile, _item, instantiatedObject));
        }
        
        public override bool IsMovable()
        {
            return _isMovable;
        }
    }
}