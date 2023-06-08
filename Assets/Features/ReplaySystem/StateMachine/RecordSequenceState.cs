using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class RecordSequenceState : IReplaySequenceState
    {
        private readonly ReplayController _replayController;
        
        public RecordSequenceState(ReplayController replayController)
        {
            _replayController = replayController;
        }
        
        public void Enter() {}
        
        public void Tick(float totalTickTime)
        {
            throw new System.NotImplementedException();
        }
        public void Exit() { }
    }
}
