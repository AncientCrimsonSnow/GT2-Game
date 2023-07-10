namespace Uilities.Pool
{
    public interface IBeforeReusePoolableCallback
    {
        void OnBeforeReusePoolable();
    }
    
    public interface IBeforeReleasePoolableCallback
    {
        void OnBeforeReleasePoolable();
    }
}
