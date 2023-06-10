using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using Features.TileSystem.TileSystem;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class PointerTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private Item itemLoot;

        [SerializeField] private TileInteractableRegistrator pointerRegistrator;
        [SerializeField] private int craftAmount;

        protected override ITileInteractable RegisterTileInteractable(Tile tile)
        {
            var tileComponent = new PointerResourceGeneratorTileInteractable(tile, isMovable, pointerRegistrator.Tile, itemLoot, craftAmount);
            tile.RegisterTileInteractable(tileComponent);
            return tileComponent;
        }

        protected override void UnregisterTileInteractable(ITileInteractable tileInteractable)
        {
            Tile.UnregisterTileInteractable(tileInteractable);
        }
    }
}