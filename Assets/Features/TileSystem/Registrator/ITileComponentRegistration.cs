using Features.TileSystem.TileComponents;

namespace Features.TileSystem.Registrator
{
    public interface ITileComponentRegistration
    {
        public void RegisterTileInteractable(ITileInteractable newTileInteractable);

        public void UnregisterTileInteractable(ITileInteractable newTileInteractable);
    }
}