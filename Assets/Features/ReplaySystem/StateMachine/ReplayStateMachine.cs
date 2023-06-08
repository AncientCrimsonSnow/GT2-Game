using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayStateMachine
    {
        private readonly ReplayController _replayController;
        
        private IReplaySequenceState _currentReplayState;
        
        
        private int _currentReplaySequenceStateIndex;

        public ReplayStateMachine(IReplaySequenceState currentReplayState)
        {
            _currentReplayState = currentReplayState;
        }

        public bool IsInRequestedState(params Type[] requestedTypes)
        {
            return requestedTypes.Length != 0 && requestedTypes.Any(type => type == _currentReplayState.GetType());
        }

        public void ChangeState(IReplaySequenceState newReplayState)
        {
            _currentReplayState.Exit();
            _currentReplayState = newReplayState;
            newReplayState.Enter();
        }

        public void Tick(float totalTickTime)
        {
            _currentReplayState.Tick(totalTickTime);
        }
    }
}
