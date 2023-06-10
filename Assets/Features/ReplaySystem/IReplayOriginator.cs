using System;
using System.Collections.Generic;

namespace NewReplaySystem
{
    public interface IReplayOriginator
    {
        /// <summary>
        /// Method for passing a record towards the ReplayManager as an event.
        /// </summary>
        Action<IInputSnapshot> PushNewTick { get; set; }
    }
}
