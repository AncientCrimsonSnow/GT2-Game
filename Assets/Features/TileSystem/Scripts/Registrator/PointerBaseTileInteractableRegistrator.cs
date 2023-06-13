using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class PointerBaseTileInteractableRegistrator : BaseTileInteractableRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemLoot")] [SerializeField] private BaseItem_SO baseItemLoot;

        [SerializeField] private BaseTileInteractableRegistrator pointerRegistrator;
        [SerializeField] private int craftAmount;

        private ITileInteractable _tileInteractable;

        protected override void RegisterTileInteractable()
        {
            var tileComponent = new PointerResourceGeneratorTileInteractable(Tile, isMovable, pointerRegistrator.Tile, baseItemLoot, craftAmount);
            Tile.RegisterTileInteractable(tileComponent);
            _tileInteractable = tileComponent;
        }

        protected override void UnregisterTileInteractable()
        {
            Tile.UnregisterTileInteractable(_tileInteractable);
        }
    }
}