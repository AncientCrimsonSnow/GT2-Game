using Features.Items.Scripts;

namespace Features.TileSystem.Scripts.Registrator
{
    public class BlockedTileRegistrator : BaseTileRegistrator
    {
        private ITileInteractable _tileInteractable;

        public override bool CanRegisterOnTile()
        {
            return base.CanRegisterOnTile() && Tile.ContainsTileInteractableOfType<EmptyItemTileInteractable>();
        }

        protected override void InternalRegisterOnTile()
        {
            var tileComponent = new BlockedTileInteractable();
            Tile.RegisterTileInteractable(tileComponent);
            _tileInteractable = tileComponent;
        }

        protected override void UnregisterOnTile()
        {
            Tile.UnregisterTileInteractable(_tileInteractable);
        }
    }
}
