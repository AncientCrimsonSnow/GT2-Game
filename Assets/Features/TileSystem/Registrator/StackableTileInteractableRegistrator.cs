using Features.TileSystem.TileComponents;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class StackableTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private Item.Item itemType;
        [SerializeField] private bool useThisGameObject;
        [SerializeField] private int containedItemAmountOnSpawn;
        [SerializeField] private int maxContainedItemCount;

        protected override bool CanRegisterTileInteractable(Tile.Tile tile)
        {
            return !tile.ItemContainer.ContainsItem() || tile.TryGetFirstTileInteractableOfType(out StackableItemTileInteractable _);
        }

        protected override ITileInteractable RegisterTileInteractable(Tile.Tile tile)
        {
            if (tile.ItemContainer.ContainsItem())
            {
                if (tile.TryGetFirstTileInteractableOfType(out StackableItemTileInteractable stackableItemTileComponent))
                {
                    tile.ItemContainer.AddRegistrator(this);
                    return stackableItemTileComponent;
                }
                
                Debug.LogError("On the ItemContainer is currently an item. You need to remove it, until you can Initialize a new Item on it!");
                return null;
            }
            
            tile.ItemContainer.AddRegistrator(this);
            if (useThisGameObject)
            {
                tile.ItemContainer.InitializeItem(itemType, gameObject, maxContainedItemCount, containedItemAmountOnSpawn);
            }
            else
            {
                tile.ItemContainer.InitializeItem(itemType, maxContainedItemCount, containedItemAmountOnSpawn);
            }
            var tileComponent = new StackableItemTileInteractable(tile);
            tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(tileComponent);
            return tileComponent;
        }

        protected override bool CanUnregisterTileInteractable(ITileInteractable tileInteractable)
        {
            return Tile.ItemContainer.CanDestroyItem(0);
        }

        protected override void UnregisterTileInteractable(ITileInteractable tileInteractable)
        {
            if (!Tile.ItemContainer.CanDestroyItem(0))
            {
                Debug.LogWarning("On the ItemContainer is no item to remove!");
                return;
            }
            
            Tile.ItemContainer.RemoveRegistrator(this);
            Tile.ItemContainer.DestroyItem(0);
            Tile.ExchangeFirstTileInteractableOfType<ItemTileInteractable>(new EmptyItemTileInteractable(Tile));
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Tile.ItemContainer.RemoveRegistrator(this);
        }
    }
}