namespace Features.TileSystem
{
    public abstract class ExchangeableBaseTileComponent : BaseTileComponent, IExchangeable<BaseTileComponent>
    {
        protected ExchangeableBaseTileComponent(Tile tile) : base(tile) { }
        
        public abstract bool IsExchangeable(BaseTileComponent newBaseTileComponent);
        
        public abstract void OnExchange(BaseTileComponent newBaseTileComponent);
    }
}