using System.Collections.Generic;

namespace NewReplaySystem
{
    public interface IReplaySequenceState
    {
        void Enter();
        void Tick(float totalTickTime);
        void Exit();
    }
}
