using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class UnstackableBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem_SO baseItemType;
        [SerializeField] private bool useThisGameObject;
        
        private bool _canBeUnregistered;
    
        public override bool CanRegisterOnTile()
        {
            _canBeUnregistered = Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
            return _canBeUnregistered;
        }

        protected override void InternalRegisterOnTile()
        {
            ItemTileInteractable tileComponent = useThisGameObject ? new UnstackableItemTileInteractable(Tile, isMovable, baseItemType, gameObject) 
                : new UnstackableItemTileInteractable(Tile, isMovable, baseItemType);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override bool CanUnregisterOnTile()
        {
            return Tile.ItemContainer.CanDestroyItem(1) && _canBeUnregistered;
        }

        protected override void UnregisterOnTile()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
