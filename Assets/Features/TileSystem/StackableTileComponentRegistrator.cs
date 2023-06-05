using UnityEngine;

namespace Features.TileSystem
{
    public class StackableTileComponentRegistrator : TileComponentRegistrator
    {
        [SerializeField] private Item itemType;

        protected override bool CanRegisterTileComponent(Tile tile)
        {
            return tile.ItemContainer.ContainsItem() || tile.TryGetFirstTileComponentOfType(out StackableItemTileComponent stackableItemTileComponent);
        }

        protected override ITileComponent RegisterTileComponent(Tile tile)
        {
            if (!tile.ItemContainer.ContainsItem())
            {
                if (tile.TryGetFirstTileComponentOfType(out StackableItemTileComponent stackableItemTileComponent))
                {
                    tile.ItemContainer.AddRegistrator(this);
                    return stackableItemTileComponent;
                }
                
                Debug.LogError("On the ItemContainer is currently an item. You need to remove it, until you can Initialize a new Item on it!");
                return null;
            }
            
            tile.ItemContainer.AddRegistrator(this);
            tile.ItemContainer.InitializeItem(itemType, itemType.maxStack);
            var tileComponent = new StackableItemTileComponent(tile);
            tile.ExchangeFirstTileComponentOfType<ItemTileComponent>(tileComponent);
            return tileComponent;
        }

        protected override void UnregisterTileComponent(Tile tile, ITileComponent tileComponent)
        {
            tile.ItemContainer.RemoveRegistrator(this);
            
            if (!tile.ItemContainer.CanDestroyItem())
            {
                Debug.LogWarning("On the ItemContainer is no item to remove!");
                return;
            }
            
            tile.ItemContainer.DestroyItem();
            tile.ExchangeFirstTileComponentOfType<ItemTileComponent>(new EmptyItemTileComponent(tile));
        }
    }
}