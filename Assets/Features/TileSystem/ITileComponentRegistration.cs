namespace Features.TileSystem
{
    public interface ITileComponentRegistration
    {
        public void RegisterTileComponent(ITileComponent newTileComponent);

        public void UnregisterTileComponent(ITileComponent newTileComponent);
    }
}