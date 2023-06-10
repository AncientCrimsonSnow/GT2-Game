using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class StackableTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private Item itemType;
        [SerializeField] private bool useThisGameObject;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        protected override void RegisterTileInteractable()
        {
            if (!Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;

            ItemTileInteractable tileComponent = useThisGameObject ? 
                new StackableItemTileInteractable(Tile, itemType, maxContainedItemCount, containedItemAmountOnSpawn, gameObject) 
                : new StackableItemTileInteractable(Tile, itemType, maxContainedItemCount, containedItemAmountOnSpawn);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(tileComponent);
        }

        protected override void UnregisterTileInteractable()
        {
            if (!Tile.ItemContainer.CanDestroyItem(0) || !Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}