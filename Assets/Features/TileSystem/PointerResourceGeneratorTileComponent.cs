using UnityEngine;

namespace Features.TileSystem
{
    public class PointerResourceGeneratorTileComponent : BaseTileComponent, IInstantiatedGameObject
    {
        public GameObject InstantiatedGameObject { get; }
        
        private readonly bool _isMovable;
        private readonly StackableBaseTileComponent _stackableBaseTileComponentPointer;
        private readonly Item _item;
        private readonly int _itemAmountCost;
        
        public PointerResourceGeneratorTileComponent(Tile tile, GameObject instantiatedGameObject, bool isMovable,
            StackableBaseTileComponent stackableBaseTileComponentPointer, Item item, int itemAmountCost) : base(tile)
        {
            _isMovable = isMovable;
            InstantiatedGameObject = instantiatedGameObject;
            _stackableBaseTileComponentPointer = stackableBaseTileComponentPointer;
            _item = item;
            _itemAmountCost = itemAmountCost;
        }

        public override bool TryInteract(GameObject interactor)
        {
            if (!_stackableBaseTileComponentPointer.IsSuccessfulItemRemove(_itemAmountCost)) return false;

            var instantiatedObject = TileHelper.InstantiateOnTile(Tile, _item.prefab, Quaternion.identity);
            return Tile.TryRegisterTileComponent(new UnstackableBaseTileComponent(Tile, _item, instantiatedObject));
        }
        
        public override bool IsMovable()
        {
            return _isMovable;
        }
    }
}