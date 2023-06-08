using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewReplaySystem
{
    public class ReplayController
    {
        public IReplayable Replayable { get; private set; }
        public bool IsRecording { get; private set; }

        private readonly ReplayManager _replayManager;
        private readonly List<IRecord> _ticks;
        
        private int _registrationTick;
        private int _currentIndex;
        private bool _isLoop;
        

        public ReplayController(ReplayManager replayManager, IReplayable replayable)
        {
            Replayable = replayable;
            _replayManager = replayManager;
            _ticks = new List<IRecord>();
            
            Replayable.ProvideReplayEventFrames += ProvideReplayEventFrame;
            IsRecording = true;
        }

        public void StopRecording(Action onCompleteReplay)
        {
            Replayable.ProvideReplayEventFrames -= ProvideReplayEventFrame;
            IsRecording = false;
            
            _isLoop = isLoop;
        }

        public void Tick()
        {
            if (IsRecording)
            {
                Debug.LogWarning("You can only register Records during ReplayState");
                return;
            }
            
            if (_ticks.Count == 0)
            {
                Debug.LogWarning("There are no ticks registered for replay");
                return;
            }
            
            _ticks[_currentIndex].PerformRecordBehaviourForward();
            _currentIndex++;

            if (_currentIndex < _ticks.Count) return;
            if (_isLoop)
            {
                _currentIndex = 0;
            }
            else
            {
                OnUnregisterReplayController();
            }
        }
        
        private void ProvideReplayEventFrame(IRecord record)
        {
            if (_replayManager.AdvancedTicks == _registrationTick)
            {
                Debug.LogWarning("You can only register one record per tick!");
                return;
            }
            
            _registrationTick = _replayManager.AdvancedTicks;
            _ticks.Add(record);
        }
    }
}
