namespace Features.TileSystem
{
    public interface ITileContextRegistration
    {
        public void RegisterTileContext(ITileInteractionContext tileInteractionContext);

        public void UnregisterTileContext(ITileInteractionContext tileInteractionContext);
    }
}