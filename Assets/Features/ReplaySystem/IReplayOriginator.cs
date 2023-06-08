using System;
using System.Collections.Generic;

namespace NewReplaySystem
{
    public interface IReplayOriginator
    {
        /// <summary>
        /// Method for passing a record towards the ReplayManager as an event.
        /// </summary>
        Action<IRecordSnapshot> ProvideReplaySnapshot { get; set; }
        
        //TODO: make this also execute a tick
    }
}
