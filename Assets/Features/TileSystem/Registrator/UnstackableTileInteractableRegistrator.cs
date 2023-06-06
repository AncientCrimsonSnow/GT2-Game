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
    
        protected override bool CanRegisterTileInteractable(Tile tile)
        {
            return !tile.ItemContainer.ContainsItem();
        }

        protected override ITileInteractable RegisterTileInteractable(Tile tile)
        {
            if (tile.ItemContainer.ContainsItem()) return null;
        
            if (useThisGameObject)
            {
                tile.ItemContainer.InitializeItem(itemType, gameObject);
            }
            else
            {
                tile.ItemContainer.InitializeItem(itemType);
            }
            var tileComponent = new UnstackableItemTileInteractable(tile);
            tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(tileComponent);
            return tileComponent;
        }

        protected override bool CanUnregisterTileInteractable(ITileInteractable tileInteractable)
        {
            return Tile.ItemContainer.CanDestroyItem(1);
        }

        protected override void UnregisterTileInteractable(ITileInteractable tileInteractable)
        {
            if (!Tile.ItemContainer.CanDestroyItem(1))
            {
                Debug.LogError("Cant destroy the item");
                return;
            }
        
            Tile.ItemContainer.DestroyItem(1);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}
