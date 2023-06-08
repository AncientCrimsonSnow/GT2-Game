using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ForwardReplaySequenceState : IReplaySequenceState
    {
        private readonly ReplayController _replayController;
        
        public ForwardReplaySequenceState(ReplayController replayController)
        {
            _replayController = replayController;
        }

        public void Enter()
        {
            
        }

        public void Tick(float totalTickTime)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCurrentStateCompleted(float advancedTime)
        {
            return _replayController.TickEndingExclusive <= advancedTime;
        }
        
        public void LateUpdate(float advancedTime, List<IReplayable> registeredReplayables)
        {
            while (_frameDataQueue.TryPeek(out var frameData) && frameData.FrameTime <= advancedTime)
            {
                var dequeuedFrameData = _frameDataQueue.Dequeue();
                dequeuedFrameData.PerformAllRecords(this);
            }
        }

        public float GetNewAdvancedTime(float advancedTime)
        {
            return advancedTime + (Time.deltaTime * _replayController.ForwardTimeScale);
        }

        public void Exit()
        {
            for (var i = 0; i < _frameDataQueue.Count; i++)
            {
                _frameDataQueue.Dequeue().PerformAllRecords(this);
            }
        }
    }
}
