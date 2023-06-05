namespace Features.TileSystem
{
    public interface IExchangeable<in T>
    {
        bool IsExchangeable(T newBaseTileComponent);
    }
}