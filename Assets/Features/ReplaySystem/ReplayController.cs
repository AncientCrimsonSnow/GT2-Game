using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewReplaySystem
{
    public class ReplayController
    {
        private int _currentIndex;

        private int _registrationTick;

        private readonly ReplayManager _replayManager;
        private readonly IReplayable _replayable;
        private bool _isLoop;
        private List<IRecord> _ticks;

        private bool _isRecording;

        public ReplayController(ReplayManager replayManager, IReplayable replayable)
        {
            _replayManager = replayManager;
            _replayable = replayable;
            _ticks = new List<IRecord>();
            RegisterReplayable(replayable);

            _isRecording = true;
        }

        private void RegisterReplayable(IReplayable replayable)
        {
            replayable.ProvideReplayEventFrames += ProvideReplayEventFrame;
        }

        public void UnregisterReplayable(IReplayable replayable)
        {
            replayable.ProvideReplayEventFrames -= ProvideReplayEventFrame;
        }
        
        private void ProvideReplayEventFrame(IRecord record)
        {
            if (!IsRecording())
            {
                Debug.LogWarning("You can only register Records during RecordState");
                return;
            }

            if (_replayManager.AdvancedTicks == _registrationTick)
            {
                Debug.LogWarning("You can only register one record per tick!");
                return;
            }

            //TODO: perform tick
            
            _registrationTick = _replayManager.AdvancedTicks;
            _ticks.Add(record);
        }

        public void StopRecording(bool isLoop)
        {
            _isLoop = isLoop;
            _isRecording = false;
        }
        
        public bool IsRecording()
        {
            return _isRecording;
        }

        public void Tick(float totalTickTime)
        {
            _ticks[_currentIndex].PerformRecordBehaviourForward();
            _currentIndex++;

            if (_currentIndex < _ticks.Count) return;
            if (_isLoop)
            {
                _currentIndex = 0;
            }
            else
            {
                OnReplayCompleted();
            }
        }

        private void OnReplayCompleted()
        {
            //TODO: unregister from replayManager
        }
    }
}
