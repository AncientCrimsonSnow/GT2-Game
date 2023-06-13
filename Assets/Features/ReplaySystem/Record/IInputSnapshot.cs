
namespace Features.ReplaySystem.Record
{
    public interface IInputSnapshot
    {
        void Tick(float tickDurationInSeconds);
    }
}
