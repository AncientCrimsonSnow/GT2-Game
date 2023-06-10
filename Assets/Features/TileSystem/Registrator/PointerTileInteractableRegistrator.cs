using Features.TileSystem.ItemSystem;
using Features.TileSystem.TileComponents;
using UnityEngine;

namespace Features.TileSystem.Registrator
{
    public class PointerTileInteractableRegistrator : TileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [SerializeField] private Item itemLoot;

        [SerializeField] private TileInteractableRegistrator pointerRegistrator;
        [SerializeField] private int craftAmount;

        private ITileInteractable _tileInteractable;

        protected override void RegisterTileInteractable()
        {
            var tileComponent = new PointerResourceGeneratorTileInteractable(Tile, isMovable, pointerRegistrator.Tile, itemLoot, craftAmount);
            Tile.RegisterTileInteractable(tileComponent);
            _tileInteractable = tileComponent;
        }

        protected override void UnregisterTileInteractable()
        {
            Tile.UnregisterTileInteractable(_tileInteractable);
        }
    }
}