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
    
        public override bool CanRegisterTileInteractable()
        {
            _canBeUnregistered = Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
            return _canBeUnregistered;
        }

        public override void RegisterTileInteractable()
        {
            ItemTileInteractable tileComponent = useThisGameObject ? new UnstackableItemTileInteractable(Tile, isMovable, baseItemType, gameObject) 
                : new UnstackableItemTileInteractable(Tile, isMovable, baseItemType);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        public override bool CanUnregisterTileInteractable()
        {
            return Tile.ItemContainer.CanDestroyItem(1) && _canBeUnregistered;
        }

        public override void UnregisterTileInteractable()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
