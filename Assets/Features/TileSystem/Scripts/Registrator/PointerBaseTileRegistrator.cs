using Features.Items.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TileSystem.Scripts.Registrator
{
    public class PointerBaseTileRegistrator : BaseTileRegistrator
    {
        [SerializeField] private bool isMovable;
        [FormerlySerializedAs("itemLoot")] [SerializeField] private BaseItem_SO baseItemLoot;

        [SerializeField] private BaseTileRegistrator pointerRegistrator;
        [SerializeField] private int craftAmount;

        private ITileInteractable _tileInteractable;

        protected override void InternalRegisterOnTile()
        {
            var tileComponent = new PointerResourceGeneratorTileInteractable(Tile, isMovable, pointerRegistrator.Tile, baseItemLoot, craftAmount);
            Tile.RegisterTileInteractable(tileComponent);
            _tileInteractable = tileComponent;
        }

        protected override void UnregisterOnTile()
        {
            Tile.UnregisterTileInteractable(_tileInteractable);
        }
    }
}