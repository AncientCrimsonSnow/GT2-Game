namespace Features.TileSystem
{
    public interface ITileComponentRegistration
    {
        public bool TryRegisterTileComponent(BaseTileComponent newExchangeable);

        public bool TryUnregisterTileComponent(BaseTileComponent newExchangeable);
    }
}