using UnityEngine;

namespace Features.TileSystem
{
    public class PointerTileComponentRegistrator : TileComponentRegistrator
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private Item itemLoot;

        [SerializeField] private TileComponentRegistrator pointerRegistrator;
        [SerializeField] private int craftAmount;

        protected override ITileComponent RegisterTileComponent(Tile tile)
        {
            var tileComponent = new PointerResourceGeneratorTileComponent(tile, isMovable, pointerRegistrator.Tile, itemLoot, craftAmount);
            tile.RegisterTileComponent(tileComponent);
            return tileComponent;
        }

        protected override void UnregisterTileComponent(Tile tile, ITileComponent tileComponent)
        {
            tile.UnregisterTileComponent(tileComponent);
        }
    }
}