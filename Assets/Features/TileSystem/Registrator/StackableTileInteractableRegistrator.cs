using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class StackableTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private Item itemType;
        [SerializeField] private bool useThisGameObject;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        private bool _canBeUnregistered;

        protected override bool CanRegisterTileInteractable()
        {
            _canBeUnregistered = Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>() || 
                                 Tile.ContainsTileInteractableOfType<StackableItemTileInteractable>() && Tile.ItemContainer.ContainedItem == itemType;
            return _canBeUnregistered;
        }

        protected override void RegisterTileInteractable()
        {
            Tile.ItemContainer.AddRegistratorStack();
            
            if (!Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>()) return;

            ItemTileInteractable tileComponent = useThisGameObject ?
                new StackableItemTileInteractable(Tile, isMovable, itemType, maxContainedItemCount, containedItemAmountOnSpawn, gameObject) 
                : new StackableItemTileInteractable(Tile, isMovable, itemType, maxContainedItemCount, containedItemAmountOnSpawn);
            Tile.ExchangeFirstTileInteractableOfType(tileComponent);
        }

        protected override bool CanUnregisterTileInteractable()
        {
            return _canBeUnregistered;
        }

        protected override void UnregisterTileInteractable()
        {
            Tile.ItemContainer.RemoveRegistratorStack();
            
            if (!Tile.ItemContainer.CanDestroyItem(0)) return;
            
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
    }
}