namespace Features.TileSystem
{
    public interface IExchangeable<in T>
    {
        bool IsExchangeable(T newBaseTileComponent);

        void OnExchange(T newBaseTileComponent);
    }
}