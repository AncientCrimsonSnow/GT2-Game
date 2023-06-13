using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class UnstackableBaseTileInteractableRegistrator : BaseTileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemType")] [SerializeField] private BaseItem_SO baseItemType;
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
