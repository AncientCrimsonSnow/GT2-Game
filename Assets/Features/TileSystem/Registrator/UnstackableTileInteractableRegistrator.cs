using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Registrator
{
    public class UnstackableTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem baseItemType;
        [SerializeField] private bool useThisGameObject;
        
        private bool _canBeUnregistered;
    
        protected override bool CanRegisterTileInteractable()
        {
            _canBeUnregistered = Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
            return _canBeUnregistered;
        }

        protected override void RegisterTileInteractable()
        {
            ItemTileInteractable tileComponent = useThisGameObject ? new UnstackableItemTileInteractable(Tile, isMovable, baseItemType, gameObject) 
                : new UnstackableItemTileInteractable(Tile, isMovable, baseItemType);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override bool CanUnregisterTileInteractable()
        {
            return Tile.ItemContainer.CanDestroyItem(1) && _canBeUnregistered;
        }

        protected override void UnregisterTileInteractable()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
