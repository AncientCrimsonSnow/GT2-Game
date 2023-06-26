using Features.ReplaySystem.Record;

namespace Features.Character.Scripts
{
    public class EmptySnapshot : IInputSnapshot
    {
        public void Tick(float tickDurationInSeconds) { }
    }
}
