using Features.TileSystem.Scripts;
using Features.TileSystem.Scripts.Registrator;

public class BlockedTileRegistrator : BaseTileRegistrator
{
    private ITileInteractable _tileInteractable;
    
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
