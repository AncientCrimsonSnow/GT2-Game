namespace Features.TileSystem.Scripts.Registrator
{
    public interface ITileComponentRegistration
    {
        public void RegisterTileInteractable(ITileInteractable newTileInteractable);

        public void UnregisterTileInteractable(ITileInteractable newTileInteractable);
    }
}