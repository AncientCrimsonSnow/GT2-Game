using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class UnstackableTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private Item itemType;
        [SerializeField] private bool useThisGameObject;
    
        protected override bool CanRegisterTileInteractable()
        {
            return !Tile.ItemContainer.ContainsItem();
        }

        protected override void RegisterTileInteractable()
        {
            ItemTileInteractable tileComponent = useThisGameObject ? new UnstackableItemTileInteractable(Tile, itemType, gameObject) 
                : new UnstackableItemTileInteractable(Tile, itemType);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override bool CanUnregisterTileInteractable()
        {
            return Tile.ItemContainer.CanDestroyItem(1);
        }

        protected override void UnregisterTileInteractable()
        {
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
