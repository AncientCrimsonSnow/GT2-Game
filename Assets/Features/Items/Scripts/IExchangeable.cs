namespace Features.Items.Scripts
{
    public interface IExchangeable<in T>
    {
        bool IsExchangeable(T newBaseTileComponent);
    }
}