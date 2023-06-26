using System;
using Features.ReplaySystem.Record;

namespace Features.ReplaySystem
{
    public interface IReplayOriginator
    {
        /// <summary>
        /// Method for passing a record towards the ReplayManager as an event.
        /// </summary>
        Action<IInputSnapshot> PushNewTick { get; set; }
    }
}
