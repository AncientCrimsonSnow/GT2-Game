using System;
using System.Collections.Generic;

namespace NewReplaySystem
{
    public interface IReplayable
    {
        /// <summary>
        /// Method for passing a record towards the ReplayManager as an event.
        /// </summary>
        Action<IRecord> ProvideReplayEventFrames { get; set; }

        /// <summary>
        /// Method for passing records towards the ReplayManager. Will get called inside a LateUpdate.
        /// </summary>
        /// <returns> Passes Data for the ReplayManager this script got registered for.</returns>
        IEnumerable<IRecord> CreateSnapshot();
    }
}
